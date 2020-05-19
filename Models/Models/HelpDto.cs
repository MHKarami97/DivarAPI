using Models.Base;
using Entities.More;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class HelpDto : BaseDto<HelpDto, Help>
    {
        [JsonIgnore]
        public override int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Image { get; set; }

        public int? ParentHelpId { get; set; }
    }

    public class HelpSelectDto : BaseDto<HelpSelectDto, Help>
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ParentHelpName { get; set; }
        public int ParentHelpId { get; set; }
    }
}
