﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OleLukoje.Models
{
    public enum State
    {
        Active,
        Complited,
        TemporarilyBlocked
    }

    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Column]
        [Required]
        public string Header { get; set; }

        [Column]
        [Required]
        public string Description { get; set; }

        [Column]
        [Required]
        public int Price { get; set; }

        [Column]
        [Required]
        public State StateAd { get; set; }

        [Column]
        [Required]
        public bool SpecialAd { get; set; }

        [Column]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAd { get; set; }

        /// <summary>
        /// Многие-ко-многим с категориями (у категории много обьявлений и у обьявления много категорий)
        /// </summary>
        [Required]
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Связь один-ко-многим с сообщениями (у одного обьявления много сообщений)
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Связь один-ко-многим с файлами (у одного обьявления много файлов)
        /// </summary>
        public virtual ICollection<File> Files { get; set; }

        /// <summary>
        /// Связь один-ко-многим с order
        /// </summary>
        public virtual ICollection<Application> Applications { get; set; }

        public Ad()
        {
            Categories = new List<Category>();
            Comments = new List<Comment>();
            Files = new List<File>();
            Applications = new List<Application>();
        }

        /// <summary>
        /// Связь один-ко-многим с юзером (у одного юзера много обьявлений)
        /// </summary>
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}