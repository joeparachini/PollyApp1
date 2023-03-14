using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;

namespace ConsoleApp1;

public class FooService
{
    public async Task<PolicyResult<FooData>> HappyPath()
    {
        // Define the Polly Context
        var myContext = new Context("HappyPath")
        {
            ["MessageGuid"] = Guid.NewGuid(),
            ["EntityId"] = Guid.NewGuid(),
        };

        // Define the policy
        var myPolicy = FlexjetRetryPolicies.FlexjetDefaultPolicy;
        
        // Execute And Capture the method
        var result = await myPolicy.ExecuteAndCaptureAsync(GetData);

        // Return the result
        return result;
    }

    public async Task<PolicyResult<FooData>> SadPath()
    {
        var myPolicy = FlexjetRetryPolicies.FlexjetDefaultPolicy;
        var result = await myPolicy.ExecuteAndCaptureAsync(ThrowException);
        
        return result;
    }
    
    public async Task<PolicyResult<FooData>> ChaosPath()
    {
        var myPolicy = FlexjetRetryPolicies.FlexjetDefaultPolicy;

        var chaosPolicy = MonkeyPolicy.InjectResultAsync<FooData>(with =>
        {
            with.InjectionRate(1)
                .Enabled(true);

            with.Fault(new Exception("THIS IS FINE"));
        });
        
        var newPolicy = myPolicy.WrapAsync(chaosPolicy);
        
        var result = await newPolicy.ExecuteAndCaptureAsync(GetData);

        return result;
    }

    private static async Task<FooData> GetData()
    {
        await Task.FromResult(true);
        
        return new FooData()
        {
            Name = "Joe P"
        };
    }
    
    private static async Task<FooData> ThrowException()
    {
        await Task.FromResult(true);

        throw new Exception("SOMETHING BAD HAPPENED");
        
        return new FooData()
        {
            Name = "Joe P"
        };
    }
}