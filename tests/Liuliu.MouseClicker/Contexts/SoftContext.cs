using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Liuliu.MouseClicker.Contexts;
using Liuliu.MouseClicker.ViewModels;
using Liuliu.ScriptEngine;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Practices.ServiceLocation;

using OSharp.Utility.Data;


namespace Liuliu.MouseClicker
{
    public class SoftContext
    {
        static SoftContext()
        {
            Version = GetVersion();
        }

        public static MainWindow MainWindow { get; set; }

        public static SoftRunStatus RunStatus { get; set; }

        public static ViewModelLocator Locator { get; } = ServiceLocator.Current.GetInstance<ViewModelLocator>();

        public static Version Version { get; }

        public static DmSystem DmSystem { get; set; }

        public static ProgressDialogController Progress { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static OperationResult Initialize()
        {
            var settings = Locator.Settings;
            string dmPath = settings.DmFile;
            var result = OperationHelper.GetDmSystem(dmPath);
            if (result.Successed)
            {
                DmSystem = result.Data;
                settings.DmVersion = DmSystem.Dm.Ver();
            }
            return new OperationResult(result.ResultType, result.Message);
        }

        /// <summary>
        /// 获取Progress对象
        /// </summary>
        public static Task<ProgressDialogController> GetProgress(string message)
        {
            return GetProgress("请稍候", message);
        }

        /// <summary>
        /// 获取Progress对象
        /// </summary>
        public static async Task<ProgressDialogController> GetProgress(string title, string message)
        {
            if (Progress == null || !Progress.IsOpen)
            {
                Progress = await MainWindow.ShowProgressAsync(title, message);
            }
            else
            {
                Progress.SetMessage(message);
            }
            return Progress;
        }

        /// <summary>
        /// 显示 ShowMessageAsync 弹窗信息
        /// </summary>
        public static async Task<MessageDialogResult> ShowMessageAsync(string title, string message)
        {
            if (Progress != null && Progress.IsOpen)
            {
                await Progress.CloseAsync();
            }
            return await MainWindow.ShowMessageAsync(title, message);
        }

        /// <summary>
        /// 关闭 Progress 弹窗  
        /// </summary>
        public static async Task ProgressCloseAsync()
        {
            if (Progress.IsOpen)
            {
                await Progress.CloseAsync();
            }
        }

        private static Version GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly.Location != null)
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return new Version(fvi.FileVersion);
            }
            return new Version("0.0.0.1");
        }
    }
}
