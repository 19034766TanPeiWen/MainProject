using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1;

namespace MainProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionUserController : ControllerBase
    {
        // GET api/emotion
        [HttpGet]

        public IEnumerable<EmotionUser> Get()
        {
            List<EmotionUser> dbList = DBUtl.GetList<EmotionUser>("SELECT * FROM EmotionUserData");
            return dbList;
        }

        
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {

            List<EmotionUser> dbList = DBUtl.GetList<EmotionUser>($"SELECT * FROM EmotionUserData WHERE username='{name}'");
            if (dbList.Count > 0)
                return Ok(dbList);
            else
                return NotFound();
        }

        // POST api/emotion
        [HttpPost]
        public IActionResult Post([FromBody] EmotionUser rec)
        {
            if (rec == null)
            {
                return BadRequest();
            }

            string sqlInsert = @"INSERT INTO EmotionUserData(username, Anger, Contempt, Disgust, Fear, Happiness, Neutral, Sadness, Surprise) VALUES
                        ('{0}',{1},{2},{3},{4},{5},{6},{7},{8});";
            if (DBUtl.ExecSQL(sqlInsert,rec.username, rec.Anger, rec.Contempt, rec.Disgust, rec.Fear, rec.Happiness, rec.Neutral, rec.Sadness, rec.Surprise) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] EmotionUser rec)
        {
            if (rec == null || id == null)
            {
                return BadRequest();
            }

            string sql = @"UPDATE EmotionUserData 
                           SET username = '{1}'
                               Anger= '{2}',
                               Contempt ='{3}',
                               Disgust = '{4}',
                               Fear = '{5}',
                               Happiness = '{6}',
                               Neutral = '{7}',
                               Sadness = '{8}',
                               Surprise = '{9}'
                         WHERE ID ={0}";
            string update = String.Format(sql, rec.ID, rec.username, rec.Anger, rec.Contempt, rec.Disgust, rec.Fear, rec.Happiness, rec.Neutral, rec.Sadness, rec.Surprise);
            if (DBUtl.ExecSQL(update) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            string sql = @"DELETE EmotionUserData 
                         WHERE ID={0}";
            string delete = String.Format(sql, id);
            if (DBUtl.ExecSQL(delete) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });

        }
    }
}

