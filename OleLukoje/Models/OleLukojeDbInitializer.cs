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
            db.Categories.Add(new Category { Name = "Children" });
            db.Categories.Add(new Category { Name = "Home & Garden" });
            db.Categories.Add(new Category { Name = "Fashion & Style" });
            db.Categories.Add(new Category { Name = "Transport" });
            db.Categories.Add(new Category { Name = "Work" });
            db.Categories.Add(new Category { Name = "Zoo" });
            db.Categories.Add(new Category { Name = "Relaxation" });
            db.Categories.Add(new Category { Name = "Sport" });
            db.Categories.Add(new Category { Name = "Hobby" });
            db.Categories.Add(new Category { Name = "Business & Services" });
            db.Categories.Add(new Category { Name = "Electronics" });
            db.Categories.Add(new Category { Name = "The property" });
            base.Seed(db);
        }
    }
}