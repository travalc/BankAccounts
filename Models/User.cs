using System;
using System.ComponentModel.DataAnnotations;
namespace BankAccounts.Models
{
    public abstract class BaseEntity {}

    public class User : BaseEntity
    {
        [Key]
        public int id {get; set;}
        public string firstName {get; set;}
        public string lastName {get; set;}
        public string email {get; set;}
        public string password {get; set;}
        public DateTime created_at {get; set;}
        public DateTime updated_at {get; set;}
    }
}