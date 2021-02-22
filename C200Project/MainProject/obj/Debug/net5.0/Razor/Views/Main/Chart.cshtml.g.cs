#pragma checksum "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "51c91ed809404a3915705ca7ea53d000c4c0bffc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Main_Chart), @"mvc.1.0.view", @"/Views/Main/Chart.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\_ViewImports.cshtml"
using System.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\_ViewImports.cshtml"
using MainProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"51c91ed809404a3915705ca7ea53d000c4c0bffc", @"/Views/Main/Chart.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"695f351203188e57b8e5ee6b66fb90e4bb3e7c0b", @"/Views/_ViewImports.cshtml")]
    public class Views_Main_Chart : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("body"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            DefineSection("MoreScripts", async() => {
                WriteLiteral("\r\n    <script language=\"javascript\">\r\n\r\n      $(document).ready(function () {\r\n         new Chart(document.getElementById(\"chart\"), {\r\n             type: \'");
#nullable restore
#line 6 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
               Write(ViewData["Chart"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\',\r\n            data: {\r\n               labels: ");
#nullable restore
#line 8 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                  Write(Json.Serialize(ViewData["Labels"]));

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n               datasets: [{\r\n                  label: \'");
#nullable restore
#line 10 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                     Write(ViewData["Legend"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\',\r\n                  data: ");
#nullable restore
#line 11 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                   Write(Json.Serialize(ViewData["Data"]));

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n                  backgroundColor: ");
#nullable restore
#line 12 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                              Write(Json.Serialize(ViewData["Colors"]));

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n                  fill: false\r\n               }]\r\n            },\r\n            options: {\r\n               responsive: false,\r\n               legend: {\r\n                  display: ");
#nullable restore
#line 19 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                      Write(ViewData["ShowLegend"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n               },\r\n               title: {\r\n                  display: true,\r\n                  text: \'");
#nullable restore
#line 23 "D:\RP\Year 2 Sem 2\C200\web_page\Code\C200Project\MainProject\Views\Main\Chart.cshtml"
                    Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\'\r\n                },\r\n                plugins: {\r\n                    labels: {\r\n                        render: \'value\'\r\n                    }\r\n                }\r\n            }\r\n         });\r\n      });\r\n\r\n    </script>\r\n\r\n");
            }
            );
            WriteLiteral("<style>\r\n    .body {\r\n        background-image: linear-gradient(150deg,#FFFFFF,#DDF3FB);\r\n        height: 100vh;\r\n        width: 100%;\r\n    }\r\n\r\n</style>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "51c91ed809404a3915705ca7ea53d000c4c0bffc6447", async() => {
                WriteLiteral("\r\n    <div class=\"container\">\r\n        <canvas id=\"chart\" width=\"600\" height=\"400\"></canvas>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
