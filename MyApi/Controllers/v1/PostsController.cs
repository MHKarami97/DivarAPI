using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Contracts;
using Entities.Post;
using Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Base;
using Models.Models;
using Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    public class PostsController : CrudController<PostDto, PostSelectDto, Post>
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<PostImage> _repositoryPostImage;
        private readonly IRepository<Favorite> _repositoryFavorite;
        private readonly ViewsController _viewsController;
        private readonly FilesController _filesController;

        public PostsController(IRepository<Post> repository, IMapper mapper, UserManager<User> userManager, IRepository<PostImage> repositoryImage, IRepository<Favorite> repositoryFavorite, ViewsController viewsController, IPostRepository postRepository, FilesController filesController)
            : base(repository, mapper)
        {
            _userManager = userManager;
            _repositoryPostImage = repositoryImage;
            _repositoryFavorite = repositoryFavorite;
            _viewsController = viewsController;
            _postRepository = postRepository;
            _filesController = filesController;
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        public override Task<ApiResult<List<PostSelectDto>>> Get(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }

        [AllowAnonymous]
        public override async Task<ApiResult<PostSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await base.Get(id, cancellationToken);

            result.Data.IsFavorite = false;

            var isAuthorize = false;

            if (UserIsAutheticated)
            {
                var userId = HttpContext.User.Identity.GetUserId<int>();

                var isFavorite = await _repositoryFavorite.TableNoTracking
                    .AnyAsync(a => a.VersionStatus.Equals(2) && a.PostId.Equals(result.Data.Id) && a.UserId.Equals(userId), cancellationToken);

                if (isFavorite)
                {
                    result.Data.IsFavorite = true;
                }

                isAuthorize = true;
            }

            var images = await _repositoryPostImage.TableNoTracking
            .Where(a => !a.VersionStatus.Equals(2) && a.PostId.Equals(result.Data.Id))
            .ProjectTo<PostImageDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

            result.Data.Images = images;

            await _viewsController.IncreaseView(id, isAuthorize, cancellationToken);

            return result;
        }

        public override async Task<ApiResult<PostSelectDto>> Update(int id, PostDto dto, CancellationToken cancellationToken)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return await base.Update(id, dto, cancellationToken);
            }

            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                Post item = await Repository.TableNoTracking.SingleAsync(a => a.Id.Equals(id), cancellationToken);

                if (!item.UserId.Equals(user.Id))
                {
                    return BadRequest();
                }

                return await base.Update(id, dto, cancellationToken);
            }

            return BadRequest();
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(900_000_000)]
        public override async Task<ApiResult<PostSelectDto>> Create([FromForm] PostDto dto, CancellationToken cancellationToken)
        {
            dto.UserId = HttpContext.User.Identity.GetUserId<int>();

            var result = await base.Create(dto, cancellationToken);

            var files = Request.Form.Files;

            if (files == null)
                return result;

            var imgResult = _filesController.Upload(files);

            if (!imgResult.Status)
                return BadRequest(imgResult.Message);

            var imgSaveResult = await _postRepository
                .AddImage(imgResult.Images, result.Data.Id, cancellationToken);

            return imgSaveResult != 1 ? BadRequest("مشکل در ذخیره عکس ها") : result;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetAllByCatId(CancellationToken cancellationToken, int id, int to = 0)
        {
            return await _postRepository.GetAllByCatId(cancellationToken, id, to);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetSimilar(CancellationToken cancellationToken, int id)
        {
            return await _postRepository.GetSimilar(cancellationToken, id);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetByUserId(int id, CancellationToken cancellationToken)
        {
            return await _postRepository.GetByUserId(cancellationToken, id);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetByStateId(int id, CancellationToken cancellationToken)
        {
            return await _postRepository.GetByStateId(cancellationToken, id);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<ViewShortDto>>> GetView(int id, CancellationToken cancellationToken)
        {
            return await _postRepository.GetView(cancellationToken, id);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetCustom(CancellationToken cancellationToken, int type = 1, int dateType = 1, int count = DefaultTake)
        {
            return await _postRepository.GetCustom(cancellationToken, type, dateType, count);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> Search(string str, CancellationToken cancellationToken)
        {
            return await _postRepository.Search(cancellationToken, str);
        }

        [HttpGet]
        [Authorize(Policy = "WorkerPolicy")]
        public virtual async Task<ApiResult<List<PostShortStatusSelectDto>>> GetByStatus(CancellationToken cancellationToken, bool status = true)
        {
            return await _postRepository.GetByStatus(cancellationToken, status);
        }

        [HttpPost]
        [Authorize(Policy = "WorkerPolicy")]
        public virtual async Task<ApiResult<OkResult>> ChangeStatus(CancellationToken cancellationToken, int id)
        {
            var result = await _postRepository.ChangeStatus(cancellationToken, id);

            return Ok($"وضعیت با موفقیت تغییر کرد {result}");
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<PostShortSelectDto>>> GetShort(CancellationToken cancellationToken)
        {
            return await _postRepository.GetShort(cancellationToken);
        }
    }
}