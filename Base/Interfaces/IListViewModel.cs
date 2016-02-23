using System;
using System.Threading.Tasks;
namespace OpenUWP.Base.Interfaces
{
    public interface IListViewModel
    {
        bool IsLoaded { get; set; }
        int Page { get; set; }
        int Size { get; set; }

        Task Load(Action completed = null);

        Task LoadMore(Action completed = null);

        Task Refresh(Action completed = null);
    }
    public interface IListViewModel<T>
    {
        ObservableList<T> Data { set; get; }
    }
}
