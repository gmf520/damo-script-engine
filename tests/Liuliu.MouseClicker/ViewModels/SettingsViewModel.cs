// -----------------------------------------------------------------------
//  <copyright file="SettingsViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-17 19:02</last-date>
// -----------------------------------------------------------------------

using System.IO;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Liuliu.MouseClicker.Mvvm;

using OSharp.Utility.Extensions;


namespace Liuliu.MouseClicker.ViewModels
{
    public class SettingsViewModel : ViewModelExBase
    {
        private string _dmFile = "dm.dll";
        public string DmFile
        {
            get { return _dmFile; }
            set { SetProperty(ref _dmFile, value, () => DmFile); }
        }

        private bool _dmVersionShow;
        public bool DmVersionShow
        {
            get { return _dmVersionShow; }
            set { SetProperty(ref _dmVersionShow, value, () => DmVersionShow); }
        }

        private string _dmVersion;
        public string DmVersion
        {
            get { return _dmVersion; }
            set { SetProperty(ref _dmVersion, value, () => DmVersion); }
        }

        private bool _dmRegCodeShow;
        public bool DmRegCodeShow
        {
            get { return _dmRegCodeShow; }
            set { SetProperty(ref _dmRegCodeShow, value, () => DmRegCodeShow); }
        }

        private string _dmRegCode;
        public string DmRegCode
        {
            get { return _dmRegCode; }
            set { SetProperty(ref _dmRegCode, value, () => DmRegCode); }
        }

        public ICommand DmFileBrowseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send("DmFileBrowse","SettingsFlyout");
                });
            }
        }

        public override string Error
        {
            get
            {
                if (!File.Exists(DmFile))
                {
                    return "大漠插件文件不存在";
                }
                if (DmRegCodeShow && DmRegCode.IsMissing())
                {
                    return "大漠注册码不能为空";
                }
                return base.Error;
            }
        }
    }
}