using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace OpenUWP.Extentions
{
    public static class StoryboardExtentions
    {
        public static Task<bool> StopCompleted(this Storyboard storyboard)
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<object> onCompleted = null;
            onCompleted = (object sender, object e) =>
            {
                storyboard.Completed -= onCompleted;
                tcs.SetResult(true);
            };
            storyboard.Completed += onCompleted;
            return tcs.Task;
        }
    }
}
