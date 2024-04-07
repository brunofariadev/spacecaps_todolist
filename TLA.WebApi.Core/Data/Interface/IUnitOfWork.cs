using System.Threading.Tasks;

namespace TLA.WebApi.Core.Data.Interface
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
