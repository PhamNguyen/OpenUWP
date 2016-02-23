using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUWP.Base.Interfaces
{
    public interface IDetailViewModel
    {
        Task Load(Action completed = null);
    }

    public interface IDetailViewModel<T>
    {
        T SelectedItem { set; get; }
        Task Load(Action completed = null);
    }
}
