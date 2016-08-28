using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OleLukoje.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Column]
        public string Text { get; set; }

        [Column]
        public string Dignity { get; set; }

        [Column]
        public string Defects { get; set; }

        [Column]
        public int? Stars { get; set; }

        [Column]
        [Required]
        public string UserName { get; set; }

        [Column]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeReview { get; set; }

        public string GetDateTime { get { return DateTimeReview.ToString(); } }

        /// <summary>
        /// Связь один-ко-многим с ad
        /// </summary>
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
