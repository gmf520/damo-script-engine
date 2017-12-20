using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using OSharp.Utility.Extensions;


namespace Liuliu.MouseClicker
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 初始化一个<see cref="App"/>类型的新实例
        /// </summary>
        public App()
        {
            //注册全局事件
            AppDomain.CurrentDomain.UnhandledException += async (sender, e) => await CurrentDomain_UnhandledException(sender, e);
            DispatcherUnhandledException += async (sender, e) => await App_DispatcherUnhandledException(sender, e);
            TaskScheduler.UnobservedTaskException += async (sender, e) => await TaskScheduler_UnobservedTaskException(sender, e);
        }


        private async Task CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            const string msg = "未捕获主线程异常";
            try
            {
                if (e.ExceptionObject is Exception)
                {
                    Dispatcher.Invoke(new Action(async () =>
                    {
                        Exception ex = (Exception)(e.ExceptionObject);
                        HandleException(msg, ex);
                        await FatalReport(ex);
                    }));
                }
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private async Task App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string msg = "未捕获子线程异常";
            try
            {
                HandleException(msg, e.Exception);
                e.Handled = true;
                await FatalReport(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private async Task TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            const string msg = "未捕获异步异常";
            try
            {
                HandleException(msg, e.Exception);
                e.SetObserved();
                await FatalReport(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void HandleException(string msg, Exception ex)
        {
            //_logger.Fatal(msg, ex);
        }

        private async Task FatalReport(Exception exception)
        {
            if (exception is InvalidCastException && exception.Message.Contains(".Windows.Media.Visual"))
            {
                return;
            }

            if (SoftContext.Progress != null && SoftContext.Progress.IsOpen)
            {
                await Task.Delay(2000);
                await SoftContext.Progress.CloseAsync();
            }
            List<string> lines = new List<string>();
            while (exception != null)
            {
                lines.Add(exception.Message);
                exception = exception.InnerException;
            }
            await SoftContext.ShowMessageAsync("程序错误", $"错误消息：{lines.ExpandAndToString("\r\n---")}");
        }
    }
}
