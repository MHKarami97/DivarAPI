using System.Collections.Generic;
using Entities.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Common.Utilities;
using Models.Base;

namespace Models.Models
{
    public class FavoriteDto : BaseDto<FavoriteDto, Favorite>
    {
        public string PostTitle { get; set; }
        public string PostCategoryName { get; set; }
        public string PostStateName { get; set; }
        public string PostTime { get; set; }
        public List<PostImageDto> PostImages { get; set; }
        public long PostPrice { get; set; }
        public int PostType { get; set; }
        public int PostId { get; set; }

        public override void CustomMappings(IMappingExpression<Favorite, FavoriteDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.PostTime,
                config => config.MapFrom(src => src.Post.Time.ToBeautyDate()));
        }
    }

    public class FavoriteSelectDto : BaseDto<FavoriteSelectDto, Favorite>
    {
        [JsonIgnore]
        public override int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}