using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using loginexample.Web.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace loginexample.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            using (var db = new LiteDatabase(@"Filename=App_Data/mydb.db;mode=Exclusive"))
            {
                var accounts = db.GetCollection<Account>("accounts");

                var hashedPassword = hashPassword(model.Password);
                var account = accounts.Find(x => x.Username == model.Username && x.Password == hashedPassword).FirstOrDefault();

                if (account != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult Add()
        {
            using (var db = new LiteDatabase(@"Filename=App_Data/mydb.db;mode=Exclusive"))
            {
                var accounts = db.GetCollection<Account>("accounts");
                var hashedPassword = hashPassword("P@ssw0rd");
                var account = new Account
                {
                    Username = "ploy",
                    Password = hashedPassword,
                    CreateAt = DateTime.Now
                };
                accounts.Insert(account);
            }

            return RedirectToAction("Login");
        }

        private string hashPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var data = Encoding.ASCII.GetBytes(password);
            var md5data = md5.ComputeHash(data);
            var hashedPassword = Encoding.ASCII.GetString(md5data);
            return hashedPassword;
        }
    }
}
