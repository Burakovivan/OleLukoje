using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OleLukoje.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Column]
        [Required]
        public string Text { get; set; }

        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
