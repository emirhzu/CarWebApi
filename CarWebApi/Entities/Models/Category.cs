using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    [DataContract(IsReference = true)]
    public class Category
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public String? CategoryName { get; set; }
        [JsonIgnore]
        public ICollection<Car> Cars { get; set; }
    }
}
