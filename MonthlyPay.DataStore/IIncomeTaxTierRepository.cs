using System.Collections.Generic;
using System.Threading.Tasks;

using MonthlyPay.DomainModels;

namespace MonthlyPay.DataStore
{
    public interface IIncomeTaxTierRepository
    {
        Task<List<IIncomeTaxTier>> GetIncomeTaxTiersAsync();
    }
}
