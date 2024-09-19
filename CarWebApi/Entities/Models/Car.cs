using System.Runtime.Serialization;

namespace Entities.Models
{
    [DataContract(IsReference = true)]
    public class Car
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public int Years { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public Category Category { get; set; }
    }
}
