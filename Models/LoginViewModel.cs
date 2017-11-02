using System.ComponentModel.DataAnnotations;
namespace BankAccounts.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required]
        public string email {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string password {get; set;}

    }
}