using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract record CarDtoForManipulation
    {
        [Required(ErrorMessage = "Brand is a required field.")]
        [MinLength(2, ErrorMessage = "Brand must consist of at least 2 chracters")]
        [MaxLength(30, ErrorMessage = "Brand must consist of at maximum 30 chracters")]
        public string Brand { get; init; }
        [Required(ErrorMessage = "Model is a required field.")]
        [MinLength(2, ErrorMessage = "Model must consist of at least 2 chracters")]
        [MaxLength(30, ErrorMessage = "Model must consist of at maximum 30 chracters")]
        public string Model { get; init; }
        [Required(ErrorMessage = "Years is a required field.")]
        public int Years { get; init; }
        [Required(ErrorMessage = "Price is a required field.")]
        public decimal Price { get; init; }
    }
}
