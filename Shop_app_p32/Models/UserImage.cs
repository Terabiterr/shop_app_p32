using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_app_p32.Models
{
    public class UserImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ShopUser ShopUser { get; set; } = null!;
    }
}
