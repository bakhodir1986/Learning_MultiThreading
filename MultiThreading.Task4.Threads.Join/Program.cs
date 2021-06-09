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
    public enum TaskOptions
    {
        OptionA,
        OptionB
    }
    class Program
    {
        private static readonly Semaphore Semaphore = new Semaphore(0, 1);
        private  static  int _threadsCount = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            while (true)
            {
                Console.WriteLine("Enter option character: ");
                var option = Console.ReadLine();

                if (string.IsNullOrEmpty(option))
                {
                    Console.WriteLine("Enter valid option");
                    continue;
                }

                switch (option)
                {
                    case "a":
                        TaskImplimentationA(_threadsCount);
                        break;
                    case "b":
                        TaskImplimentationB(_threadsCount);
                        break;
                }

                Console.WriteLine("main thread done");

                Console.WriteLine("Do you want to stop? enter y, to continue enter n: ");
                var exit = Console.ReadLine();

                if (!string.IsNullOrEmpty(exit) && exit.Equals("y"))
                {
                    Console.WriteLine("Program closed");
                    break;
                }
            }

            Console.ReadLine();
        }

        private static void TaskImplimentationA(int threadState)
        {
            if (threadState > 0)
            {
                Thread thread = new Thread(DoSomeWorkA) {Name = "Thread" + threadState, IsBackground = true};
                thread.Start(threadState);
                thread.Join();
            }
        }

        private static void DoSomeWorkA(object threadState)
        {
            DoSomeWork(threadState, TaskOptions.OptionA);
        }


        private static void DoSomeWork(object threadState, TaskOptions option)
        {
            int threadStateNum = (int)threadState;

            Thread.Sleep(200);

            threadStateNum--;

            if (option == TaskOptions.OptionA)
            {
                Console.WriteLine("DoSomeWorkA - thread.Name: " + Thread.CurrentThread.Name + "- threadStateNum is :" + threadStateNum);

                TaskImplimentationA(threadStateNum);
            }
            else
            {
                Console.WriteLine("DoSomeWorkB - thread.ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId +
                                  "- threadStateNum is :" + threadStateNum);

                TaskImplimentationB(threadStateNum);
            }
        }



        private static void TaskImplimentationB(int threadState)
        {

            if (threadState > 0)
            {
                ThreadPool.QueueUserWorkItem(DoSomeWorkB, threadState);
            }

            if (_threadsCount == threadState)
            {
                //Main Thread in waiting state
                Semaphore.WaitOne();
            }

            if (threadState == 0)
            {
                //Worker thread will release
                Semaphore.Release();
            }
        }

        private static void DoSomeWorkB(object threadState)
        {
             DoSomeWork(threadState, TaskOptions.OptionB);
        }
    }
}
