﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Models.Base;
using Models.More;
using Services.Security;
using System.Collections.Generic;
using System.IO;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class FilesController : BaseController
    {
        private readonly ISecurity _security;
        private readonly IHostEnvironment _environment;

        public FilesController(ISecurity security, IHostEnvironment environment)
        {
            _security = security;
            _environment = environment;
        }

        [HttpPost]
        [RequestSizeLimit(900_000)]
        public ApiResult<List<string>> Create(List<IFormFile> files)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                switch (_security.ImageCheck(file))
                {
                    case 0:
                        break;

                    case 1:
                        return BadRequest("فایل نامعتبر است");

                    case 2:
                        return BadRequest("فرمت فایل نامعتبر است");

                    case 3:
                        return BadRequest("حداکثر حجم فایل نامعتبر است");
                }

                var uploads = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");

                var address = _security.GetUniqueFileName(file.FileName);

                var fullPath = Path.Combine(uploads, address);

                file.CopyTo(new FileStream(fullPath, FileMode.Create));

                result.Add(address);
            }

            return Ok(result);
        }

        [HttpPost]
        [RequestSizeLimit(900_000)]
        public UploadResultSingle SingleCreate(IFormFile file)
        {
            switch (_security.ImageCheck(file))
            {
                case 0:
                    break;

                case 1:
                    return new UploadResultSingle
                    {
                        Images = null,
                        Message = "فایل نامعتبر است",
                        Status = false
                    };

                case 2:
                    return new UploadResultSingle
                    {
                        Images = null,
                        Message = "فرمت فایل نامعتبر است",
                        Status = false
                    };

                case 3:
                    return new UploadResultSingle
                    {
                        Images = null,
                        Message = "حداکثر حجم فایل نامعتبر است",
                        Status = false
                    };
            }

            var uploads = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");

            var address = _security.GetUniqueFileName(file.FileName);

            var fullPath = Path.Combine(uploads, address);

            file.CopyTo(new FileStream(fullPath, FileMode.Create));


            return new UploadResultSingle
            {
                Images = address,
                Message = "ok",
                Status = true
            };
        }

        [HttpGet]
        [Authorize]
        [RequestSizeLimit(900_000_000)]
        public UploadResult Upload(IFormFileCollection files)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                switch (_security.ImageCheck(file))
                {
                    case 0:
                        break;

                    case 1:
                        return new UploadResult
                        {
                            Images = null,
                            Message = "فایل نامعتبر است",
                            Status = false
                        };

                    case 2:
                        return new UploadResult
                        {
                            Images = null,
                            Message = "فرمت فایل نامعتبر است",
                            Status = false
                        };

                    case 3:
                        return new UploadResult
                        {
                            Images = null,
                            Message = "حداکثر حجم فایل نامعتبر است",
                            Status = false
                        };
                }

                var uploads = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");

                var address = _security.GetUniqueFileName(file.FileName);

                var fullPath = Path.Combine(uploads, address);

                file.CopyTo(new FileStream(fullPath, FileMode.Create));

                result.Add(address);
            }

            return new UploadResult
            {
                Images = result,
                Message = "ok",
                Status = true
            };
        }
    }
}