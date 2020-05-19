using Data.Contracts;
using Entities.State;
using Models.Base;
using Models.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IStateRepository : IRepository<State>
    {
        Task<ApiResult<List<StateDto>>> GetAllMainCat(CancellationToken cancellationToken);

        Task<ApiResult<List<StateWithSubCatDto>>> GetCategoryWithSub(CancellationToken cancellationToken);

        Task<ApiResult<List<StateDto>>> GetAllByCatId(int id, CancellationToken cancellationToken);
    }
}