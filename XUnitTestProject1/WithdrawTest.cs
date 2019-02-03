using Moq;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class WithdrawTest
    {
        public WithdrawTest()
        {

        }

        [Fact]
        public async void WithdrawEmptyAdress()
        {
            try
            {
                var currencyHttpServiceMock = new Mock<ICurrencyHttpService>();
                currencyHttpServiceMock.Setup(m => m.GetEuroToUSdRate()).Returns(Task.Run(() => { return 10.0; }));

                var aTMRepositoryMock = new Mock<IATMRepository>();
                aTMRepositoryMock.Setup(m => m.CreateAsync(new BankTransaction())).Returns(Task.Run(() => { return new BankTransaction(); }));

                ATMService service = new ATMService(currencyHttpServiceMock.Object, aTMRepositoryMock.Object);
                await service.Withdraw(100, Currency.EURO, "", Country.Germany);
                Assert.True(false, "Test failed");
            }
            catch (Exception ex)
            {
                Assert.Equal("Address is incorrect", ex.Message);
            }
        }

        [Fact]
        public async void WithdrawRateNotCorrect()
        {
            try
            {
                var currencyHttpServiceMock = new Mock<ICurrencyHttpService>();
                currencyHttpServiceMock.Setup(m => m.GetEuroToUSdRate()).Returns(Task.Run(() => { return 0.0; }));

                var aTMRepositoryMock = new Mock<IATMRepository>();
                aTMRepositoryMock.Setup(m => m.CreateAsync(new BankTransaction())).Returns(Task.Run(() => { return new BankTransaction(); }));

                ATMService service = new ATMService(currencyHttpServiceMock.Object, aTMRepositoryMock.Object);
                await service.Withdraw(100, Currency.EURO, "address", Country.Germany);
                Assert.True(false, "Test failed");
            }
            catch (Exception ex)
            {
                Assert.Equal("Amount cant be less or equals then zero", ex.Message);
            }
        }

        [Fact]
        public async void WithdrawSuccessful()
        {
            try
            {
                var currencyHttpServiceMock = new Mock<ICurrencyHttpService>();
                currencyHttpServiceMock.Setup(m => m.GetEuroToUSdRate()).Returns(Task.Run(() => { return 10.0; }));

                var aTMRepositoryMock = new Mock<IATMRepository>();
                aTMRepositoryMock.Setup(m => m.CreateAsync(new BankTransaction())).Returns(Task.Run(() => { return new BankTransaction(); }));

                var list = new List<BankTransaction>();
                list.Add(new BankTransaction { Amount = 900 });
                aTMRepositoryMock.Setup(m => m.All).Returns(list.AsQueryable());

                ATMService service = new ATMService(currencyHttpServiceMock.Object, aTMRepositoryMock.Object);
                await service.Withdraw(100, Currency.EURO, "address", Country.Germany);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
