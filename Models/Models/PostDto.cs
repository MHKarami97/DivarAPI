using System;
using AutoMapper;
using Models.Base;
using Entities.Post;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Models.Models
{
    public class PostDto : BaseDto<PostDto, Post>
    {
        [JsonIgnore]
        public override int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public string Text { get; set; }

        // [AllowNull]
        // public List<IFormFile> Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StateId { get; set; }

        [JsonIgnore]
        public DateTimeOffset Time { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public long? Price { get; set; }

        public int Type { get; set; }

        [JsonIgnore]
        public bool IsConfirm { get; set; }

        [Required]
        public string Phone { get; set; }

        [DataType(DataType.Text)]
        public string Location { get; set; }
    }

    public class PostShortSelectDto : BaseDto<PostShortSelectDto, Post>
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string StateName { get; set; }
        public string Time { get; set; }
        public List<PostImageDto> Images { get; set; }
        public long Price { get; set; }
        public int Type { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostShortSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("d")));
        }
    }

    public class PostShortStatusSelectDto : BaseDto<PostShortStatusSelectDto, Post>
    {
        public string Title { get; set; }
        public bool IsConfirm { get; set; }
        public string CategoryName { get; set; }
        public string StateName { get; set; }
    }

    public class PostSelectDto : BaseDto<PostSelectDto, Post>
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string StateName { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public long Price { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public List<PostImageDto> Images { get; set; }

        [IgnoreMap]
        public bool IsFavorite { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("d")));
        }
    }
}