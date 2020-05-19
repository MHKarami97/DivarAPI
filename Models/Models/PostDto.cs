using System;
using AutoMapper;
using Models.Base;
using Entities.Post;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(500)]
        [DataType(DataType.Text)]
        public string ShortDescription { get; set; }

        [Required]
        public int TimeToRead { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StateId { get; set; }

        [JsonIgnore]
        public DateTimeOffset Time { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        public long Price { get; set; }

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
        public string Image { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostShortSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("d")));
        }
    }

    public class PostSelectDto : BaseDto<PostSelectDto, Post>
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string StateName { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public List<PostImage> Images { get; set; }
        public long Price { get; set; }

        [IgnoreMap]
        public bool IsFavorite { get; set; }

        [IgnoreMap]
        public List<TagDto> Tags { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("d")));
        }
    }
}