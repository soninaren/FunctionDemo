using Common;
using Common.Logging.Abstractions;
using Microsoft.Azure.WebJobs;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "")]string myQueueItem, Microsoft.Extensions.Logging.ILogger logger)
        {
            Common.Logging.Abstractions.ILogger abstractLogger = new MicrosoftLoggingAdapter(logger);
            abstractLogger.Log($"C# Queue trigger function processed: {myQueueItem}");

            Common.Class1 class1 = new Class1(abstractLogger);            
        }
    }
}
