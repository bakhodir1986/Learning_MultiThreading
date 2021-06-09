/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine("Enter option character: ");
            var option = Console.ReadLine();


            if (option != null && (option.Equals("a") || option.Equals("A")))
            {
                //Option A
                TaskImplimentationA(10);

            }
            else if (option != null && (option.Equals("b") || option.Equals("B")))
            {
                //Option B
                TaskImplimentationB(10);
            }
            else
            {
                Console.WriteLine("Enter valid option");
            }


            Console.ReadLine();
        }

        private static void TaskImplimentationA(int threadState)
        {
            if (threadState > 0)
            {
                Thread thread = new Thread(DoSomeWorkA) {Name = "Thread" + threadState};
                thread.IsBackground = true;
                thread.Start(threadState);
                thread.Join();
            }
        }

        private static void DoSomeWorkA(object threadState)
        {
            int threadStateNum = (int)threadState;

            Thread.Sleep(200);

            threadStateNum--;

            Console.WriteLine("DoSomeWorkA - thread.Name: " + Thread.CurrentThread.Name + "- threadStateNum is :" + threadStateNum);

            TaskImplimentationA(threadStateNum);
        }

        private static Semaphore semaphore = new Semaphore(2, 10);

        private static void TaskImplimentationB(int threadState)
        {
            if (threadState > 0)
            {
                ThreadPool.QueueUserWorkItem(DoSomeWorkB, threadState);
            }
        }

        private static void DoSomeWorkB(object threadState)
        {
            try
            {
                semaphore.WaitOne();

                Thread.Sleep(200);

                int threadStateNum = (int) threadState;

                threadStateNum--;

                Console.WriteLine("DoSomeWorkB - thread.ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId +
                                  "- threadStateNum is :" + threadStateNum);

                TaskImplimentationB(threadStateNum);
            }
            finally
            {
                semaphore.Release();
            }

        }


    }
}
