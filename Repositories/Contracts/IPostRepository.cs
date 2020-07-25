using Data.Contracts;
using Entities.Post;
using Models.Base;
using Models.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sieve.Models;

namespace Repositories.Contracts
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<ApiResult<List<PostShortSelectDto>>> GetAllByCatId(CancellationToken cancellationToken, int id, SieveModel sieveModel);

        Task<ApiResult<List<PostShortSelectDto>>> GetSimilar(CancellationToken cancellationToken, int id);

        Task<ApiResult<List<PostShortSelectDto>>> GetByUserId(CancellationToken cancellationToken, int id);

        Task<ApiResult<List<PostShortSelectDto>>> GetUserPosts(CancellationToken cancellationToken, int id);

        Task<ApiResult<List<PostShortSelectDto>>> GetByStateId(CancellationToken cancellationToken, int id, SieveModel sieveModel);

        Task<ApiResult<List<PostShortSelectDto>>> GetBySubStateId(CancellationToken cancellationToken, int id);

        Task<ApiResult<List<ViewShortDto>>> GetView(CancellationToken cancellationToken, int id);

        Task<ApiResult<List<PostShortSelectDto>>> GetCustom(CancellationToken cancellationToken, int type, int dateType, int count);

        Task<ApiResult<List<PostShortSelectDto>>> Search(CancellationToken cancellationToken, string str, SieveModel sieveModel);

        Task<ApiResult<List<PostShortStatusSelectDto>>> GetByStatus(CancellationToken cancellationToken, bool status);

        Task<ApiResult<List<PostShortSelectDto>>> GetShort(CancellationToken cancellationToken, SieveModel sieveModel);

        Task<int> AddImage(List<string> images, int postId, CancellationToken cancellationToken);

        Task<bool> ChangeStatus(CancellationToken cancellationToken, int id);

        Task<ApiResult<PostSelectEditDto>> GetByIdForEdit(CancellationToken cancellationToken, int id);
    }
}