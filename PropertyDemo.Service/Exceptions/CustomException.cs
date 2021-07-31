using System;

namespace PropertyDemo.Service.Exceptions
{
    public class CustomException : ApplicationException
    {
        public virtual string UserFriendlyMessage { get; }

        public CustomException()
        { }

        public CustomException(string userMessage)
            : base(userMessage)
        {
            UserFriendlyMessage = userMessage;
        }

        public CustomException(string userMessage, Exception innerException)
            : base(userMessage, innerException)
        {
            UserFriendlyMessage = userMessage;
        }

        public CustomException(string userMessage, string message, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = userMessage;
        }
    }
}
