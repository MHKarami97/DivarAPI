using Entities.Post;
using Entities.State;
using Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.Models
{
    public class StateDto : BaseDto<StateDto, State>
    {
        public string Name { get; set; }
        public int? ParentStateId { get; set; }

        public string ParentStateName { get; set; }
    }

    public class ShortStateDto : BaseDto<ShortStateDto, State>
    {
        public string Name { get; set; }
    }

    public class StateCreateDto : BaseDto<StateCreateDto, State>
    {
        [JsonIgnore]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public int? ParentStateId { get; set; }
    }

    public class StateWithSubCatDto : BaseDto<StateWithSubCatDto, State>
    {
        public string Name { get; set; }
        public List<ShortStateDto> ChildStates { get; set; }
    }
}
