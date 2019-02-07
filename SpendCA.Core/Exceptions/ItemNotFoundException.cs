using System;
namespace SpendCA.Core.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message)
        {
        }

        public ItemNotFoundException() : base($"Item not found")
        {
        }
    }
}
