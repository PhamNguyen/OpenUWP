using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace OpenUWP.Utils
{
    public class ActionHelper
    {
        private static Semaphore delaySemaphore = new Semaphore(1, 1);
        private static bool _isBusy = false;
        public static void Run(Action doWork)
        {
            if (doWork != null)
            {
               
                if (!_isBusy && delaySemaphore.WaitOne(0))
                {
                    _isBusy = true;
                    ThreadPool.RunAsync(async(t) =>
                     {
                         doWork();

                         await Task.Delay(1000);
                         _isBusy = false;
                         delaySemaphore.Release();
                     });
                }
            }
        }
    }
}
