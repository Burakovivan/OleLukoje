using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OleLukoje.Models
{
    public class File
    {
        [Key]
        public int Id { set; get; }

        [Column]
        [Required]
        public string Path { set; get; }

        /// <summary>
        /// Связь один-ко-многим с обьявлением (у одного обьявления много файлов)
        /// </summary>
        [ForeignKey("Ad")]
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}