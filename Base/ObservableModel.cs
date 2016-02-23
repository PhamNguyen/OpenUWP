using System.ComponentModel;
using Windows.UI.Core;

namespace OpenUWP.Base
{
    public class ObservableModel : BaseListViewItem, INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                });
            }
        }

        public void NotifyPropertyChanging(string name)
        {
            if (PropertyChanging != null)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   PropertyChanging(this, new PropertyChangingEventArgs(name));
               });
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

    }
}
