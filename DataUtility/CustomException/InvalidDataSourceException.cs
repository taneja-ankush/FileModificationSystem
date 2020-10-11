using System;

namespace DataUtility.CustomException
{
    public class InvalidDataSourceException : Exception
    {
        public InvalidDataSourceException(string message) : base(message)
        {

        }
    }
}
