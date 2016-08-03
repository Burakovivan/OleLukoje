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

        [Column]
        [Required]
        public string UserName { get; set; }

        [Column]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeMessage { get; set; }

        public string GetDateTimeString { get { return DateTimeMessage.ToString(); } }

        /// <summary>
        /// Связь один-ко-многим с обьявлением (у одного обьявления много сообщений)
        /// </summary>
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
