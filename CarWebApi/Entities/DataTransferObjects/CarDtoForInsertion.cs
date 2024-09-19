using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record CarDtoForInsertion : CarDtoForManipulation
    {
        [Required(ErrorMessage = "CategoryId is required!")]
        public int CategoryId { get; init; }
    }
}
