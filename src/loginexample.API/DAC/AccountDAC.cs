using System;
using loginexample.API.Models;
using System.Linq;

namespace loginexample.API.DAC
{
    public class AccountDAC : IAccountDAC
    {
        private readonly AppDbContext dbContext;

        public AccountDAC(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User GetUser(string username, string password)
        {
            var all = dbContext.Users.All(it => true);
            var user = dbContext.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user;
        }
    }
}
