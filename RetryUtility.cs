using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public static class RetryUtility
    {
        public static TRet RetryIfThrown<TException, TRet>(Func<TRet> action, int numberOfTries, int delayBetweenTries) where TException : Exception
        {
            TException lastException = null;
            int counter = 0;
            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    return action();
                }
                catch (TException e)
                {
                    counter = LogError(numberOfTries, delayBetweenTries, counter, e);
                    lastException = e;
                }

                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;

            throw new Exception("You shouldn't be here: all went silently but last exception is non null!");
        }

        public static void RetryIfThrown<TException>(Action action, int numberOfTries, int delayBetweenTries) where TException : Exception
        {
            TException lastException = null;
            int counter = 0;

            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    action();
                    return;
                }
                catch (TException e)
                {
                    counter = LogError(numberOfTries, delayBetweenTries, counter, e);
                    lastException = e;
                }

                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;
        }

        public static TRet Retry<TRet>(Func<TRet> action, int numberOfTries, int delayBetweenTries)
        {
            Exception lastException = null;
            int counter = 0;

            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    counter = LogError(numberOfTries, delayBetweenTries, counter, e);
                    lastException = e;
                }

                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;

            throw new Exception("You shouldn't be here: all went silently but last exception is non null!");
        }

        public static void Retry(Action action, int numberOfTries, int delayBetweenTries)
        {
            Exception lastException = null;
            int counter = 0;

            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    counter = LogError(numberOfTries, delayBetweenTries, counter, e);
                    lastException = e;
                }

                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;
        }

        private static int LogError<TException>(int numberOfTries, int delayBetweenTries, int counter, TException e) where TException : Exception
        {
            Console.WriteLine($"Error: {e.Message}!{Environment.NewLine}Attempt #{++counter} of {numberOfTries}.", ConsoleColor.Yellow);
            if (counter < numberOfTries) Console.WriteLine($" Will try again in {delayBetweenTries} ms...", ConsoleColor.Yellow);
            return counter;
        }
    }
}
