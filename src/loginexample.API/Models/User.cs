using System;
namespace loginexample.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
