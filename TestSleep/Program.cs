using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Timers;

namespace TestSleep
{
    internal class Program
    {
        static Stopwatch sw = new Stopwatch();
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern void TimeBeginPeriod(int t);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern void TimeEndPeriod(int t);
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);

        [DllImport("ntdll.dll")]
        private static extern bool NtDelayExecution(bool Alertable, ref long DelayInterval);
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int NtQueryTimerResolution(out int MinimumResolution, out int MaximumResolution, out int CurrentResolution);
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Start measurement:");
                Console.WriteLine("Enter the time interval in milliseconds");
                var intervalMs = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the sleepMilliseconds");
                var sleepMilliseconds = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the count measurement");
                var count = Convert.ToInt32(Console.ReadLine());
                AdjustTimerResolution(intervalMs,sleepMilliseconds,count);
                Console.WriteLine("Stop measurement:");
                Console.WriteLine("Restart?");
                var str = Console.ReadLine().ToLower();
                if (str != "yes")
                {
                    break;
                }
            }
        }

        private static void AdjustTimerResolution(int intervalMs,int sleepMilliseconds,int count)
        {
            uint newInterval = (uint)intervalMs * 10000;

            Console.WriteLine($"\tBefore: {SleepTimeMeasurement(sw,sleepMilliseconds, count)}");

            NtSetTimerResolution(newInterval, true, ref newInterval);

            Console.WriteLine($"\tAfter: {SleepTimeMeasurement(sw, sleepMilliseconds, count)}");
            NtSetTimerResolution(156250, true, ref newInterval);
        }
        private static double SleepTimeMeasurement(Stopwatch stopwatch, int sleepMilliseconds,int count)
        {
            double avg = 0;
            for (int i = 0; i < count; i++)
            {
                sw.Restart();
                Thread.Sleep(sleepMilliseconds);
                sw.Stop();
                avg += sw.ElapsedMilliseconds;
            }
            return avg / count;
        }

    }
    



}
