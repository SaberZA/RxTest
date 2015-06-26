using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxTest.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            StartBackgroundWork();
            Console.ReadKey();
        }

        private static async void StartBackgroundWork()
        {
            Console.WriteLine("Shows use of Start to start on a background thread:");
            var o = Observable.Start(() =>
            {
                //This starts on a background thread.
                Console.WriteLine("From background thread. Does not block main thread.");
                Console.WriteLine("Calculating...");
                Thread.Sleep(1000);
                Console.WriteLine("Background work completed.");
            });
            await o.FirstAsync(); //subsribe and wait for completion of background operation. If you remove await, the main thread will complete first.
            Console.WriteLine("Main thread completed");
        }
    }
}
