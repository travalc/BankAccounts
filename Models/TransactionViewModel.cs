using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class TransactionViewModel : BaseEntity
    {
        [Required]
        public float amount {get; set;}
        public int accounts_id {get; set;}

    }
}