using System.ComponentModel.DataAnnotations;

namespace SpendCA.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
