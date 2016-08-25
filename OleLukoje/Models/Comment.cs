﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OleLukoje.Models
{
    public class Comment
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
        public DateTime DateTimeComment { get; set; }

        public string GetDateTimeString { get { return DateTimeComment.ToString(); } }

        /// <summary>
        /// Связь один-ко-многим с ad
        /// </summary>
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}