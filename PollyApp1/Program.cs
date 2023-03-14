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
- A declarative, functional, 'Result' based approach
- Similar to the ServiceResult. Operations either 'Succeed' or they 'Fail', but they always have Data. 
	- You probably don't care about data from a 'Failed' operation
- If there's a failure it's very explicit and the caller and can respond appropriately

Bonus:
- You can add retries to this approach very easily

https://github.com/App-vNext/Polly
 */

var fooService = new FooService();


Console.Clear();

Console.WriteLine("HappyPath Start");
var fooServiceResult = await fooService.HappyPath();

Console.WriteLine("HappyPath ServiceResult Outcome: " + fooServiceResult.Outcome);

if (fooServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("HappyPath ServiceResult FinalException: " + fooServiceResult.FinalException);
	// return;
}
Console.WriteLine("HappyPath ServiceResult Result: " + fooServiceResult.Result);
Console.WriteLine("Press Any key to continue");
Console.ReadKey();



Console.WriteLine("SadPath Start");
fooServiceResult = await fooService.SadPath();
    
Console.WriteLine("SadPath ServiceResult Outcome: " + fooServiceResult.Outcome);

if (fooServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("SadPath ServiceResult FinalException: " + fooServiceResult.FinalException);
	// return;
}
Console.WriteLine("SadPath ServiceResult Result: " + fooServiceResult.Result);
Console.WriteLine("Press Any key to continue");
Console.ReadKey();

Console.WriteLine("ChaosPath Start");
fooServiceResult = await fooService.ChaosPath();
    
Console.WriteLine("ChaosPath ServiceResult Outcome: " + fooServiceResult.Outcome);

if (fooServiceResult.Outcome == OutcomeType.Failure)
{
	Console.WriteLine("SadPath ServiceResult FinalException: " + fooServiceResult.FinalException);
	// return;
}
Console.WriteLine("ChaosPath ServiceResult Result: " + fooServiceResult.Result);



