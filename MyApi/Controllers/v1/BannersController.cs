using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contracts;
using Entities.More;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Base;
using Models.Models;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    public class BannersController : CrudController<BannerDto, BannerSelectDto, Banner>
    {
        private readonly FilesController _filesController;

        public BannersController(IRepository<Banner> repository, IMapper mapper, FilesController filesController)
            : base(repository, mapper)
        {
            _filesController = filesController;
        }

        [AllowAnonymous]
        public override Task<ApiResult<BannerSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [AllowAnonymous]
        public override Task<ApiResult<List<BannerSelectDto>>> Get(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        public override Task<ApiResult<BannerSelectDto>> Update(int id, BannerDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        [RequestSizeLimit(900_000_000)]
        public override async Task<ApiResult<BannerSelectDto>> Create([FromForm] BannerDto dto, CancellationToken cancellationToken)
        {
            var result = await base.Create(dto, cancellationToken);

            var files = Request.Form.Files;

            if (files == null || files.Count == 0)
                return result;

            var imgResult = _filesController.SingleCreate(files[0]);

            if (!imgResult.Status)
                return BadRequest(imgResult.Message);

            dto.Image = imgResult.Images;

            return await base.Create(dto, cancellationToken);
        }
    }
}