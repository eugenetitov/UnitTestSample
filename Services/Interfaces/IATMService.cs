using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IATMService
    {
        Task Withdraw(int amount, Currency currency, string address, Country country);
        Task Withdraw(int amount, string address);
    }
}
