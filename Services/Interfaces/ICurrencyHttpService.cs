using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICurrencyHttpService
    {
        Task<double> GetEuroToUSdRate();
    }
}
