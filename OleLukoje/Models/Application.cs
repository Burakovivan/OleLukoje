using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Column]
        [Required]
        public string Header { get; set; }

        [Column]
        public string Text { get; set; }

        [Column]
        public string Contact { get; set; }

        [Column]
        [Required]
        public string UserName { get; set; }

        [Column]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeApplication { get; set; }

        public string GetDateTimeString { get { return DateTimeApplication.ToString(); } }

        /// <summary>
        /// Связь один-ко-многим с Ad
        /// </summary>
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}