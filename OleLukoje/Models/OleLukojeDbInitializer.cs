using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class OleLukojeDbInitializer : CreateDatabaseIfNotExists<OleLukojeContext>
    {
        protected override void Seed(OleLukojeContext db)
        {
            db.Categories.Add(new Category { Name = "Sport" });
            db.Categories.Add(new Category { Name = "Home" });
            db.Categories.Add(new Category { Name = "Garden" });
            db.Categories.Add(new Category { Name = "Electronics" });
            base.Seed(db);
        }
    }
}