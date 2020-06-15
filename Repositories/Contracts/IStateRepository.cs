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
        Task<ApiResult<List<StateDto>>> GetAllMainState(CancellationToken cancellationToken);

        Task<ApiResult<List<StateWithSubCatDto>>> GetStateWithSub(CancellationToken cancellationToken);

        Task<ApiResult<List<ShortStateDto>>> GetSubState(CancellationToken cancellationToken);
        
        Task<ApiResult<List<StateDto>>> GetAllByStateId(int id, CancellationToken cancellationToken);
    }
}