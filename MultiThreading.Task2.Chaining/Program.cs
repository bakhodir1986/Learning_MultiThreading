/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void PrintArray(int[] array, string taskNumber)
        {
            Console.Write(taskNumber + ": ");

            foreach (var element in array)
            {
                Console.Write(element + "\t");
            }

            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            Task<int[]> task1 = Task.Run(() => {

                int Min = 0;
                int Max = 10;
                Random randNum = new Random();
                int[] arrayOfInt = Enumerable
                    .Repeat(Min, Max)
                    .Select(i => randNum.Next(Min, Max))
                    .ToArray();

                PrintArray(arrayOfInt, "Task 1");

                return arrayOfInt;

            });


            Task<int[]> task2 = task1.ContinueWith(antecedent => {

                var randomArray = antecedent.Result;
                Random randNum = new Random();
                var randomInteger = randNum.Next(10);

                Console.WriteLine("Random Number: " + randomInteger);

                for (int i = 0; i < randomArray.Length; i++)
                {
                    randomArray[i] = randomArray[i] * randomInteger;
                }

                PrintArray(randomArray, "Task 2");

                return randomArray;
            });

            Task<int[]> task3 = task2.ContinueWith(antecedent => {

                var randomArray = antecedent.Result;

                Array.Sort(antecedent.Result);

                PrintArray(randomArray, "Task 3");

                return randomArray;
            });

            Task<double> task4 = task3.ContinueWith(antecedent => {

                var randomArray = antecedent.Result;

                var result = randomArray.Average();

                Console.Write("Task 4 Avarage:  ");

                Console.WriteLine(result);

                return result;

            });

            Console.ReadLine();
        }
    }
}
