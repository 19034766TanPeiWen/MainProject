using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainProject.Controllers
{
    public class FaceController : Controller
    {

        public async Task<string> Test()
        {
            var personGroupId = "c200y2019";

            string fullpath = Path.Combine(_env.WebRootPath, "webcam\\FrankieDesmond.jpg");

            using (Stream fs = System.IO.File.OpenRead(fullpath))
            {
                var faces = await faceClient.Face.DetectWithStreamAsync(fs);
                var faceIds = faces.Select(face => face.FaceId).ToArray();
                var results = await faceClient.Face.IdentifyAsync(faceIds.OfType<Guid?>().ToList(), personGroupId);
                foreach (var identifyResult in results)
                {
                    Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Count == 0)
                    {
                        Debug.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var confidence = identifyResult.Candidates[0].Confidence;
                        var person = await faceClient.PersonGroupPerson.GetAsync(personGroupId, candidateId);
                        Debug.WriteLine("Identified as {0} ({1})", person.Name, confidence);
                    }
                }
            }

            return "";
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public async Task<string> FaceLogin(IFormFile upimage)
        {
            string outcome = "no";
            string fullpath = Path.Combine(_env.WebRootPath, @"webcam\login.jpg");
            using (FileStream fs = new FileStream(fullpath, FileMode.Create))
            {
                upimage.CopyTo(fs);
                fs.Close();
            }
            var personGroupId = "c200y2019";

            using (Stream fs = System.IO.File.OpenRead(fullpath))
            {
                var faces = await faceClient.Face.DetectWithStreamAsync(fs);
                if (faces.Count == 0)
                {
                    TempData["Message"] = "No person in the Webcam";
                }
                else if (faces.Count > 1)
                {
                    TempData["Message"] = "One person at a time please";
                }
                else
                {
                    var faceIds = faces.Select(face => face.FaceId).ToArray();
                    var results = await faceClient.Face.IdentifyAsync(faceIds.OfType<Guid?>().ToList(), personGroupId);
                    foreach (var identifyResult in results)
                    {
                        Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                        if (identifyResult.Candidates.Count == 0)
                        {
                            Debug.WriteLine("No one identified");
                            TempData["Message"] = "No One Identified";
                        }
                        else
                        {
                            // Get top 1 among all candidates returned
                            var candidateId = identifyResult.Candidates[0].PersonId;
                            var confidence = identifyResult.Candidates[0].Confidence;
                            var person = await faceClient.PersonGroupPerson.GetAsync(personGroupId, candidateId);
                            Debug.WriteLine("Identified as {0} ({1})", person.Name, confidence);

                            TempData["Message"] = $"Identified as {person.Name} ({confidence})";
                            outcome = "yes";                           

                        }
                    }
                }
            }

            return outcome;

        }

        
        public string FaceLoginOrg(IFormFile upimage)
        {
            string fullpath = Path.Combine(_env.WebRootPath, "login/user.jpg");
            using (FileStream fs = new FileStream(fullpath, FileMode.Create))
            {
                upimage.CopyTo(fs);
                fs.Close();
            }

            string imagePath = @"/login/user.jpg";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", FACEAPIKEY);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            string uri = FACEAPIENDPOINT + "/detect?" + requestParameters;
            var fileInfo = _env.WebRootFileProvider.GetFileInfo(imagePath);
            var byteData = GetImageAsByteArray(fileInfo.PhysicalPath);
            string contentStringFace = string.Empty;
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                var response = client.PostAsync(uri, content).Result;

                // Get the JSON response.
                contentStringFace = response.Content.ReadAsStringAsync().Result;
            }

            var expConverter = new ExpandoObjectConverter();
            dynamic faceList = JsonConvert.DeserializeObject<List<ExpandoObject>>(contentStringFace, expConverter);
            if (faceList.Count == 0)
            {
                TempData["json"] = "No Face detected";
            }
            else
            {
                TempData["json"] = JsonPrettyPrint(contentStringFace);
            }

            return contentStringFace;

        }




        public IActionResult Next()
        {
            return View();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            return bytes;
        }
        
        

        static string JsonPrettyPrint(string json)
        {
            string INDENT_STRING = "    ";
            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null ? openChar.Length > 1 ? openChar : closeChar : lineBreak;
            return String.Concat(result);
        }

        public FaceController(IWebHostEnvironment environment,
                        IConfiguration config)
        {
            _env = environment;
            FACEAPIKEY = config.GetSection("FaceConfig").GetValue<string>("SubscriptionKey");
            FACEAPIENDPOINT = config.GetSection("FaceConfig").GetValue<string>("EndPoint");
            faceClient = new FaceClient(
               new ApiKeyServiceClientCredentials(FACEAPIKEY),
               new System.Net.Http.DelegatingHandler[] { });
            //faceClient.Endpoint = FACEAPIENDPOINT;
            faceClient.Endpoint = "https://southeastasia.api.cognitive.microsoft.com";

        }

        private IWebHostEnvironment _env;

        //const string faceApiKey = "0d7af552cce6469999a42e0383d1edd9";
        //const faceApiEndPoint = "https://southeastasia.api.cognitive.microsoft.com/face/v1.0";

        private readonly string FACEAPIKEY;
        private readonly string FACEAPIENDPOINT;
        private readonly IFaceClient faceClient;

        private bool AuthenticateUser(string confidence,
                                    out ClaimsPrincipal principal)
        {
            principal = null;

            string sql = @"SELECT * FROM UserDetails
                         WHERE UserName = '{0}'";
            string select = String.Format(sql,confidence);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, confidence),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["UserName"].ToString()),
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme));
                return true;
            }
            return false;
        }
    }
}

