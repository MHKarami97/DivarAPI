using Entities.Post;
using Models.Base;

namespace Models.Models
{
    public class PostImageDto : BaseDto<PostImageDto, PostImage>
    {
        public string Name { get; set; }
    }
}