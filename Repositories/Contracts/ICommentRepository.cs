using Data.Contracts;
using Entities.Post;
using Models.Base;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<bool> GetLastComment(int postId, int creatorId, CancellationToken cancellationToken);

        Task<ApiResult<List<CommentSelectDto>>> GetPostComments(int id, CancellationToken cancellationToken);

        Task<ApiResult<List<CommentSelectDto>>> GetLastComments(CancellationToken cancellationToken);

        Task<DateTimeOffset> Create(CommentDto dto, CancellationToken cancellationToken);

        Task<ApiResult<List<CommentShortSelectDto>>> GetByUser(CancellationToken cancellationToken, int userId);

        Task<ApiResult<CommentPostShortSelectDto>> GetByPost(CancellationToken cancellationToken, int userId, int id, int creatorId);
    }
}