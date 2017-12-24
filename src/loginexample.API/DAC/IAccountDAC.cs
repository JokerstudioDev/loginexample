using System;
using loginexample.API.Models;

namespace loginexample.API.DAC
{
    public interface IAccountDAC
    {
        User GetUser(string username, string password);
    }
}
