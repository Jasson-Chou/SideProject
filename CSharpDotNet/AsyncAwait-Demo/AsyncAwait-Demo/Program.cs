using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwait_Demo
{
    class Program
    {
        //1. 平行資料處裡
        //2. 回傳值
        static void Main(string[] args)
        {
            DataHandlerDemo().Wait();
            returnValueHandlerDemo().Wait();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadKey();
        }

        static async Task returnValueHandlerDemo()
        {
            var value = await Task.Run(() => returnValueFunc_demo());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"returnValueHandlerDemo - return value = [{value}]");
        }

        static ulong returnValueFunc_demo()
        {
            ulong i = int.MaxValue;
            ulong result = 0;
            do
            {
                result += i;
                --i;
            } while (i != 0);
            return result;
        }

        static async Task DataHandlerDemo()
        {
            Action<int> func = Func_demo2;
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Serial Data handler Start----------------");
            stopwatch.Restart();
            func(1);
            func(2);
            stopwatch.Stop();
            Console.WriteLine($"Serial Data handler Spend[{stopwatch.ElapsedMilliseconds} ms]");
            Console.WriteLine("Serial Data handler End------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Parallel Data handler Start--------------");
            stopwatch.Restart();
            Task func1 = Task.Run(() => func(1));
            func(2);
            await func1;
            stopwatch.Stop();
            Console.WriteLine($"Parallel Data handler Spend[{stopwatch.ElapsedMilliseconds} ms]");
            Console.WriteLine("Parallel Data handler End----------------");

        }

        static void Func_demo1(int funcNum)
        {
            ulong i = int.MaxValue;
            ulong result = 0;
            do
            {
                result += i;
                --i;
            } while (i != 0);
            Console.WriteLine($"Func[{funcNum}] Done");
        }

        static void Func_demo2(int funcNum)
        {
            int i = ushort.MaxValue;
            int result = 0;
            do
            {
                result += i;
                --i;
            } while (i != 0);
            Console.WriteLine($"Func[{funcNum}] Done");
        }
    }

    

}
