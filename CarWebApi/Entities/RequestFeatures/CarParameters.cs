namespace Entities.RequestFeatures
{
    public class CarParameters : RequestParameters 
    {
        //Brand
        public string Brand { get; set; }

        //Model
        public string Model { get; set; }

        //Years
        public uint MinYears { get; set; }
        public uint MaxYears { get; set; } = 2025;
        public bool ValidYearsRange => MaxYears > MinYears;

        //Price
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 1000000;
        public bool ValidPriceRange => MaxPrice > MinPrice;

        public CarParameters()
        {
            OrderBy = "id";
        }
    }
}
