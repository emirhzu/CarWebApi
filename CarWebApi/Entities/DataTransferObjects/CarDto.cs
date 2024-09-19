using System.ComponentModel.Design.Serialization;

namespace Entities.DataTransferObjects
{
    public record CarDto()
    {
        public int id { get; init; }
        public string Brand { get; init; }
        public string Model { get; init; }
        public int Years { get; init; }
        public decimal Price { get; init; }
    }
}
