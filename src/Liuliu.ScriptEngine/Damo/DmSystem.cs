// -----------------------------------------------------------------------
//  <copyright file="DmSystem.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:28</last-date>
// -----------------------------------------------------------------------

using System;


namespace Liuliu.ScriptEngine.Damo
{
    public class DmSystem
    {
        private readonly DmPlugin _dm;

        public DmSystem(DmPlugin dm)
        {
            _dm = dm;
        }

        /// <summary>
        /// 获取 公共大漠对象
        /// </summary>
        public DmPlugin Dm { get { return _dm; } }

        public string HostName { get { return Environment.UserDomainName; } }

        /// <summary>
        /// 获取 操作系统类型
        /// </summary>
        public string SystemType
        {
            get
            {
                int t = _dm.GetOSType();
                switch (t)
                {
                    case 0:
                        return "Win95/98/ME";
                    case 1:
                        return "Win2000/XP";
                    case 2:
                        return "Win2003";
                    case 3:
                        return "Win Vista/7/2008";
                    default:
                        return "未知系统";
                }
            }
        }

        /// <summary>
        /// 获取 屏幕色深
        /// </summary>
        public int ScreenDepth { get { return _dm.GetScreenDepth(); } }

        public int ScreenWidth { get { return _dm.GetScreenWidth(); } }

        public int ScreenHeight { get { return _dm.GetScreenHeight(); } }

        public bool Is64Bit { get { return _dm.Is64Bit(); } }

        public bool IsUacOpen { get { return _dm.CheckUAC(); } }

        /// <summary>
        /// 获取 硬盘序列号
        /// </summary>
        public string DiskSerial { get { return _dm.GetDiskSerial(); } }

        /// <summary>
        /// 获取 机器码，排除Mac地址
        /// </summary>
        public string MachineCodeNoMac { get { return _dm.GetMachineCodeNoMac(); } }

        /// <summary>
        /// 【系统】蜂鸣器
        /// </summary>
        /// <param name="fre">频率</param>
        /// <param name="delay">时长</param>
        /// <returns>0.失败，1.成功</returns>
        public void Beep(int fre, int delay)
        {
            _dm.Beep(fre, delay);
        }

        /// <summary>
        /// 设置当前系统的UAC（用户账户控制）
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <returns>操作是否成功</returns>
        public bool SetUac(bool enable)
        {
            return _dm.SetUAC(enable);
        }

        /// <summary>
        /// 【系统】设置系统的 系统色深
        /// </summary>
        /// <param name="depth">系统色深</param>
        /// <returns>0.失败，1.成功</returns>
        public bool SetScreenDepth(int depth)
        {
            return _dm.SetScreen(ScreenWidth, ScreenHeight, depth);
        }

        /// <summary>
        /// 【系统】关闭电源管理，不会进入睡眠
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public bool DisablePowerSave()
        {
            return _dm.DisablePowerSave();
        }

        /// <summary>
        /// 【系统】关闭屏幕保护
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public bool DisableScreenSave()
        {
            return _dm.DisablePowerSave();
        }

        /// <summary>
        /// 【系统】退出系统(注销 重启 关机)
        /// </summary>
        /// <param name="type">0.注销系统，1.关机，2.重新启动</param>
        /// <returns>0.失败，1.成功</returns>
        public bool ExitOs(int type)
        {
            return _dm.ExitOs(type);
        }
    }
}