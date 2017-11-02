using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BankAccounts.Models
{
        public class Transaction : BaseEntity
    {
        public int id {get; set;}
        public float amount {get; set;}
        public int accounts_id {get; set;}

        [ForeignKey("accounts_id")]
        public Account account {get; set;}

        public DateTime created_at {get; set;}
    }
}