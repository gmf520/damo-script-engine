using System.Windows.Controls;
using Liuliu.ScriptEngine;

namespace Liuliu.MouseClicker.UserControls
{
    /// <summary>
    /// ClickSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class ClickSettingsView : UserControl
    {
        public ClickSettingsView()
        {
            InitializeComponent();
        }

        private void winSpy_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (winSpy.IsMouseLeftButtonDown)
            {
                DmPlugin dm = SoftContext.DmSystem.Dm;
                int hwnd = dm.GetMousePointWindow();
                SoftContext.Locator.ClickSettings.Window = new DmWindow(dm, hwnd);
            }
        }
    }
}
