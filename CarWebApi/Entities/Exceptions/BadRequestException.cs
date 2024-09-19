namespace Entities.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message): base(message)
        {

        }
    }
    public class YearsOutofRangeBadRequestException : BadRequestException
    {
        public YearsOutofRangeBadRequestException() : base("Maximum years should be less then 2025 and greater than 1950")
        {
        }
    }
    public class PriceOutofRangeBadRequestException : BadRequestException
    {
        public PriceOutofRangeBadRequestException() : base("Maximum price should be less then 1000000")
        {
        }
    }
}
