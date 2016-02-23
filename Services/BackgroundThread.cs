using System;
using System.ComponentModel;

namespace OpenUWP.Services
{
    public class BackgroundThread
    {
        public static void Run(Action doWorkAction, Action completedAction = null)
        {
            if (doWorkAction != null)
            {
                var backgroundThread = new BackgroundWorker();

                DoWorkEventHandler doWorkEvent = (sender, e) => doWorkAction();
                RunWorkerCompletedEventHandler runCompletedEvent = (sender, e) =>
                {
                    backgroundThread.DoWork -= doWorkEvent;
                    if (completedAction != null)
                        completedAction();
                    backgroundThread = null;
                };
                backgroundThread.RunWorkerCompleted += runCompletedEvent;
                backgroundThread.DoWork += doWorkEvent;
                backgroundThread.RunWorkerAsync();
            }
        }
    }
}
