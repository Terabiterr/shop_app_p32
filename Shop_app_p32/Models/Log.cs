using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_app_p32.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Message { get; set; }
        [Required]
        public string? Level { get; set; }
        [Required]
        public DateTime? Timestamp { get; set; }
        [Required]
        public string? Exception { get; set; }
        public string? Properties { get; set; }
    }
}
