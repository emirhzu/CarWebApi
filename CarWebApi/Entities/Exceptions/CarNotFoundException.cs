namespace Entities.Exceptions
{
    public sealed class CarNotFoundException : NotFoundException
    {
        public CarNotFoundException(int id) : base($"There is no car with id: {id}")
        {
        }
    }

}
