// -----------------------------------------------------------------------
//  <copyright file="ViewModelLocator.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-15 12:47</last-date>
// -----------------------------------------------------------------------

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;


namespace Liuliu.MouseClicker.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            RegisterViewModels();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public MainCommandViewModel MainCommand
        {
            get { return ServiceLocator.Current.GetInstance<MainCommandViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get
            {
                var model = ServiceLocator.Current.GetInstance<SettingsViewModel>();
                if (model.DmVersion == null)
                {
                    model.InitFromLocal();
                }
                return model;
            }
        }

        public ClickSettingsViewModel ClickSettings
        {
            get { return ServiceLocator.Current.GetInstance<ClickSettingsViewModel>(); }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<ViewModelLocator>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainCommandViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<ClickSettingsViewModel>();
        }
    }
}