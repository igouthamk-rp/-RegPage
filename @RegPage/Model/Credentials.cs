using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _RegPage.Model
{
    [BindProperties]
    public class Credentials
    {
        public Credentials()
        {
            
        }

        [Required]
        [Key]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Salutation { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfPassword { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage ="This feilds needs to be checked")]
        public bool IsChecked { get; set; }

        
        public DateTime ModDate { get; set; } = DateTime.Now;


    }
}
