using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MainProject.Models
{
    public class Chart
    {
            //public int id { get; set; }

            //public string username { get; set; }

            public double Anger { get; set; }

            public double Contempt { get; set; }

            public double Disgust { get; set; }

            public double Fear { get; set; }

            public double Happiness { get; set; }

            public double Neutral { get; set; }

            public double Sadness { get; set; }

            public double Surprise { get; set; }
    }
}
