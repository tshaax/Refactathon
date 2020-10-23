using System;

namespace Refactoring.Conway.Domain.Exceptions
{
    [Serializable]
    public class InvalidCellException : Exception
    {
        public InvalidCellException(string message) : base(message) { }
        public InvalidCellException(string message, Exception inner) : base(message, inner) { }
    }
}
