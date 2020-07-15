using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities.Post;
using Models.Base;
using Models.Models;
using Repositories.Contracts;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository, IScopedDependency, IBaseRepository
    {
        public CommentRepository(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<ApiResult<List<CommentSelectDto>>> GetPostComments(int id, CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.PostId.Equals(id))
                .ProjectTo<CommentSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<CommentSelectDto>>> GetLastComments(CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2))
                .OrderByDescending(a => a.Time)
                .ProjectTo<CommentSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<DateTimeOffset> Create(CommentDto dto, CancellationToken cancellationToken)
        {
            var lastComment = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.PostId.Equals(dto.PostId) && a.CreatorId.Equals(dto.CreatorId))
                .OrderByDescending(a => a.Time)
                .Select(a => a.Time)
                .FirstAsync(cancellationToken);

            return lastComment;
        }

        public async Task<ApiResult<List<CommentShortSelectDto>>> GetByUser(CancellationToken cancellationToken, int userId)
        {
            var list = await TableNoTracking
                .Include(a => a.Post)
                .Where(a => !a.VersionStatus.Equals(2) &&
                           (a.CreatorId.Equals(userId) || a.Post.UserId.Equals(userId)) &&
                            !a.Post.VersionStatus.Equals(2))
                .OrderByDescending(a => a.Time)
                .ProjectTo<CommentShortSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

           var result= list.GroupBy(n => new {n.PostId, n.CreatorId})
                .Select(a => a.First()).ToList();

            return result;
        }

        public async Task<ApiResult<CommentPostShortSelectDto>> GetByPost(CancellationToken cancellationToken, int userId, int postId, int creatorId)
        {
            List<CommentPostSelectDto> list;

            if (creatorId == userId)
            {
                list = await TableNoTracking
                   .Include(a => a.Post)
                   .Where(a => !a.VersionStatus.Equals(2) &&
                               a.PostId.Equals(postId) &&
                               a.CreatorId.Equals(creatorId) &&
                               !a.Post.VersionStatus.Equals(2))
                   .OrderByDescending(a => a.Time)
                   .ProjectTo<CommentPostSelectDto>(Mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            }
            else
            {
                list = await TableNoTracking
                   .Include(a => a.Post)
                   .Where(a => !a.VersionStatus.Equals(2) &&
                               a.PostId.Equals(postId) &&
                               a.CreatorId.Equals(creatorId) &&
                               a.Post.UserId.Equals(userId) &&
                               !a.Post.VersionStatus.Equals(2))
                   .OrderByDescending(a => a.Time)
                   .ProjectTo<CommentPostSelectDto>(Mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            }

            return new CommentPostShortSelectDto
            {
                UserId = userId,
                Comment = list
            };
        }
    }
}