using System.Globalization;

namespace DumpTruckManagementSystem.Shared.Exceptions
{
    public class ForbiddenAccessException : Exception
    {

        public ForbiddenAccessException() : base() { }

        public ForbiddenAccessException(string message) : base(message) { }

        public ForbiddenAccessException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

    //public class CustomValidationException : Exception
    //{
    //    public CustomValidationException()
    //        : base("One or more validation failures have occurred.")
    //    {
    //        Failures = new Dictionary<string, string[]>();
    //    }

    //    public CustomValidationException(List<ValidationFailure> failures): this()
    //    {
    //        var propertyNames = failures
    //            .Select(e => e.PropertyName)
    //            .Distinct();

    //        foreach (var propertyName in propertyNames)
    //        {
    //            var propertyFailures = failures
    //                .Where(e => e.PropertyName == propertyName)
    //                .Select(e => e.ErrorMessage)
    //                .ToArray();

    //            Failures.Add(propertyName, propertyFailures);
    //        }
    //    }

    //    public IDictionary<string, string[]> Failures { get; }
    //}
}
