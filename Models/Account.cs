using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BankAccounts.Models
{
        public class Account : BaseEntity
    {
        public int id {get; set;}
        public float balance {get; set;}
        public int users_id {get; set;}

        [ForeignKey("users_id")]
        public User user {get; set;}
        public List<Transaction> transactions {get; set;}
        public DateTime created_at {get; set;}
        public DateTime updated_at {get; set;}
    }
}