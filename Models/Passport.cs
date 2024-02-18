using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RenewingPassport.Models
{
    public class Passport
    {
        public int Id { get; set; }
        [DisplayName("User Name : ")]
        public string? UserID { get; set; }
        public string? Email { get; set; }
        public DateTime? Date { get; set; }
        public string? PassportImage { get; set; }
        [NotMapped]
        [DisplayName ("Upload Passport Image")]
        public IFormFile? PassportFormFile { get; set; }
        public string? IDImage { get; set; }
        [NotMapped]
        [DisplayName("Upload ID Image")]
        public IFormFile? IDImageFile { get; set; }
        public string? Status { get; set; }
    }
}
