namespace OnionApiTemplate.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public BadRequestException(IEnumerable<string> errors)
        : base(errors?.FirstOrDefault() ?? "Bad Request") => Errors = errors;
    }

}
