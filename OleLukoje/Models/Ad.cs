using System;
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
        /// Многие-ко-многим с категориями
        /// </summary>
        [Required]
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Многие-ко-многим с характеристиками
        /// </summary>
        public virtual ICollection<AdCharacteristic> AdCharacteristics { get; set; }

        /// <summary>
        /// Связь один-ко-многим с сообщениями 
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Связь один-ко-многим с файлами
        /// </summary>
        public virtual ICollection<File> Files { get; set; }

        /// <summary>
        /// Связь один-ко-многим с order
        /// </summary>
        public virtual ICollection<Application> Applications { get; set; }

        public Ad()
        {
            Categories = new List<Category>();
            AdCharacteristics = new List<AdCharacteristic>();
            Reviews = new List<Review>();
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