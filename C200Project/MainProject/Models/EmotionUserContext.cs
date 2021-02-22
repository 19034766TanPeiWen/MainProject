using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EmotionUserContext : DbContext
    {
        public EmotionUserContext(DbContextOptions<EmotionUserContext> options)
            : base(options)
        {
        }

        public DbSet<EmotionUser> emotitonData { get; set; }

    }
}
