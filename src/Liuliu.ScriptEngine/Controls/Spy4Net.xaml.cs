using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OSharp.Utility.Extensions;

using Brush = System.Windows.Media.Brush;
using Res = Liuliu.ScriptEngine.Properties.Resources;

namespace Liuliu.ScriptEngine.Controls
{
    /// <summary>
    /// Spy4Net.xaml 的交互逻辑
    /// </summary>
    public partial class Spy4Net : UserControl
    {
        public Spy4Net()
        {
            InitializeComponent();
            Background = new ImageBrush(ToImageSource(Res.drag_fill));
        }

        /// <summary>
        /// 获取 鼠标左键是否已按下
        /// </summary>
        public bool IsMouseLeftButtonDown { get; private set; }

        private BitmapImage ToImageSource(Bitmap bmp)
        {
            byte[] bytes = bmp.ToBytes();
            using (MemoryStream ms = new MemoryStream(bytes, false))
            {
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.StreamSource = ms;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
                return source;
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)e.Source).CaptureMouse();
            IsMouseLeftButtonDown = true;
            Background = new ImageBrush(ToImageSource(Res.drag_blank));
            byte[] bytes = Res.drag_eye;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Cursor = new Cursor(ms);
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Background = new ImageBrush(ToImageSource(Res.drag_fill));
            Cursor = Cursors.Arrow;
            IsMouseLeftButtonDown = false;
            ((UIElement)e.Source).ReleaseMouseCapture();
        }
    }
}
