using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var qtd = 15;
            var sTask = StartTasks(qtd);
            var sThread = StartThreads(qtd);

            await sThread;
            Console.WriteLine("Type any key to quit...");
            Console.ReadKey();
        }

        public static async Task StartTasks(int qtd)
        {
            Console.WriteLine($"Initialize Taskies creating {qtd} taskies...");

            var list = new List<Task>();

            for (int i = 1; i <= qtd; i++)
            {
                var processNumber = i;
                var task = Task.Run(() => WaitMilliseconds("Task", processNumber));
                list.Add(task);
            }

            var taskAsync = Task.WhenAll(list.ToArray());

            try
            {
                await taskAsync;
            }
            catch { }

            if (taskAsync.Status == TaskStatus.RanToCompletion)
                Console.WriteLine("Process taskies executed.");
            else if (taskAsync.Status == TaskStatus.Faulted)
                Console.WriteLine("Process taskies executed with erros.");
        }

        public static async Task StartThreads(int qtd)
        {
            Console.WriteLine($"Initialize Threads creating {qtd} Threads...");

            var list = await Task.Run(() => new List<Thread>());

            for (int i = 1; i <= qtd; i++)
            {
                var processNumber = i;
                var th = new Thread(() => WaitMilliseconds("Thread", processNumber));
                th.Name = $"SID{i}";
                th.Start();
                list.Add(th);
            }

        }

        public static void WaitMilliseconds(string type, int processNumber)
        {
            var milliseconds = new Random().Next(1000, 5000);
            Console.WriteLine($"{type} - process number {processNumber} - waiting {milliseconds / 1000} second(s).");
            Thread.Sleep(milliseconds);
        }
    }
}
