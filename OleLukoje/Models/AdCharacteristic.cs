using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class AdCharacteristic
    {
        [Key]
        public int Id { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        [Column]
        [Required]
        public string Description { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}