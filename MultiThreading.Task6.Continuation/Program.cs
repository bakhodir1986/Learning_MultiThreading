/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            Console.WriteLine("Enter option character: ");
            var option = Console.ReadLine();
  

            Task parentTask = Task.Run(() => {

                Console.WriteLine("Task 1");

                if (option != null && (option.Equals("a") || option.Equals("A")))
                {
                    //
                }
                else if (option != null && (option.Equals("b") || option.Equals("B")))
                {
                    throw new ApplicationException("NotOnRanToCompletion");
                }
                else if (option != null && (option.Equals("c") || option.Equals("C")))
                {
                    throw new ApplicationException("OnlyOnFaulted");
                }
                else if (option != null && (option.Equals("d") || option.Equals("D")))
                {
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                }

            }, tokenSource2.Token);

            if (option != null && (option.Equals("d") || option.Equals("D")))
            {
                tokenSource2.Cancel();
            }

            //Continuation task should be executed regardless of the result of the parent task.
            parentTask.ContinueWith(antecedent => {

                Console.WriteLine("Task None");

            } , TaskContinuationOptions.None);

            //Continuation task should be executed when the parent task finished without success.
            parentTask.ContinueWith(antecedent => {

                Console.WriteLine("Task NotOnRanToCompletion");

            }, TaskContinuationOptions.NotOnRanToCompletion);

            //Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.
            parentTask.ContinueWith(antecedent => {

                Console.WriteLine("Task OnlyOnFaulted");

            }, TaskContinuationOptions.OnlyOnFaulted);

            //Continuation task should be executed outside of the thread pool when the parent task would be cancelled.
            parentTask.ContinueWith(antecedent => {

                Console.WriteLine("Task OnlyOnCanceled");

            }, TaskContinuationOptions.OnlyOnCanceled);

            Console.ReadLine();
        }
    }
}
