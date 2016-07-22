using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class OleLukojeContext : DbContext
    {
        public OleLukojeContext()
            : base("DefaultConnection")
        { }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}