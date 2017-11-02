using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private BankAccountsContext _context;
        public HomeController(BankAccountsContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            double number = 3.45;
            Console.WriteLine(number.GetType().Name);

            if (HttpContext.Session.GetInt32("user_id") != null)
            {
                string stringId = HttpContext.Session.GetInt32("user_id").ToString();
                Dictionary<string, string> obj = new Dictionary<string, string>() {
                        {"id", stringId}
                    };
                return RedirectToAction("Account", "Home", stringId);
            }
            return View();
        }

        [HttpGet]
        [Route("Account/{id}")]

        public IActionResult Account(string id)
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index");
            }
            else if (HttpContext.Session.GetInt32("user_id").ToString() != id)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int userid = (int)HttpContext.Session.GetInt32("user_id");
                Account account = _context.accounts.Where(item => item.users_id == userid).Include(item => item.user).Include(item => item.transactions).SingleOrDefault();
                TransactionViewModel transactionModel = new TransactionViewModel();
                transactionModel.accounts_id = account.id;
                ViewBag.account = account;
                ViewBag.transactionModel = transactionModel;
                return View();
            }
        }

        [HttpPost]
        [Route("Register")]

        public IActionResult Register(RegisterViewModel model)
        {
            List<User> existingEmails = _context.users.Where(item => item.email == model.email).ToList();
            if (existingEmails.Count > 0)
            {
                TempData["emailError"] = "A user with that email already exists";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User user = new User
                {
                    firstName = model.firstName,
                    lastName = model.lastName,
                    email = model.email,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    password = ""
                };
                user.password = Hasher.HashPassword(user, model.password);
                _context.users.Add(user);
                _context.SaveChanges();

                
                User addedUser = _context.users.SingleOrDefault(item => item.email == model.email);
                Account account = new Account {
                    balance = 0,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    users_id = addedUser.id,
                    user = addedUser
                };
                
                _context.accounts.Add(account);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id", addedUser.id);
                string stringId = addedUser.id.ToString();
                Dictionary<string, string> obj = new Dictionary<string, string>() {
                        {"id", stringId}
                    };
                return RedirectToAction("Account", "Home", obj);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<User> users = _context.users.Where(item => item.email == model.email).ToList();
                if (users.Count < 1)
                {
                    TempData["loginError"] = "No user with that email found";
                    return RedirectToAction("Index");
                }
                User user = users[0];
                var Hasher = new PasswordHasher<User>();
                if (Hasher.VerifyHashedPassword(user, user.password, model.password) != 0)
                {
                    HttpContext.Session.SetInt32("user_id", user.id);
                    string stringId = user.id.ToString();
                    Dictionary<string, string> obj = new Dictionary<string, string>() {
                        {"id", stringId}
                    };
                    return RedirectToAction("Account", "Home", obj);
                }
                else
                {
                    TempData["loginError"] = "That password does not match what is on record";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Route("Transaction")]

        public IActionResult Transaction(TransactionViewModel model)
        {
            string userid = HttpContext.Session.GetInt32("user_id").ToString();
            Dictionary<string, string> obj = new Dictionary<string, string>() {
                {"id", userid}
            };
            if (ModelState.IsValid)
            {
                Account account = _context.accounts.SingleOrDefault(item => item.id == model.accounts_id);
                if (model.amount < 0 && Math.Abs(model.amount) > account.balance)
                {
                    TempData["transactionError"] = "You do not have enough funds for that withdrawal";
                    return RedirectToAction("Account", "Home", obj);
                }
                Transaction transaction = new Transaction {
                    amount = (float)model.amount,
                    accounts_id = model.accounts_id,
                    account = account,
                    created_at = DateTime.Now
                };
                account.balance += (float)model.amount;
                _context.transactions.Add(transaction);
                _context.SaveChanges();

                return RedirectToAction("Account", "Home",  obj);
            }
            else
            {
                return View(model);
            }

        }
    }
}
