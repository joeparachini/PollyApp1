using ConsoleApp1;
using Polly;
/*
 
TLDR
The Problem:
- If we don't know the exception handling behavior of our code, it can be difficult to follow the control flow and properly design resilient logic

- If we return null, 0, or some default value (e.g. empty list), it can be difficult for the call site to distinguish between 
	1 : The operation succeeded and there happened to be no results
	2 : The operation failed, and so there's no results

The Fix:
- A pure, functional, 'Result' based approach
- Similar to the ServiceResult. Operations either 'Succeed' or they 'Fail'. They always have Data.
	- You probably don't care about data from a 'Failed' operation
- Methods never throw or change application state, but can respond to application events (i.e. shutdown)
- When (not if) there's a failure - it's very explicit and the caller and can respond appropriately

Bonus:
- You can add retries to this approach very easily

https://github.com/App-vNext/Polly
 */

var exampleService = new ExampleService();

Console.Clear();

Console.WriteLine("HappyPath Start");
var exampleServiceResult = await exampleService.HappyPath();

Console.WriteLine("HappyPath ServiceResult Outcome: " + exampleServiceResult.Outcome);

if (exampleServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("HappyPath ServiceResult FinalException: " + exampleServiceResult.FinalException);
	// return;
}
Console.WriteLine("HappyPath ServiceResult Result: " + exampleServiceResult.Result);
Console.WriteLine("Press Any key to continue");
Console.ReadKey();



Console.WriteLine("SadPath Start");
exampleServiceResult = await exampleService.SadPath();
    
Console.WriteLine("SadPath ServiceResult Outcome: " + exampleServiceResult.Outcome);

if (exampleServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("SadPath ServiceResult FinalException: " + exampleServiceResult.FinalException);
	// return;
}
Console.WriteLine("SadPath ServiceResult Result: " + exampleServiceResult.Result);
Console.WriteLine("Press Any key to continue");
Console.ReadKey();

Console.WriteLine("ChaosPath Start");
exampleServiceResult = await exampleService.ChaosPath();
    
Console.WriteLine("ChaosPath ServiceResult Outcome: " + exampleServiceResult.Outcome);

if (exampleServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("SadPath ServiceResult FinalException: " + exampleServiceResult.FinalException);
	// return;
}
Console.WriteLine("ChaosPath ServiceResult Result: " + exampleServiceResult.Result);