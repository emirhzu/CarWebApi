using Entities.Exceptions;

public sealed class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(int id) : base($"There is no car with id: {id}")
    {
    }
}