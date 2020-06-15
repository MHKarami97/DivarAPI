using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contracts;
using Entities.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Base;
using Models.Models;
using Repositories.Contracts;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    public class StatesController : CrudController<StateCreateDto, StateDto, State>
    {
        private readonly IStateRepository _stateRepository;

        public StatesController(IRepository<State> repository, IMapper mapper, IStateRepository stateRepository)
            : base(repository, mapper)
        {
            _stateRepository = stateRepository;
        }

        [AllowAnonymous]
        public override Task<ApiResult<StateDto>> Get(int id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [AllowAnonymous]
        public override Task<ApiResult<List<StateDto>>> Get(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        public override Task<ApiResult<StateDto>> Update(int id, StateCreateDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        [Authorize(Policy = "WorkerPolicy")]
        public override async Task<ApiResult<StateDto>> Create(StateCreateDto dto, CancellationToken cancellationToken)
        {
            if (dto.ParentStateId == 0 || dto.ParentStateId == null)
                return await base.Create(dto, cancellationToken);

            var isParentExist = await Repository.TableNoTracking.SingleOrDefaultAsync(a => a.Id.Equals(dto.ParentStateId), cancellationToken);

            if (isParentExist == null)
                return BadRequest("دسته مادر موجود نمی باشد");

            if (!isParentExist.ParentStateId.Equals(0) || isParentExist.ParentStateId != null)
                return BadRequest("امکان دسته بندی بیشتر از دو مرحله امکان پذیر نمی باشد");

            return await base.Create(dto, cancellationToken);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<StateDto>>> GetAllMainState(CancellationToken cancellationToken)
        {
            return await _stateRepository.GetAllMainState(cancellationToken);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<StateWithSubCatDto>>> GetStateWithSub(CancellationToken cancellationToken)
        {
            return await _stateRepository.GetStateWithSub(cancellationToken);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<ShortStateDto>>> GetSubState(CancellationToken cancellationToken)
        {
            return await _stateRepository.GetSubState(cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<List<StateDto>>> GetAllByStateId(int id, CancellationToken cancellationToken)
        {
            return await _stateRepository.GetAllByStateId(id, cancellationToken);
        }
    }
}