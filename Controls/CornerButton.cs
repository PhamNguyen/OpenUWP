using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Controls
{
    public class CornerButton : Button
    {
        #region CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CornerButton), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            set { this.SetValue(CornerRadiusProperty, value); }
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
        }
        #endregion

        public CornerButton():base()
        {
            this.DefaultStyleKey = typeof(CornerButton);
        }
    }
}