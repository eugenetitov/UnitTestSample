using Moq;
using Repositories.Models;
using Services.Interfaces;
using Services.Services;
using System;
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
            var currencyHttpServiceMock = new Mock<ICurrencyHttpService>();
            var mock = new Mock<ATMService>(currencyHttpServiceMock.Object);
            mock.Setup(p => p.Withdraw(It.IsAny<int>(), It.IsAny<string>())).Callback<int, string>((i, j) => CheckValues(i, j));
            var aTMService = mock.Object;

            try
            {
                await aTMService.Withdraw(2000, Currency.USD, "", Country.Germany);
                throw new Exception();
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

                var aTMServiceMock = new Mock<ATMService>(currencyHttpServiceMock.Object);
                aTMServiceMock.Setup(p => p.Withdraw(It.IsAny<int>(), It.IsAny<string>()))
                    .Callback<int, string>((i, j) => CheckValues(i, j));
                var aTMService = aTMServiceMock.Object;
                await aTMService.Withdraw(100, Currency.EURO, "", Country.Germany);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CheckValues(int amount, string address)
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
        }
    }
}
