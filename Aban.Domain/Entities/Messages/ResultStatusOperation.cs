using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Domain.Entities
{
    public class ResultStatusOperation
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ResultStatusOperation()
        {

        }

        public ResultStatusOperation(string title, string message, MessageTypeResult type, bool isSuccessed, Exception errorException)
        {
            Title = title;
            Message = message;
            Type = type;
            IsSuccessed = isSuccessed;
            ErrorException = errorException;
        }
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageTypeResult Type { get; set; }
        public bool IsSuccessed { get; set; }
        public Exception ErrorException { get; set; }
    }

}
