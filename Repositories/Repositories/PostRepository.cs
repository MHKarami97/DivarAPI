using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Utilities;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Repositories;
using Entities.Post;
using Entities.User;
using Models.Base;
using Models.Models;
using Repositories.Contracts;
using System.Collections.Generic;
using System.Data;

namespace Repositories.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository, IScopedDependency, IBaseRepository
    {
        private readonly IRepository<Comment> _repositoryComment;
        private readonly IRepository<PostImage> _repositoryImage;
        private readonly IRepository<View> _repositoryView;

        public PostRepository(ApplicationDbContext dbContext, IMapper mapper, IRepository<Comment> repositoryComment, IRepository<View> repositoryView, IRepository<PostImage> repositoryImage)
            : base(dbContext, mapper)
        {
            _repositoryComment = repositoryComment;
            _repositoryView = repositoryView;
            _repositoryImage = repositoryImage;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetAllByCatId(CancellationToken cancellationToken, int id, int to = 0)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.CategoryId.Equals(id) && a.IsConfirm)
                .OrderByDescending(a => a.Time)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                //.Take(DefaultTake + to)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetSimilar(CancellationToken cancellationToken, int id)
        {
            var post = await TableNoTracking
                .Where(a => a.Id.Equals(id))
                .SingleAsync(cancellationToken);

            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.CategoryId.Equals(post.CategoryId) && a.IsConfirm)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                //.Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<PostSelectEditDto>> GetByIdForEdit(CancellationToken cancellationToken, int id)
        {
            var item = await TableNoTracking
                .Where(a => a.Id.Equals(id))
                .ProjectTo<PostSelectEditDto>(Mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

            return item;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetByUserId(CancellationToken cancellationToken, int id)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.UserId.Equals(id) && a.IsConfirm)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                //.Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetByStateId(CancellationToken cancellationToken, int id)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.StateId.Equals(id) && a.IsConfirm)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                //.Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetBySubStateId(CancellationToken cancellationToken, int id)
        {
            var list = await TableNoTracking
                .Include(a => a.State)
                .Where(a => !a.VersionStatus.Equals(2) &&
                            a.State.ParentStateId.Equals(id) && a.IsConfirm)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                //.Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<ViewShortDto>>> GetView(CancellationToken cancellationToken, int id)
        {
            var result = await _repositoryView.TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.PostId.Equals(id))
                .OrderByDescending(a => a.Time)
                .ProjectTo<ViewShortDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetCustom(CancellationToken cancellationToken, int type, int dateType, int count)
        {
            if (count > 30)
                throw new DataException("تعداد درخواست زیاد است");

            var today = DateTimeOffset.Now;
            var week = today.AddDays(-7);

            switch (dateType)
            {
                case 1:
                    switch (type)
                    {
                        case 1:
                            var list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;


                        case 2:
                            var result = await _repositoryView.TableNoTracking
                               .GroupBy(a => a.PostId)
                               .Select(g => new { g.Key, Count = g.Count() })
                               .OrderByDescending(a => a.Count)
                               .Take(count)
                               .ToListAsync(cancellationToken);

                            var ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        case 4:
                            result = await _repositoryComment.TableNoTracking
                                .GroupBy(a => a.PostId)
                                .Select(g => new { g.Key, Count = g.Count() })
                                .OrderByDescending(a => a.Count)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        default:
                            throw new DataException("نوع مطلب درخواستی نامعتبر است");
                    }

                case 2:
                    switch (type)
                    {
                        case 1:
                            var list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => a.Time.Year == today.Year && a.Time.Month == today.Month)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        case 2:
                            var result = await _repositoryView.TableNoTracking
                                .GroupBy(a => a.PostId)
                                .Select(g => new { g.Key, Count = g.Count() })
                                .OrderByDescending(a => a.Count)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            var ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .Where(a => a.Time.Year == today.Year && a.Time.Month == today.Month)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        case 4:
                            result = await _repositoryComment.TableNoTracking
                                .GroupBy(a => a.PostId)
                                .Select(g => new { g.Key, Count = g.Count() })
                                .OrderByDescending(a => a.Count)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .Where(a => a.Time.Year == today.Year && a.Time.Month == today.Month)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        default:
                            throw new DataException("نوع مطلب درخواستی نامعتبر است");
                    }

                case 3:
                    switch (type)
                    {
                        case 1:
                            var list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => a.Time >= week)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        case 3:
                            var result = await _repositoryView.TableNoTracking
                               .GroupBy(a => a.PostId)
                               .Select(g => new { g.Key, Count = g.Count() })
                               .OrderByDescending(a => a.Count)
                               .Take(count)
                               .ToListAsync(cancellationToken);

                            var ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .Where(a => a.Time >= week)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        case 4:
                            result = await _repositoryComment.TableNoTracking
                                .GroupBy(a => a.PostId)
                                .Select(g => new { g.Key, Count = g.Count() })
                                .OrderByDescending(a => a.Count)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            ids = result.Select(item => item.Key).ToList();

                            list = await TableNoTracking
                                .Where(a => !a.VersionStatus.Equals(2))
                                .Where(a => ids.Contains(a.Id))
                                .Where(a => a.Time >= week)
                                .OrderByDescending(a => a.Time)
                                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                                .Take(count)
                                .ToListAsync(cancellationToken);

                            return list;

                        default:
                            throw new DataException("نوع مطلب درخواستی نامعتبر است");
                    }

                default:
                    throw new DataException("خطا در اطلاعات ورودی");
            }
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> Search(CancellationToken cancellationToken, string str)
        {
            Assert.NotNullArgument(str, "کلمه مورد جستجو نامعتبر است");

            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) &&
                            EF.Functions.Contains(a.Title, str) && a.IsConfirm)
                .OrderByDescending(a => a.Time)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                .Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortStatusSelectDto>>> GetByStatus(CancellationToken cancellationToken, bool status)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.IsConfirm.Equals(status))
                .OrderByDescending(a => a.Time)
                .Include(a => a.Images)
                .ProjectTo<PostShortStatusSelectDto>(Mapper.ConfigurationProvider)
                .Take(DefaultTake)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetUserPosts(CancellationToken cancellationToken, int id)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.UserId.Equals(id))
                .OrderByDescending(a => a.Time)
                .Include(a => a.Images)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<ApiResult<List<PostShortSelectDto>>> GetShort(CancellationToken cancellationToken)
        {
            var list = await TableNoTracking
                .Where(a => !a.VersionStatus.Equals(2) && a.IsConfirm)
                .OrderByDescending(a => a.Time)
                .Include(a => a.Images)
                .ProjectTo<PostShortSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<bool> ChangeStatus(CancellationToken cancellationToken, int id)
        {
            var item = await TableNoTracking
                .SingleAsync(a => !a.VersionStatus.Equals(2) && a.Id.Equals(id), cancellationToken);

            if (item.IsConfirm)
                return true;

            item.IsConfirm = true;

            await UpdateAsync(item, cancellationToken);

            return true;
        }

        public async Task<int> AddImage(List<string> images, int postId, CancellationToken cancellationToken)
        {
            Assert.NotNullArgument(images, "عکس ها خالی است");

            foreach (var img in images)
            {
                await _repositoryImage.AddAsync(new PostImage
                {
                    Image = img,
                    PostId = postId,
                    Version = 1,
                    VersionStatus = 0
                }, cancellationToken, false);
            }

            var result = await DbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
