// -----------------------------------------------------------------------
//  <copyright file="MainWindow.xaml.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-15 12:47</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows;

using Liuliu.MouseClicker.ViewModels;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Practices.ServiceLocation;

using OSharp.Utility.Data;


namespace Liuliu.MouseClicker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MetroDialogOptions.AffirmativeButtonText = "确定";
            MetroDialogOptions.NegativeButtonText = "取消";
            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            SoftContext.MainWindow = this;
            Loaded += async (o, args) => await MainWindow_Loaded(o, args);
        }

        public ViewModelLocator Locator
        {
            get { return ServiceLocator.Current.GetInstance<ViewModelLocator>(); }
        }

        private async Task MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressDialogController progress = await SoftContext.GetProgress("正在初始化，请稍候……");
            OperationResult initResult = SoftContext.Initialize();
            if (!initResult.Successed)
            {
                await SoftContext.ShowMessageAsync("初始化失败", initResult.Message);
                SoftContext.RunStatus = SoftRunStatus.StartFail;
                return;
            }
            await SoftContext.ProgressCloseAsync();
            Locator.Main.StatusBar = $"准备就绪，大漠版本：{SoftContext.DmSystem.Dm.Ver()}";
        }

        private void Button_Initialized(object sender, System.EventArgs e)
        {
            CmdMenuButton.ContextMenu = null;
        }

        private void CmdMenuButton_Click(object sender, RoutedEventArgs e)
        {
            CmdMenu.PlacementTarget = (UIElement)sender;
            CmdMenu.IsOpen = true;
        }
    }
}