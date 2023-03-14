using Polly;
using Polly.Retry;

namespace ConsoleApp1;

public class FlexjetRetryPolicies
{
    public static readonly AsyncRetryPolicy FlexjetDefaultPolicy  = Policy
        .Handle<Exception>(CheckForRetryExclusions())
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, context) => {
                Console.WriteLine($"Something bad happened, but we're trying again in {timeSpan}");
            });

    private static Func<Exception, bool> CheckForRetryExclusions()
    {
        return exception => exception is not DoNotRetryException;
    }
}