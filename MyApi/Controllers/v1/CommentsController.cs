using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Utilities;
using Models.Models;
using Data.Contracts;
using Entities.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Base;
using Repositories.Contracts;
using Services.Security;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    public class CommentsController : CrudController<CommentDto, CommentSelectDto, Comment>
    {
        private readonly ISecurity _security;
        private readonly IRepository<Post> _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentsController(IRepository<Comment> repository, IMapper mapper, ICommentRepository commentRepository, ISecurity security, IRepository<Post> postRepository)
            : base(repository, mapper)
        {
            _commentRepository = commentRepository;
            _security = security;
            _postRepository = postRepository;
        }

        [NonAction]
        public override Task<ApiResult<CommentSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        public override Task<ApiResult<List<CommentSelectDto>>> Get(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<ApiResult<List<CommentSelectDto>>> GetPostComments(int id, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetPostComments(id, cancellationToken);
        }

        [HttpGet]
        [Authorize(Policy = "WorkerPolicy")]
        public async Task<ApiResult<List<CommentSelectDto>>> GetLastComments(CancellationToken cancellationToken)
        {
            return await _commentRepository.GetLastComments(cancellationToken);
        }

        [HttpGet]
        public async Task<ApiResult<List<CommentShortSelectDto>>> GetByUser(CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.Identity.GetUserId<int>();

            return await _commentRepository.GetByUser(cancellationToken, userId);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<CommentPostShortSelectDto>> GetByPost(CancellationToken cancellationToken, int id, int creatorId)
        {
            var userId = HttpContext.User.Identity.GetUserId<int>();

            return await _commentRepository.GetByPost(cancellationToken, userId, id, creatorId);
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        public override Task<ApiResult<CommentSelectDto>> Update(int id, CommentDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }

        public override async Task<ApiResult<CommentSelectDto>> Create(CommentDto dto, CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.Identity.GetUserId<int>();

            dto.CreatorId = userId;

            var post =
                await _postRepository
                    .TableNoTracking
                    .SingleAsync(a => a.Id.Equals(dto.PostId), cancellationToken);

            dto.Witch = post.UserId.Equals(userId) ? 2 : 1;

            if (!_security.TimeCheck(await _commentRepository.Create(dto, cancellationToken)))
                return BadRequest("لطفا کمی صبر کنید و بعد نظر بدهید");

            return await base.Create(dto, cancellationToken);
        }
    }
}