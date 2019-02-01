using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CurrencyHttpService : ICurrencyHttpService
    {
        public async Task<double> GetEuroToUSdRate() {
            //Assume we do a http request to goverment api to get rate 
            return 1.4;
        } 
    }
}
