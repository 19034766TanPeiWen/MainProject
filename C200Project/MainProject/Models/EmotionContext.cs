﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EmotionContext : DbContext
    {
        public EmotionContext(DbContextOptions<EmotionContext> options)
            : base(options)
        {
        }

        public DbSet<EmotionData> emotitonData { get; set; }

    }
}
