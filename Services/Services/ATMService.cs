using Repositories;
using Repositories.Repositories;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using Repositories.Models;
using Repositories.Interfaces;

namespace Services.Services
{
    public class ATMService : IATMService
    {
        //private readonly ATMRepository _aTMRepository = new ATMRepository(new ApplicationDbContext());
        private ICurrencyHttpService _currencyHttpService;
        private IATMRepository _aTMRepository;

        public ATMService(ICurrencyHttpService currencyHttpService, IATMRepository aTMRepository)
        {
            _currencyHttpService = currencyHttpService;
            _aTMRepository = aTMRepository;
        }

        public async Task InitATM()
        {
            //await _aTMRepository.CreateAsync(new BankTransaction() { Amount = 10000, ATMAddress = "Prospekt Nauki 37", IsDebit = true, TransactionDate = DateTime.Now });
        }

        public async Task Withdraw(int amount, Currency currency, string address, Country country)
        {
            //if we withdraw euro we need to conver it to usd, out ATM suppors only USD
            if (currency.Equals(Currency.EURO))
            {
                var rate = await _currencyHttpService.GetEuroToUSdRate();
                amount = Convert.ToInt32(amount * rate);
            }

            //if country is not USA then we need to add tax 1% from amount

            if (!country.Equals(Country.USA))
            {
                amount = amount - Convert.ToInt32(amount * .1);
            }

            await Withdraw(amount, address);
        }

        public virtual async Task Withdraw(int amount, string address)
        {
            if (amount <= 0)
            {
                throw new Exception("Amount cant be less or equals then zero");
            }
            if (amount % 100 > 0)
            {
                throw new Exception("Amount is incorrect");
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new Exception("Address is incorrect");
            }

            var totalAmount = _aTMRepository.All.Sum(x => x.Amount);

            if (totalAmount < amount)
            {
                throw new Exception("Not enough money");
            }
            await _aTMRepository.CreateAsync(new BankTransaction()
            {
                Amount = -amount,
                ATMAddress = address,
                IsDebit = false,
                TransactionDate = DateTime.Now
            });
        }
    }
}
