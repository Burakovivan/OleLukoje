using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Многие-ко-многим с обьявлениями (у категории много обьявлений и у обьявления много категорий)
        /// </summary>
        public virtual ICollection<Ad> Ads { get; set; }
        public Category()
        {
            Ads = new List<Ad>();
        }
    }
}