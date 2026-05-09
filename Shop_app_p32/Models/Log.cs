using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_app_p32.Models
{

    /*
     Таблицю логів потрібно додати вручну без міграцій
    CREATE TABLE Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Exception NVARCHAR(MAX) NULL,
    Level NVARCHAR(50) NULL,
    Message NVARCHAR(MAX) NULL,
    Properties NVARCHAR(MAX) NULL,
    Timestamp DATETIME2 NOT NULL
);
     */
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
