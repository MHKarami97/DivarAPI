using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities.State;
using Models.Base;
using Models.Models;
using Repositories.Contracts;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class StateRepository : Repository<State>, IStateRepository, IScopedDependency, IBaseRepository
    {
        public StateRepository(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<ApiResult<List<StateDto>>> GetAllMainState(CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.ParentStateId.Equals(0) || a.ParentStateId == null)
                .ProjectTo<StateDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<StateWithSubCatDto>>> GetStateWithSub(CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.ParentStateId.Equals(0) || a.ParentStateId == null)
                .Include(a => a.ChildStates)
                .ProjectTo<StateWithSubCatDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<ShortStateDto>>> GetSubState(CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.ParentStateId != null)
                .ProjectTo<ShortStateDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<StateDto>>> GetAllByStateId(int id, CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.ParentStateId.Equals(id))
                .ProjectTo<StateDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}