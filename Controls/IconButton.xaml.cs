using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OpenUWP.Controls
{
    public enum ButtonType
    {
        Normal,
        Toogle
    }
    public sealed partial class IconButton : UserControl
    {
        public static BitmapImage OnePixelBitmap = new BitmapImage(new Uri("ms-appx:///Assets/mobo_logo_full.png", UriKind.RelativeOrAbsolute));

        public IconButton()
        {
            InitializeComponent();
        }

        public int DecodePixelWidth
        {
            get { return (int)GetValue(DecodePixelWidthProperty); }
            set { SetValue(DecodePixelWidthProperty, value); }
        }

        public static readonly DependencyProperty DecodePixelWidthProperty = DependencyProperty.Register("DecodePixelWidth", typeof(int), typeof(IconButton), new PropertyMetadata(200));


        #region Rest Image Source
        public static readonly DependencyProperty RestImageSourceProperty = DependencyProperty.Register("RestIconImageSource", typeof(ImageSource), typeof(IconButton), new PropertyMetadata(OnePixelBitmap, OnRestImageSourceChanged));

        public ImageSource RestImageSource
        {
            set { this.SetValue(RestImageSourceProperty, value); }
            get { return (ImageSource)this.GetValue(RestImageSourceProperty); }
        }
        private static void OnRestImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;

            if (iconButton != null)
            {
                BitmapImage bitmap = iconButton.RestImageSource as BitmapImage;

                if (bitmap != null)
                {
                    bitmap.DecodePixelType = DecodePixelType.Logical;
                    bitmap.DecodePixelWidth = iconButton.DecodePixelWidth;
                    bitmap.CreateOptions = BitmapCreateOptions.None;
                    iconButton.RestImage.Source = bitmap;
                }
            }
        }
        #endregion

        #region Pressed Image Source
        public static readonly DependencyProperty PressedImageSourceProperty = DependencyProperty.Register("PressedImageSource", typeof(ImageSource), typeof(IconButton), new PropertyMetadata(OnePixelBitmap, OnPressedImageSourceChanged));

        public ImageSource PressedImageSource
        {
            set { this.SetValue(PressedImageSourceProperty, value); }
            get { return (ImageSource)this.GetValue(PressedImageSourceProperty); }
        }
        private static void OnPressedImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                BitmapImage bitmap = iconButton.PressedImageSource as BitmapImage;
                if (bitmap != null)
                {
                    bitmap.DecodePixelType = DecodePixelType.Logical;
                    bitmap.DecodePixelWidth = iconButton.DecodePixelWidth;
                    bitmap.CreateOptions = BitmapCreateOptions.None;
                    iconButton.PressedImage.Source = bitmap;
                }
            }
        }

        #endregion

        #region Title
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Title", typeof(string), typeof(IconButton), new PropertyMetadata(String.Empty, OnTextChanged));

        public string Title
        {
            set { this.SetValue(TextProperty, value); }
            get { return (string)this.GetValue(TextProperty); }
        }
        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                if (!string.IsNullOrEmpty(iconButton.Title))
                    iconButton.TitleTextBlock.Text = iconButton.Title;
                else
                    iconButton.TitleTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Title Font Size
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(int), typeof(IconButton), new PropertyMetadata(20, OnTitleFontSizeChanged));

        public int TitleFontSize
        {
            set { this.SetValue(TitleFontSizeProperty, value); }
            get { return (int)this.GetValue(TitleFontSizeProperty); }
        }
        private static void OnTitleFontSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
                iconButton.FontSize = iconButton.TitleFontSize;
        }

        #endregion

        #region Strect

        public static readonly DependencyProperty ContentStrectProperty = DependencyProperty.Register("ContentStrect", typeof(Stretch), typeof(IconButton), new PropertyMetadata(Stretch.Uniform, OnContentStrectChanged));

        public Stretch ContentStrect
        {
            set { this.SetValue(ContentStrectProperty, value); }
            get { return (Stretch)this.GetValue(ContentStrectProperty); }
        }
        private static void OnContentStrectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
                iconButton.PressedImage.Stretch = iconButton.RestImage.Stretch = iconButton.ContentStrect;
        }

        #endregion

        #region Title Foreground in Rest State

        public static readonly DependencyProperty TitleInRestStateForegroundProperty = DependencyProperty.Register("TitleForegroundInRestState", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray), OnTextForegroundChanged));

        public SolidColorBrush TitleForegroundInRestState
        {
            set { this.SetValue(TitleInRestStateForegroundProperty, value); }
            get { return (SolidColorBrush)this.GetValue(TitleInRestStateForegroundProperty); }
        }
        private static void OnTextForegroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null) iconButton.TitleTextBlock.Foreground = iconButton.TitleForegroundInRestState;
        }

        #endregion

        #region Title Foreground in Pressed State
        public static readonly DependencyProperty TitleInPressedStateForegroundProperty = DependencyProperty.Register("TitleForegroundInPressedState", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public SolidColorBrush TitleForegroundInPressedState
        {
            set { this.SetValue(TitleInPressedStateForegroundProperty, value); }
            get { return (SolidColorBrush)this.GetValue(TitleInPressedStateForegroundProperty); }
        }

        #endregion

        #region Background in rest state

        public static readonly DependencyProperty BackgroundInRestStateProperty = DependencyProperty.Register("BackgroundInRestState", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), OnBackgroundChanged));

        public SolidColorBrush BackgroundInRestState
        {
            set { this.SetValue(BackgroundInRestStateProperty, value); }
            get { return (SolidColorBrush)this.GetValue(BackgroundInRestStateProperty); }
        }
        private static void OnBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null) iconButton.Container.Background = iconButton.BackgroundInRestState;
        }

        #endregion

        #region Background in Pressed state

        public static readonly DependencyProperty BackgroundInPressedStateProperty = DependencyProperty.Register("BackgroundInPressedState", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public SolidColorBrush BackgroundInPressedState
        {
            set { this.SetValue(BackgroundInPressedStateProperty, value); }
            get { return (SolidColorBrush)this.GetValue(BackgroundInPressedStateProperty); }
        }

        #endregion

        #region IsChecked
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(IconButton), new PropertyMetadata(false, OnCheckedChanged));

        public bool IsChecked
        {
            set { this.SetValue(IsCheckedProperty, value); }
            get { return (bool)this.GetValue(IsCheckedProperty); }
        }
        private static void OnCheckedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                if (iconButton.IsChecked)
                {
                    iconButton.RestImage.Visibility = Visibility.Collapsed;
                    iconButton.PressedImage.Visibility = Visibility.Visible;
                    iconButton.TitleTextBlock.Foreground = iconButton.TitleForegroundInPressedState;
                    iconButton.Container.Background = iconButton.BackgroundInPressedState;
                }
                else
                {
                    iconButton.RestImage.Visibility = Visibility.Visible;
                    iconButton.PressedImage.Visibility = Visibility.Collapsed;
                    iconButton.TitleTextBlock.Foreground = iconButton.TitleForegroundInRestState;
                    iconButton.Container.Background = iconButton.BackgroundInRestState;
                }
            }
        }
        #endregion

        #region Image Margin
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register("ImageMargin", typeof(Thickness), typeof(IconButton), new PropertyMetadata(new Thickness(0), OnImageMarginChanged));

        public Thickness ImageMargin
        {
            set { this.SetValue(ImageMarginProperty, value); }
            get { return (Thickness)this.GetValue(ImageMarginProperty); }
        }
        private static void OnImageMarginChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                iconButton.PressedImage.Margin = iconButton.ImageMargin;
                iconButton.RestImage.Margin = iconButton.ImageMargin;
            }
        }
        #endregion

        #region Number of notification
        public static readonly DependencyProperty NotificationNumberProperty = DependencyProperty.Register("NotificationNumber", typeof(int), typeof(IconButton), new PropertyMetadata(0, NotificationNumberChanged));

        public int NotificationNumber
        {
            set { SetValue(NotificationNumberProperty, value); }
            get { return (int)GetValue(NotificationNumberProperty); }
        }
        private static void NotificationNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                if (iconButton.NotificationNumber > 0)
                {
                    iconButton.NotificationCounTextBlock.Text = iconButton.NotificationNumber > 9 ? "9+" : iconButton.NotificationNumber.ToString(CultureInfo.InvariantCulture);
                    iconButton.NotificationContainerBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    iconButton.NotificationCounTextBlock.Text = String.Empty;
                    iconButton.NotificationContainerBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region Type

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(ButtonType), typeof(IconButton), new PropertyMetadata(ButtonType.Normal, OnTypeChanged));

        public ButtonType Type
        {
            set { this.SetValue(TypeProperty, value); }
            get { return (ButtonType)this.GetValue(TypeProperty); }
        }
        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var iconButton = sender as IconButton;
            if (iconButton != null)
            {
                switch (iconButton.Type)
                {
                    case ButtonType.Normal:
                        iconButton.IsChecked = false;
                        break;
                    case ButtonType.Toogle:
                        iconButton.IsChecked = !iconButton.IsChecked;
                        break;
                }
            }
        }

        #endregion

        private void IconButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            switch (Type)
            {
                case ButtonType.Normal:
                    IsChecked = false;
                    break;
                case ButtonType.Toogle:
                    IsChecked = !IsChecked;
                    break;
            }        
        }
    }
}
