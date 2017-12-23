// -----------------------------------------------------------------------
//  <copyright file="ClickSettingViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-23 15:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using Liuliu.MouseClicker.Mvvm;
using Liuliu.ScriptEngine;


namespace Liuliu.MouseClicker.ViewModels
{
    public class ClickSettingsViewModel : ViewModelExBase
    {
        private DmWindow _window;
        public DmWindow Window
        {
            get { return _window; }
            set { SetProperty(ref _window, value, () => Window); }
        }

        private string _bindText = "绑定";
        public string BindText
        {
            get { return _bindText; }
            set { SetProperty(ref _bindText, value, () => BindText); }
        }

        public ICommand WindowBindCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (!_window.IsBind)
                    {
                        _window.BindHalfBackground();
                        BindText = "解绑";
                    }
                    else
                    {
                        _window.UnBind();
                        BindText = "绑定";
                    }
                });
            }
        }
    }
}