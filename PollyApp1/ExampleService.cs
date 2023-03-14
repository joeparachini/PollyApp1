using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;

namespace ConsoleApp1;

public class ExampleService
{
    public async Task<PolicyResult<ExampleData>> HappyPath()
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

    public async Task<PolicyResult<ExampleData>> SadPath()
    {
        var myPolicy = FlexjetRetryPolicies.FlexjetDefaultPolicy;
        var result = await myPolicy.ExecuteAndCaptureAsync(ThrowException);
        
        return result;
    }
    
    public async Task<PolicyResult<ExampleData>> ChaosPath()
    {
        var myPolicy = FlexjetRetryPolicies.FlexjetDefaultPolicy;

        var chaosPolicy = MonkeyPolicy.InjectResultAsync<ExampleData>(with =>
        {
            with.InjectionRate(1)
                .Enabled(true);

            with.Fault(new Exception("THIS IS FINE"));
        });
        
        var newPolicy = myPolicy.WrapAsync(chaosPolicy);
        
        var result = await newPolicy.ExecuteAndCaptureAsync(GetData);

        return result;
    }

    private static async Task<ExampleData> GetData()
    {
        await Task.FromResult(true);
        
        return new ExampleData()
        {
            Name = "Joe P"
        };
    }
    
    private static async Task<ExampleData> ThrowException()
    {
        await Task.FromResult(true);

        throw new Exception("SOMETHING BAD HAPPENED");
        
        return new ExampleData()
        {
            Name = "Joe P"
        };
    }
}