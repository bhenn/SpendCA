using System;
using System.ComponentModel.DataAnnotations;

namespace SpendCA.Models
{
    public class Base
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}