using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUWP.Base
{
    public enum ItemMode
    {
        Header,
        Item,
        Footer,
        Notify,
        Loading
    }

    public class BaseListViewItem
    {
        private ItemMode _mode = ItemMode.Item;

        public ItemMode Mode
        {
            set
            {
                _mode = value;
            }
            get
            {
                return _mode;
            }
        }
    }
}
