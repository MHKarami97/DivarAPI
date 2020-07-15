using System;
using System.Collections.Generic;
using AutoMapper;
using Models.Base;
using Entities.Post;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CommentDto : BaseDto<CommentDto, Comment>
    {
        [JsonIgnore]
        public override int Id { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public int PostId { get; set; }

        [JsonIgnore]
        public int CreatorId { get; set; }

        [JsonIgnore]
        public int Witch { get; set; }

        [JsonIgnore]
        public DateTimeOffset Time { get; set; }
    }

    public class CommentSelectDto : BaseDto<CommentSelectDto, Comment>
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string Time { get; set; }

        public override void CustomMappings(IMappingExpression<Comment, CommentSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("g")));
        }
    }

    public class CommentPostSelectDto : BaseDto<CommentPostSelectDto, Comment>
    {
        public string Text { get; set; }
        public string Time { get; set; }
        public int CreatorId { get; set; }
        public int Witch { get; set; }

        public override void CustomMappings(IMappingExpression<Comment, CommentPostSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.Time,
                config => config.MapFrom(src => src.Time.ToString("g")));
        }
    }

    public class CommentPostShortSelectDto
    {
        public int UserId { get; set; }
        public List<CommentPostSelectDto> Comment { get; set; }
    }

    public class CommentShortSelectDto : BaseDto<CommentShortSelectDto, Comment>
    {
        public int PostId { get; set; }
        public int CreatorId { get; set; }
        public int PostUserId { get; set; }
        public string PostTitle { get; set; }
    }
}