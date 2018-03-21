using System;

namespace Common
{
    public class Class1
    {
        public Class1(Common.Logging.Abstractions.ILogger logger)
        {
            logger.Log("started");
        }
    }
}
