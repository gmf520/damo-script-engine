// -----------------------------------------------------------------------
//  <copyright file="DmPlugin.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using OSharp.Utility.Exceptions;


namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 大漠插件C#免注册调用类
    /// 作者：清风抚断云（QQ:274838061）
    /// 本模块必须包含dmc.dll 实现不用注册dm.dll 到系统可以动态调用
    /// 分类并整理，添加注释：柳柳英侠（QQ:123202901）
    /// </summary>
    public class DmPlugin : IDisposable
    {
        private static readonly string DmName = GetDmName();
        private IntPtr _dm = IntPtr.Zero;
        private bool _disposed;
        private bool? _isFree;

        /// <summary>
        /// 初始化一个<see cref="DmPlugin"/>的新实例。
        /// </summary>
        public DmPlugin(bool showError = false)
        {
            //ReleaseResource(DmName);
            //ReleaseResource(DmcName);
            _dm = NativeMethods.CreateDM(DmName);
            if (_dm == IntPtr.Zero)
            {
                throw new OSharpException("创建大漠对象失败");
            }
            SetShowErrorMsg(showError);
        }

        /// <summary>
        /// 初始化一个<see cref="DmPlugin"/>类型的新实例
        /// </summary>
        public DmPlugin(string dict, string dictPwd, bool showError = false)
            : this(showError)
        {
            SetDictPwd(dictPwd);
            SetDict(0, dict);
            UseDict(0);
        }

        /// <summary>
        /// 获取 插件对象指针
        /// </summary>
        public IntPtr IntPtr
        {
            get { return _dm; }
        }

        /// <summary>
        /// 获取 是否收费
        /// </summary>
        public bool IsFree
        {
            get
            {
                if (_isFree == null)
                {
                    string ver = Ver();
                    _isFree = new Version(ver) <= new Version("3.1233");
                }
                return _isFree.Value;
            }
        }

        #region 私有方法

        private static string GetDmName()
        {
            Version version = Environment.OSVersion.Version;
            if (version.Major > 6 || version.Major == 6 && version.Minor > 1)
            {
                //系统大于Win7，返回5.1收费版本
                return "dmb.dll";
            }
            //XP Win7，返回3.1免费版本
            return "dma.dll";
        }

        #endregion

        #region 公共方法

        #region 基本设置

        /// <summary>
        /// 调用此函数来注册，从而使用插件的高级功能.推荐使用此函数.
        /// </summary>
        public int Reg(string code, string ver = null)
        {
            return NativeMethods.Reg(_dm, code, ver);
        }

        /// <summary>
        /// 【基本】获取注册在系统的dm.dll的路径。
        /// </summary>
        /// <returns>返回dm.dll所在路径</returns>
        public string GetBasePath()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetBasePath(_dm));
        }

        /// <summary>
        /// 【基本】返回当前大漠对象的ID值，这个值对于每个对象是唯一存在的。可以用来判定两个大漠对象是否一致。
        /// </summary>
        /// <returns>当前对象的ID值</returns>
        public long GetID()
        {
            return NativeMethods.GetID(_dm);
        }

        /// <summary>
        /// 【基本】返回当前插件版本号。
        /// </summary>
        /// <returns>当前插件的版本描述字符串</returns>
        public string Ver()
        {
            return Marshal.PtrToStringUni(NativeMethods.Ver(_dm));
        }

        /// <summary>
        /// 【基本】获取插件命令的最后错误.
        /// 必须紧跟上一句函数调用，中间任何的语句调用都会改变这个值
        /// </summary>
        /// <returns>表示错误值。 0表示无错误.
        /// -1 : 表示你使用了绑定里的收费功能，但是没注册，无法使用.
        /// -2 : 使用模式0 2 4 6时出现，因为目标窗口有保护，或者目标窗口没有以管理员权限打开. 常见于win7以上系统.或者有安全软件拦截插件.解决办法: 关闭所有安全软件，并且关闭系统UAC,然后再重新尝试. 如果还不行就可以肯定是目标窗口有特殊保护. 
        /// -3 : 使用模式0 2 4 6时出现，可能目标窗口有保护，也可能是异常错误.
        /// -4 : 使用模式1 3 5 7 101 103时出现，这是异常错误.
        /// -5 : 使用模式1 3 5 7 101 103时出现, 这个错误的解决办法就是关闭目标窗口，重新打开再绑定即可. 也可能是运行脚本的进程没有管理员权限. 
        /// -6 -7 -9 : 使用模式1 3 5 7 101 103时出现,异常错误. 还有可能是安全软件的问题，比如360等。尝试卸载360.
        /// -8 -10 : 使用模式1 3 5 7 101 103时出现, 目标进程可能有保护,也可能是插件版本过老，试试新的或许可以解决.
        /// -11 : 使用模式1 3 5 7 101 103时出现, 目标进程有保护. 告诉我解决。
        /// -12 : 使用模式1 3 5 7 101 103时出现, 目标进程有保护. 告诉我解决。
        /// -13 : 使用模式1 3 5 7 101 103时出现, 目标进程有保护. 或者是因为上次的绑定没有解绑导致。 尝试在绑定前调用ForceUnBindWindow.
        /// -14 : 使用模式0 1 4 5时出现, 有可能目标机器兼容性不太好. 可以尝试其他模式. 比如2 3 6 7
        /// -16 : 可能使用了绑定模式 0 1 2 3 和 101，然后可能指定了一个子窗口.导致不支持.可以换模式4 5 6 7或者103来尝试. 另外也可以考虑使用父窗口或者顶级窗口.来避免这个错误。还有可能是目标窗口没有正常解绑 然后再次绑定的时候.
        /// -17 : 模式1 3 5 7 101 103时出现. 这个是异常错误. 告诉我解决.
        /// -18 : 句柄无效.
        /// -19 : 使用模式0 1 2 3 101时出现,说明你的系统不支持这几个模式. 可以尝试其他模式.</returns>
        public int GetLastError()
        {
            return NativeMethods.GetLastError(_dm);
        }

        /// <summary>
        /// 【基本】获取全局路径.(可用于调试)
        /// </summary>
        /// <returns>以字符串的形式返回当前设置的全局路径</returns>
        public string GetPath()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetPath(_dm));
        }

        /// <summary>
        /// 【基本】设置全局路径,设置了此路径后,所有接口调用中,相关的文件都相对于此路径. 比如图片,字库等。
        /// </summary>
        /// <param name="path">路径,可以是相对路径,也可以是绝对路径</param>
        /// <returns>1成功，0失败。</returns>
        public int SetPath(string path)
        {
            return NativeMethods.SetPath(_dm, path);
        }

        /// <summary>
        /// 【基本】设定图色的获取方式，默认是显示器或者后台窗口(具体参考BindWindow)
        /// </summary>
        /// <param name="mode">图色输入模式，取值可为"screen"或"pic:file"</param>
        /// <returns>1成功，0失败。</returns>
        public long SetDisplayInput(string mode)
        {
            return NativeMethods.SetDisplayInput(_dm, mode);
        }

        /// <summary>
        /// 【基本】设置是否弹出错误信息,默认是打开
        /// </summary>
        /// <param name="isShow">是否显示错误信息，默认显示</param>
        /// <returns>1成功，0失败。</returns>
        public long SetShowErrorMsg(bool isShow = true)
        {
            int show = isShow ? 1 : 0;
            return NativeMethods.SetShowErrorMsg(_dm, show);
        }

        #endregion

        #region 窗口

        /// <summary>
        /// 【窗口】把窗口坐标转换为屏幕坐标
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">窗口X坐标</param>
        /// <param name="y">窗口Y坐标</param>
        /// <returns>1成功，0失败。</returns>
        public long ClientToScreen(int hwnd, ref object x, ref object y)
        {
            return NativeMethods.ClientToScreen(_dm, hwnd, ref x, ref y);
        }

        /// <summary>
        /// 【窗口】把屏幕坐标转换为窗口坐标
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">窗口X坐标</param>
        /// <param name="y">窗口Y坐标</param>
        /// <returns>1成功，0失败。</returns>
        public int ScreenToClient(int hwnd, ref object x, ref object y)
        {
            return NativeMethods.ScreenToClient(_dm, hwnd, ref x, ref y);
        }

        /// <summary>
        /// 【窗口】查找第一个符合类名或者标题名的顶层窗口
        /// </summary>
        /// <param name="className">窗口类名，如果为空，则匹配所有</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindow(string className, string title)
        {
            return NativeMethods.FindWindow(_dm, className, title);
        }

        /// <summary>
        /// 【窗口】查找第一个符合类名或者标题名的顶层窗口,如果指定了parent,则在parent的第一层子窗口中查找。
        /// </summary>
        /// <param name="parent">父窗口句柄，如果为空，则匹配所有顶层窗口</param>
        /// <param name="className">窗口类名，如果为空，则匹配所有</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有</param>
        public int FindWindowEx(int parent, string className, string title)
        {
            return NativeMethods.FindWindowEx(_dm, parent, className, title);
        }

        /// <summary>
        /// 【窗口】根据指定条件,枚举系统中符合条件的窗口,可以枚举到按键自带的无法枚举到的窗口
        /// </summary>
        /// <param name="parent">获得的窗口句柄是该窗口的子窗口的窗口句柄,取0时为获得桌面句柄</param>
        /// <param name="title">窗口标题</param>
        /// <param name="className">窗口类名</param>
        /// <param name="filter">取值定义如下：
        /// 1.匹配窗口标题,参数title有效，
        /// 2.匹配窗口类名,参数class_name有效，
        /// 4.只匹配指定父窗口的第一层孩子窗口，
        /// 8.匹配所有者窗口为0的窗口,即顶级窗口，
        /// 16.匹配可见的窗口。
        /// 这些值可以相加,比如4+8+16就是类似于任务管理器中的窗口列表</param>
        /// <returns>返回所有匹配的窗口句柄字符串,格式"hwnd1,hwnd2,hwnd3"</returns>
        public string EnumWindow(int parent, string title, string className, int filter)
        {
            return Marshal.PtrToStringUni(NativeMethods.EnumWindow(_dm, parent, title, className, filter));
        }

        /// <summary>
        /// 【窗口】根据指定进程以及其它条件,枚举系统中符合条件的窗口,可以枚举到按键自带的无法枚举到的窗口
        /// </summary>
        /// <param name="processName">进程映像名.比如(svchost.exe). 此参数是精确匹配,但不区分大小写</param>
        /// <param name="title">窗口标题. 此参数是模糊匹配</param>
        /// <param name="className">窗口类名. 此参数是模糊匹配</param>
        /// <param name="filter">1.匹配窗口标题,参数title有效
        /// 2.匹配窗口类名,参数class_name有效
        /// 4.只匹配指定映像的所对应的第一个进程. 可能有很多同映像名的进程，只匹配第一个进程的.
        /// 8.匹配所有者窗口为0的窗口,即顶级窗口
        /// 16.匹配可见的窗口
        /// 32.匹配出的窗口按照窗口打开顺序依次排列(收费功能)
        /// 这些值可以相加,比如4+8+16</param>
        /// <returns>返回所有匹配的窗口句柄字符串,格式"hwnd1,hwnd2,hwnd3"</returns>
        public string EnumWindowByProcess(string processName, string title, string className, int filter)
        {
            return Marshal.PtrToStringUni(NativeMethods.EnumWindowByProcess(_dm, processName, title, className, filter));
        }

        /// <summary>
        /// 【窗口】获取窗口客户区域的宽度和高度
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>1成功，0失败。</returns>
        public int GetClientSize(int hwnd, out int width, out int height)
        {
            object w, h;
            int result = NativeMethods.GetClientSize(_dm, hwnd, out w, out h);
            width = (int)w;
            height = (int)h;
            return result;
        }

        /// <summary>
        /// 【窗口】获取顶层活动窗口中具有输入焦点的窗口句柄
        /// </summary>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetForegroundFocus()
        {
            return NativeMethods.GetForegroundFocus(_dm);
        }

        /// <summary>
        /// 【窗口】获取顶层活动窗口,可以获取到按键自带插件无法获取到的句柄
        /// </summary>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow(_dm);
        }

        /// <summary>
        /// 【窗口】获取鼠标指向的窗口句柄,可以获取到按键自带的插件无法获取到的句柄
        /// </summary>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetMousePointWindow()
        {
            return NativeMethods.GetMousePointWindow(_dm);
        }

        /// <summary>
        /// 【窗口】获取给定坐标的窗口句柄,可以获取到按键自带的插件无法获取到的句柄
        /// </summary>
        /// <param name="x">屏幕X坐标</param>
        /// <param name="y">屏幕Y坐标</param>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetPointWindow(int x, int y)
        {
            return NativeMethods.GetPointWindow(_dm, x, y);
        }

        /// <summary>
        /// 【窗口】获取特殊窗口
        /// </summary>
        /// <param name="flag">标志：0为获取桌面窗口，1为获取任务栏窗口</param>
        /// <returns>以整型数表示的窗口句柄</returns>
        public int GetSpecialWindow(int flag)
        {
            return NativeMethods.GetSpecialWindow(_dm, flag);
        }

        /// <summary>
        /// 【窗口】获取给定窗口相关的窗口句柄
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="flag">标志：
        /// 0.获取父窗口，
        /// 1.获取第一个子窗口，
        /// 2.获取First窗口，
        /// 3.获取Last窗口，
        /// 4.获取下一个窗口，
        /// 5.获取上一个窗口，
        /// 6.获取拥有者窗口</param>
        /// <returns></returns>
        public int GetWindow(int hwnd, int flag)
        {
            return NativeMethods.GetWindow(_dm, hwnd, flag);
        }

        /// <summary>
        /// 【窗口】获取窗口的标题
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>窗口的标题</returns>
        public string GetWindowTitle(int hwnd)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWindowTitle(_dm, hwnd));
        }

        /// <summary>
        /// 【窗口】获取窗口的类名
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>窗口的类名</returns>
        public string GetWindowClass(int hwnd)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWindowClass(_dm, hwnd));
        }

        /// <summary>
        /// 【窗口】获取指定窗口所在的进程ID。
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>返回整型表示的是进程ID</returns>
        public int GetWindowProcessId(int hwnd)
        {
            return NativeMethods.GetWindowProcessId(_dm, hwnd);
        }

        /// <summary>
        /// 【窗口】获取指定窗口所在的进程的exe文件全路径
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>返回字符串表示的是exe全路径名</returns>
        public string GetWindowProcessPath(int hwnd)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWindowProcessPath(_dm, hwnd));
        }

        /// <summary>
        /// 【窗口】获取窗口在屏幕上的位置
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x1">窗口左上角X坐标</param>
        /// <param name="y1">窗口左上角Y坐标</param>
        /// <param name="x2">窗口右下角X坐标</param>
        /// <param name="y2">窗口右下角Y坐标</param>
        /// <returns>1成功，0失败。</returns>
        public int GetWindowRect(int hwnd, out int x1, out int y1, out int x2, out int y2)
        {
            object a, b, c, d;
            int result = NativeMethods.GetWindowRect(_dm, hwnd, out a, out b, out c, out d);
            x1 = (int)a;
            y1 = (int)b;
            x2 = (int)c;
            y2 = (int)d;
            return result;
        }

        /// <summary>
        /// 【窗口】获取指定窗口的一些属性
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="flag">标志：
        /// 0.判断窗口是否存在
        /// 1.判断窗口是否处于激活
        /// 2.判断窗口是否可见
        /// 3.判断窗口是否最小化
        /// 4.判断窗口是否最大化
        /// 5.判断窗口是否置顶 
        /// 6.判断窗口是否无响应</param>
        /// <returns>0: 不满足条件，1: 满足条件</returns>
        public int GetWindowState(int hwnd, int flag)
        {
            return NativeMethods.GetWindowState(_dm, hwnd, flag);
        }

        /// <summary>
        /// 【窗口】移动指定窗口到指定位置
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int MoveWindow(int hwnd, int x, int y)
        {
            return NativeMethods.MoveWindow(_dm, hwnd, x, y);
        }

        /// <summary>
        /// 【窗口】设置窗口客户区域的宽度和高度
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetClientSize(int hwnd, int width, int height)
        {
            return NativeMethods.SetClientSize(_dm, hwnd, width, height);
        }

        /// <summary>
        /// 【窗口】设置窗口的大小
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWindowSize(int hwnd, int width, int height)
        {
            return NativeMethods.SetWindowSize(_dm, hwnd, width, height);
        }

        /// <summary>
        /// 【窗口】设置窗口的状态
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="flag">功能标志：
        /// 0.关闭指定窗口，
        /// 1.激活指定窗口，
        /// 2.最小化指定窗口，
        /// 3.最小化指定窗口,并释放内存，
        /// 4.最大化指定窗口，
        /// 5.恢复指定窗口,但不激活，
        /// 6.隐藏指定窗口，
        /// 7.显示指定窗口，
        /// 8.置顶指定窗口，
        /// 9.取消置顶指定窗口，
        /// 10.禁止指定窗口，
        /// 11.取消禁止指定窗口，
        /// 12.恢复并激活指定窗口 </param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWindowState(int hwnd, int flag)
        {
            return NativeMethods.SetWindowState(_dm, hwnd, flag);
        }

        /// <summary>
        /// 【窗口】设置窗口的标题
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="title">标题</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWindowText(int hwnd, string title)
        {
            return NativeMethods.SetWindowText(_dm, hwnd, title);
        }

        /// <summary>
        /// 【窗口】设置窗口的透明度
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="trans">透明度 取值(0-255) 越小透明度越大 0为完全透明(不可见) 255为完全显示(不透明)</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWindowTransparent(int hwnd, int trans)
        {
            return NativeMethods.SetWindowTransparent(_dm, hwnd, trans);
        }

        /// <summary>
        /// 向指定窗口发送粘贴命令. 把剪贴板的内容发送到目标窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>0.失败，1.成功</returns>
        public int SendPaste(int hwnd)
        {
            return NativeMethods.SendPaste(_dm, hwnd);
        }

        /// <summary>
        /// 【窗口】向指定窗口发送文本数据
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="content">发送的文本数据</param>
        /// <returns>0.失败，1.成功</returns>
        public int SendString(int hwnd, string content)
        {
            int result = NativeMethods.SendString(_dm, hwnd, content);
            Delay(100);
            return result;
        }

        /// <summary>
        /// 【窗口】向指定窗口发送文本数据。此接口为老的SendString，如果新的SendString不能输入，可以尝试此接口。
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="content">发送的文本数据</param>
        /// <returns>0.失败，1.成功</returns>
        public int SendString2(int hwnd, string content)
        {
            int result = NativeMethods.SendString2(_dm, hwnd, content);
            Delay(100);
            return result;
        }

        #endregion

        #region 键盘鼠标

        /// <summary>
        /// 【键鼠】获取鼠标特征码。当BindWindow或者BindWindowEx中的mouse参数含有dm.mouse.cursor时，
        /// 获取到的是后台鼠标特征，否则是前台鼠标特征。
        /// </summary>
        /// <returns>成功时返回鼠标特征码，失败时返回空的串</returns>
        public string GetCursorShape()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetCursorShape(_dm));
        }

        /// <summary>
        /// 【键鼠】获取鼠标特征码. 当BindWindow或者BindWindowEx中的mouse参数含有dx.mouse.cursor时，
        /// 获取到的是后台鼠标特征，否则是前台鼠标特征.
        /// </summary>
        /// <param name="type">获取鼠标特征码的方式. 和工具中的方式1 方式2对应. 方式1此参数值为0. 方式2此参数值为1</param>
        /// <returns>成功时返回鼠标特征码，失败时返回空的串</returns>
        public string GetCursorShapeEx(int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetCursorShapeEx(_dm, type));
        }

        /// <summary>
        /// 【键鼠】获取鼠标热点位置。当BindWindow或者BindWindowEx中的mouse参数含有dm.mouse.cursor时，
        /// 获取到的是后台鼠标热点位置，否则是前台鼠标热点位置。
        /// </summary>
        /// <returns></returns>
        public string GetCursorSpot()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetCursorSpot(_dm));
        }

        /// <summary>
        /// 【键鼠】获取系统鼠标在屏幕上的位置
        /// </summary>
        /// <param name="x">返回X坐标</param>
        /// <param name="y">返回Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int GetCursorPos(out int x, out int y)
        {
            object a, b;
            int result = NativeMethods.GetCursorPos(_dm, out a, out b);
            x = (int)a;
            y = (int)b;
            return result;
        }

        /// <summary>
        /// 【键鼠】获取指定的按键状态.(前台信息,不是后台)
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>0.弹起，1.按下</returns>
        public int GetKeyState(int keyCode)
        {
            return NativeMethods.GetKeyState(_dm, keyCode);
        }

        /// <summary>
        /// 【键鼠】按住指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyDown(int keyCode)
        {
            return NativeMethods.KeyDown(_dm, keyCode);
        }

        /// <summary>
        /// 【键鼠】按住指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟键名</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyDown(string keyStr)
        {
            return NativeMethods.KeyDownChar(_dm, keyStr);
        }

        /// <summary>
        /// 【键鼠】弹起指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyUp(int keyCode)
        {
            return NativeMethods.KeyUp(_dm, keyCode);
        }

        /// <summary>
        /// 【键鼠】弹起指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟键名</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyUp(string keyStr)
        {
            return NativeMethods.KeyUpChar(_dm, keyStr);
        }

        /// <summary>
        /// 【键鼠】按下指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <param name="count">次数</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyPress(int keyCode, int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && NativeMethods.KeyPress(_dm, keyCode) == 1;
            }
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】按下指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟按键名</param>
        /// <param name="count">次数</param>
        /// <returns>0.失败，1.成功</returns>
        public int KeyPress(string keyStr, int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && NativeMethods.KeyPressChar(_dm, keyStr) == 1;
            }
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】等待指定的按键按下 (前台,不是后台)
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <param name="timeOut">等待多久,单位毫秒. 如果是0，表示一直等待</param>
        /// <returns>0.超时，1.指定的按键按下</returns>
        public int WaitKey(int keyCode, int timeOut)
        {
            return NativeMethods.WaitKey(_dm, keyCode, timeOut);
        }

        /// <summary>
        /// 【键鼠】按住鼠标左键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int LeftDown()
        {
            return NativeMethods.LeftDown(_dm);
        }

        /// <summary>
        /// 【键鼠】弹起鼠标左键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int LeftUp()
        {
            return NativeMethods.LeftUp(_dm);
        }

        /// <summary>
        /// 【键鼠】按下鼠标左键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int LeftClick(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && NativeMethods.LeftClick(_dm) == 1;
            }
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】按下鼠标左键并延时
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int LeftClickWithDelay(int mis)
        {
            int flag = NativeMethods.LeftDown(_dm);
            Delay(mis);
            return flag & NativeMethods.LeftUp(_dm);
        }

        /// <summary>
        /// 【键鼠】按下Ctrl键并单击鼠标左键
        /// </summary>
        /// <returns></returns>
        public int LeftClickCtrl()
        {
            bool flag = NativeMethods.KeyDown(_dm, 17) == 1;
            Delay(50);
            flag = flag && NativeMethods.LeftClick(_dm) == 1;
            Delay(50);
            flag = flag && NativeMethods.KeyUp(_dm, 17) == 1;
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】双击鼠标左键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int LeftDoubleClick()
        {
            return NativeMethods.LeftDoubleClick(_dm);
        }

        /// <summary>
        /// 【键鼠】按下鼠标中键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int MiddleClick()
        {
            return NativeMethods.MiddleClick(_dm);
        }

        /// <summary>
        /// 【键鼠】按住鼠标右键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int RightDown()
        {
            return NativeMethods.RightDown(_dm);
        }

        /// <summary>
        /// 【键鼠】弹起鼠标右键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int RightUp()
        {
            return NativeMethods.RightUp(_dm);
        }

        /// <summary>
        /// 【键鼠】按下鼠标右键
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int RightClick()
        {
            return NativeMethods.RightClick(_dm);
        }

        /// <summary>
        /// 【键鼠】滚轮向下滚
        /// </summary>
        /// <param name="count">次数</param>
        /// <returns>0.失败，1.成功</returns>
        public int WheelDown(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && NativeMethods.WheelDown(_dm) == 1;
            }
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】滚轮向上滚
        /// </summary>
        /// <param name="count">次数</param>
        /// <returns>0.失败，1.成功</returns>
        public int WheelUp(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && NativeMethods.WheelUp(_dm) == 1;
            }
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】鼠标相对于上次的位置移动rx,ry
        /// </summary>
        /// <param name="rx">相对于上次的X偏移</param>
        /// <param name="ry">相对于上次的Y偏移</param>
        /// <returns>0.失败，1.成功</returns>
        public int MoveR(int rx, int ry)
        {
            return NativeMethods.MoveR(_dm, rx, ry);
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int MoveTo(int x, int y)
        {
            return NativeMethods.MoveTo(_dm, x, y);
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的范围内的任意一点
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="w">宽度(从x计算起)</param>
        /// <param name="h">高度(从y计算起)</param>
        /// <returns>返回要移动到的目标点. 格式为x,y.  比如MoveToEx 100,100,10,10,返回值可能是101,102</returns>
        public string MoveToEx(int x, int y, int w, int h)
        {
            return Marshal.PtrToStringUni(NativeMethods.MoveToEx(_dm, x, y, w, h));
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)，并按下左键
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int MoveToClick(int x, int y)
        {
            bool flag = NativeMethods.MoveTo(_dm, x, y) == 1;
            Delay(300);
            flag = flag && NativeMethods.LeftClick(_dm) == 1;
            Delay(100);
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)，并按下右键
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int MoveToRightClick(int x, int y)
        {
            bool flag = NativeMethods.MoveTo(_dm, x, y) == 1;
            Delay(400);
            flag = flag && NativeMethods.RightClick(_dm) == 1;
            Delay(400);
            return flag ? 1 : 0;
        }

        /// <summary>
        /// 【键鼠】设置按键时,键盘按下和弹起的时间间隔。高级用户使用。某些窗口可能需要调整这个参数才可以正常按键。
        /// </summary>
        /// <param name="type">键盘类型,取值有以下：
        /// normal.对应normal鼠标 默认内部延时为 30ms
        /// windows.对应windows 鼠标 默认内部延时为 10ms
        /// dx.对应dx鼠标默认内部延时为40ms</param>
        /// <param name="delay">延时,单位是毫秒</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetKeypadDelay(string type, int delay)
        {
            return NativeMethods.SetKeypadDelay(_dm, type, delay);
        }

        /// <summary>
        /// 【键鼠】设置鼠标单击或者双击时,鼠标按下和弹起的时间间隔。高级用户使用。某些窗口可能需要调整这个参数才可以正常点击。
        /// </summary>
        /// <param name="type">鼠标类型,取值有以下：
        /// normal.对应normal鼠标 默认内部延时为 30ms
        /// windows.对应windows 鼠标 默认内部延时为 10ms
        /// dx.对应dx鼠标默认内部延时为40ms</param>
        /// <param name="delay">延时,单位是毫秒</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetMouseDelay(string type, int delay)
        {
            return NativeMethods.SetMouseDelay(_dm, type, delay);
        }

        #endregion

        #region 文字识别

        /// <summary>
        /// 【文字】设置字库文件
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <param name="file">字库文件名</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetDict(int index, string file)
        {
            return NativeMethods.SetDict(_dm, index, file);
        }

        /// <summary>
        /// 【文字】设置字库的密码,在SetDict前调用,目前的设计是,所有字库通用一个密码
        /// </summary>
        /// <param name="pwd">字库密码</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetDictPwd(string pwd)
        {
            return NativeMethods.SetDictPwd(_dm, pwd);
        }

        /// <summary>
        /// 【文字】给指定的字库中添加一条字库信息
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <param name="dictInfo">字库描述串，具体参考大漠综合工具中的字符定义</param>
        /// <returns>0.失败，1.成功</returns>
        public int AddDict(int index, string dictInfo)
        {
            return NativeMethods.AddDict(_dm, index, dictInfo);
        }

        /// <summary>
        /// 【文字】清空指定的字库
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <returns>0.失败，1.成功</returns>
        public int ClearDict(int index)
        {
            return NativeMethods.ClearDict(_dm, index);
        }

        /// <summary>
        /// 【文字】保存指定的字库到指定的文件中
        /// </summary>
        /// <param name="index">字库索引序号 取值为0-9对应10个字库</param>
        /// <param name="file">文件名</param>
        /// <returns>0.失败，1.成功</returns>
        public int SaveDict(int index, string file)
        {
            return NativeMethods.SaveDict(_dm, index, file);
        }

        /// <summary>
        /// 【文字】表示使用哪个字库文件进行识别(index范围:0-9)
        /// </summary>
        /// <param name="index">字库编号(0-9)</param>
        /// <returns>0.失败，1.成功</returns>
        public int UseDict(int index)
        {
            return NativeMethods.UseDict(_dm, index);
        }

        /// <summary>
        /// 【文字】根据指定的范围,以及指定的颜色描述，提取点阵信息，类似于大漠工具里的单独提取
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="word">待定义的文字,不能为空，且不能为关键符号"$"</param>
        /// <returns>识别到的点阵信息，可用于AddDict。如果失败，返回空</returns>
        public string FetchWord(int x1, int y1, int x2, int y2, string color, string word)
        {
            return Marshal.PtrToStringUni(NativeMethods.FetchWord(_dm, x1, y1, x2, y2, color, word));
        }

        /// <summary>
        /// 【文字】在屏幕范围(x1,y1,x2,y2)内,查找string(可以是任意个字符串的组合),并返回符合color_format的坐标位置,相似度sim同Ocr接口描述
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="intX">返回X坐标 没找到返回-1</param>
        /// <param name="intY">返回Y坐标 没找到返回-1</param>
        /// <returns>返回字符串的索引 没找到返回-1, 比如"长安|洛阳",若找到长安，则返回0</returns>
        public int FindStr(int x1, int y1, int x2, int y2, string str, string color, double sim, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.FindStr(_dm, x1, y1, x2, y2, str, color, sim, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】在屏幕范围(x1,y1,x2,y2)内,查找string(可以是任意个字符串的组合),并返回符合color_format的坐标位置,相似度sim同Ocr接口描述.
        /// (多色,差色查找类似于Ocr接口,不再重述)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回字符串序号以及X和Y坐标,形式如"id|x|y", 比如"0|100|200",没找到时，id和X以及Y均为-1，"-1|-1|-1"</returns>
        public string FindStrE(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrE(_dm, x1, y1, x2, y2, str, color, sim));
        }

        /// <summary>
        /// 【文字】在屏幕范围(x1,y1,x2,y2)内,查找string(可以是任意字符串的组合),并返回符合color_format的所有坐标位置,相似度sim同Ocr接口描述
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回所有找到的坐标集合,格式如下:
        /// "id,x0,y0|id,x1,y1|......|id,xn,yn"
        /// 比如"0,100,20|2,30,40" 表示找到了两个,第一个,对应的是序号为0的字符串,坐标是(100,20),第二个是序号为2的字符串,坐标(30,40)</returns>
        public string FindStrEx(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrEx(_dm, x1, y1, x2, y2, str, color, sim));
        }

        /// <summary>
        /// 【文字】同FindStr。注: 此函数比FindStr要快很多，尤其是在字库很大时，或者模糊识别时，效果非常明显。
        /// 推荐使用此函数。
        /// 另外由于此函数是只识别待查找的字符，所以可能会有如下情况出现问题。
        /// 比如 字库中有"张和三" 一共3个字符数据，然后待识别区域里是"张和三",如果用FindStr查找
        /// "张三"肯定是找不到的，但是用FindStrFast却可以找到，因为"和"这个字符没有列入查找计划中
        /// 所以，在使用此函数时，也要特别注意这一点。
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="intX">返回X坐标 没找到返回-1</param>
        /// <param name="intY">返回Y坐标 没找到返回-1</param>
        /// <returns>返回字符串的索引 没找到返回-1, 比如"长安|洛阳",若找到长安，则返回0</returns>
        public int FindStrFast(int x1, int y1, int x2, int y2, string str, string color, double sim, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.FindStrFast(_dm, x1, y1, x2, y2, str, color, sim, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】同FindStrE。注: 此函数比FindStrE要快很多，尤其是在字库很大时，或者模糊识别时，效果非常明显。
        /// 推荐使用此函数。
        /// 另外由于此函数是只识别待查找的字符，所以可能会有如下情况出现问题。
        /// 比如 字库中有"张和三" 一共3个字符数据，然后待识别区域里是"张和三",如果用FindStrE查找
        /// "张三"肯定是找不到的，但是用FindStrFastE却可以找到，因为"和"这个字符没有列入查找计划中
        /// 所以，在使用此函数时，也要特别注意这一点。
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回字符串序号以及X和Y坐标,形式如"id|x|y", 比如"0|100|200",没找到时，id和X以及Y均为-1，"-1|-1|-1"</returns>
        public string FindStrFastE(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrFastE(_dm, x1, y1, x2, y2, str, color, sim));
        }

        /// <summary>
        /// 【文字】同FindStrEx。注: 此函数比FindStrEx要快很多，尤其是在字库很大时，或者模糊识别时，效果非常明显。
        /// 推荐使用此函数。
        /// 另外由于此函数是只识别待查找的字符，所以可能会有如下情况出现问题。
        /// 比如 字库中有"张和三" 一共3个字符数据，然后待识别区域里是"张和三",如果用FindStrEx查找
        /// "张三"肯定是找不到的，但是用FindStrFastEx却可以找到，因为"和"这个字符没有列入查找计划中
        /// 所以，在使用此函数时，也要特别注意这一点。
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回所有找到的坐标集合,格式如下:
        /// "id,x0,y0|id,x1,y1|......|id,xn,yn"
        /// 比如"0,100,20|2,30,40" 表示找到了两个,第一个,对应的是序号为0的字符串,坐标是(100,20),第二个是序号为2的字符串,坐标(30,40)</returns>
        public string FindStrFastEx(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrFastEx(_dm, x1, y1, x2, y2, str, color, sim));
        }

        /// <summary>
        /// 【文字】同FindStr，但是不使用SetDict设置的字库，而利用系统自带的字库，速度比FindStr稍慢
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="fontName">系统字体名,比如"宋体"</param>
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程</param>
        /// <param name="flag">字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <param name="intX">返回X坐标 没找到返回-1</param>
        /// <param name="intY">返回Y坐标 没找到返回-1</param>
        /// <returns>返回字符串的索引 没找到返回-1, 比如"长安|洛阳",若找到长安，则返回0</returns>
        public int FindStrWithFont(int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            string fontName,
            int fontSize,
            int flag,
            out int intX,
            out int intY)
        {
            object x, y;
            int result = NativeMethods.FindStrWithFont(_dm, x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】同FindStrE，但是不使用SetDict设置的字库，而利用系统自带的字库，速度比FindStrE稍慢
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="fontName">系统字体名,比如"宋体"</param>
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程</param>
        /// <param name="flag">字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <returns>返回字符串序号以及X和Y坐标,形式如"id|x|y", 比如"0|100|200",没找到时，id和X以及Y均为-1，"-1|-1|-1"</returns>
        public string FindStrWithFontE(int x1, int y1, int x2, int y2, string str, string color, double sim, string fontName, int fontSize, int flag)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrWithFontE(_dm, x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag));
        }

        /// <summary>
        /// 【文字】同FindStrEx，但是不使用SetDict设置的字库，而利用系统自带的字库，速度比FindStrEx稍慢
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="fontName">系统字体名,比如"宋体"</param>
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程</param>
        /// <param name="flag">字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <returns>返回字符串的索引 没找到返回-1, 比如"长安|洛阳",若找到长安，则返回0</returns>
        public string FindStrWithFontEx(int x1, int y1, int x2, int y2, string str, string color, double sim, string fontName, int fontSize, int flag)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindStrWithFontEx(_dm, x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag));
        }

        /// <summary>
        /// 【文字】获取指定的字库中的字符数量
        /// </summary>
        /// <param name="index">字库序号(0-9)</param>
        /// <returns>字库数量</returns>
        public int GetDictCount(int index)
        {
            return NativeMethods.GetDictCount(_dm, index);
        }

        /// <summary>
        /// 【文字】根据指定的文字，以及指定的系统字库信息，获取字库描述信息
        /// </summary>
        /// <param name="str">需要获取的字符串</param>
        /// <param name="fontName">系统字体名,比如"宋体"</param>
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程</param>
        /// <param name="flag">字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <returns>返回字库信息,每个字符的字库信息用"|"来分割</returns>
        public string GetDictInfo(string str, string fontName, int fontSize, int flag)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetDictInfo(_dm, str, fontName, fontSize, flag));
        }

        /// <summary>
        /// 【文字】获取当前使用的字库序号(0-9)
        /// </summary>
        /// <returns>字库序号(0-9)</returns>
        public int GetNowDict()
        {
            return NativeMethods.GetNowDict(_dm);
        }

        /// <summary>
        /// 【文字】对插件部分接口的返回值进行解析,并返回ret中的坐标个数
        /// </summary>
        /// <param name="str">部分接口的返回串</param>
        /// <returns>返回ret中的坐标个数</returns>
        public int GetResultCount(string str)
        {
            return NativeMethods.GetResultCount(_dm, str);
        }

        /// <summary>
        /// 【文字】对插件部分接口的返回值进行解析,并根据指定的第index个坐标,返回具体的值
        /// </summary>
        /// <param name="str">部分接口的返回串</param>
        /// <param name="index">第几个坐标</param>
        /// <param name="intX">返回X坐标</param>
        /// <param name="intY">返回Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int GetResultPos(string str, int index, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.GetResultPos(_dm, str, index, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】在使用GetWords进行词组识别以后,可以用此接口进行识别词组数量的计算
        /// </summary>
        /// <param name="str">GetWords接口调用以后的返回值</param>
        /// <returns>返回词组数量</returns>
        public int GetWordResultCount(string str)
        {
            return NativeMethods.GetWordResultCount(_dm, str);
        }

        /// <summary>
        /// 【文字】在使用GetWords进行词组识别以后,可以用此接口进行识别各个词组的坐标
        /// </summary>
        /// <param name="str">GetWords的返回值</param>
        /// <param name="index">第几个坐标</param>
        /// <param name="intX">返回X坐标</param>
        /// <param name="intY">返回Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int GetWordResultPos(string str, int index, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.GetWordResultPos(_dm, str, index, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】在使用GetWords进行词组识别以后,可以用此接口进行识别各个词组的内容
        /// </summary>
        /// <param name="str">GetWords的返回值</param>
        /// <param name="index">第几个坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public string GetWordResultStr(string str, int index)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWordResultStr(_dm, str, index));
        }

        /// <summary>
        /// 【文字】根据指定的范围,以及设定好的词组识别参数(一般不用更改,除非你真的理解了)
        /// 识别这个范围内所有满足条件的词组. 比较适合用在未知文字的情况下,进行不定识别.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度 0.1-1.0</param>
        /// <returns>识别到的格式串,要用到专用函数来解析</returns>
        public string GetWords(int x1, int y1, int x2, int y2, string color, double sim)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWords(_dm, x1, y1, x2, y2, color, sim));
        }

        /// <summary>
        /// 【文字】根据指定的范围,以及设定好的词组识别参数(一般不用更改,除非你真的理解了)
        /// 识别这个范围内所有满足条件的词组. 这个识别函数不会用到字库。只是识别大概形状的位置
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <returns>识别到的格式串,要用到专用函数来解析</returns>
        public string GetWordsNoDict(int x1, int y1, int x2, int y2, string color)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetWordsNoDict(_dm, x1, y1, x2, y2, color));
        }

        /// <summary>
        /// 【文字】识别屏幕范围(x1,y1,x2,y2)内符合color_format的字符串,并且相似度为sim,sim取值范围(0.1-1.0),这个值越大越精确,越大速度越快,越小速度越慢,请斟酌使用！
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回识别到的字符串</returns>
        public string Ocr(int x1, int y1, int x2, int y2, string color, double sim = 1.0)
        {
            return Marshal.PtrToStringUni(NativeMethods.Ocr(_dm, x1, y1, x2, y2, color, sim));
        }

        /// <summary>
        /// 【文字】识别屏幕范围(x1,y1,x2,y2)内符合color_format的字符串,并且相似度为sim,sim取值范围(0.1-1.0),
        /// 这个值越大越精确,越大速度越快,越小速度越慢,请斟酌使用!
        /// 这个函数可以返回识别到的字符串，以及每个字符的坐标.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回识别到的字符串 格式如  "识别到的信息|x0,y0|…|xn,yn"</returns>
        public string OcrEx(int x1, int y1, int x2, int y2, string color, double sim = 1.0)
        {
            return Marshal.PtrToStringUni(NativeMethods.OcrEx(_dm, x1, y1, x2, y2, color, sim));
        }

        /// <summary>
        /// 【文字】识别位图中区域(x1,y1,x2,y2)的文字
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picName">图片名称</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回识别到的字符串</returns>
        public string OcrInFile(int x1, int y1, int x2, int y2, string picName, string color, double sim = 1.0)
        {
            return Marshal.PtrToStringUni(NativeMethods.OcrInFile(_dm, x1, y1, x2, y2, picName, color, sim));
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的行距,默认行距是1
        /// </summary>
        /// <param name="rowGap">文字行距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetRowGapNoDict(int rowGap = 1)
        {
            return NativeMethods.SetRowGapNoDict(_dm, rowGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的列距,默认列距是1
        /// </summary>
        /// <param name="colGap">文字列距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetColGapNoDict(int colGap = 1)
        {
            return NativeMethods.SetColGapNoDict(_dm, colGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别前,如果待识别区域有多行文字,可以设定行间距,默认的行间距是1
        /// </summary>
        /// <param name="minRowGap">最小行间距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetMinRowGap(int minRowGap = 1)
        {
            return NativeMethods.SetMinRowGap(_dm, minRowGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别前,如果待识别区域有多行文字,可以设定列间距,默认的列间距是0,如果根据情况设定,可以提高识别精度。一般不用设定。
        /// </summary>
        /// <param name="minColGap">最小列间距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetMinColGap(int minColGap = 1)
        {
            return NativeMethods.SetMinColGap(_dm, minColGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在使用文字识别功能前，设定是否开启精准识别。
        /// </summary>
        /// <param name="isExactOcr">是否开启精确识别，默认为开启。</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetExactOcr(bool isExactOcr = true)
        {
            int exactOcr = isExactOcr ? 1 : 0;
            return NativeMethods.SetExactOcr(_dm, exactOcr);
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别词组前,可设定词组间的间隔,默认的词组间隔是5
        /// </summary>
        /// <param name="wordGap">单词间距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWordGap(int wordGap = 5)
        {
            return NativeMethods.SetWordGap(_dm, wordGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定词组间的间隔,默认的词组间隔是1
        /// </summary>
        /// <param name="wordGap">单词间距</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWordGapNoDict(int wordGap = 1)
        {
            return NativeMethods.SetWordGapNoDict(_dm, wordGap);
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别词组前,可设定文字的平均行高,默认的词组行高是10
        /// </summary>
        /// <param name="lineHeight">行高</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWordLineHeight(int lineHeight = 10)
        {
            return NativeMethods.SetWordLineHeight(_dm, lineHeight);
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的平均行高,默认的词组行高是10
        /// </summary>
        /// <param name="lineHeight">行高</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetWordLineHeightNoDict(int lineHeight = 10)
        {
            return NativeMethods.SetWordLineHeightNoDict(_dm, lineHeight);
        }

        #endregion

        #region 图形色彩

        /// <summary>
        /// 【图色】对指定的数据地址和长度，组合成新的参数. FindPicMem FindPicMemE 以及FindPicMemEx专用
        /// </summary>
        /// <param name="picInfo">老的地址描述串</param>
        /// <param name="address">数据地址</param>
        /// <param name="size">数据长度</param>
        /// <returns>新的地址描述串</returns>
        public string AppendPicAddr(string picInfo, int address, int size)
        {
            return Marshal.PtrToStringUni(NativeMethods.AppendPicAddr(_dm, picInfo, address, size));
        }

        /// <summary>
        /// 【图色】开启图色调试模式，此模式会稍许降低图色和文字识别的速度.默认不开启
        /// </summary>
        /// <param name="enableDebug">是否开启，默认为不开启</param>
        /// <returns>0.失败，1.成功</returns>
        public int EnableDisplayDebug(bool enableDebug = false)
        {
            int debug = enableDebug ? 1 : 0;
            return NativeMethods.EnableDisplayDebug(_dm, debug);
        }

        /// <summary>
        /// 【图色】允许调用GetColor GetColorBGR GetColorHSV 以及 CmpColor时，以截图的方式来获取颜色
        /// </summary>
        /// <param name="enable">是否允许</param>
        /// <returns>0.失败，1.成功</returns>
        public int EnableGetColorByCapture(bool enable)
        {
            return NativeMethods.EnableGetColorByCapture(_dm, enable ? 1 : 0);
        }

        /// <summary>
        /// 【图色】抓取上次操作的图色区域，保存为file(24位位图)
        /// </summary>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录。当然这里也可以指定全路径名</param>
        /// <returns>0.失败，1.成功</returns>
        public int CapturePre(string file)
        {
            return NativeMethods.CapturePre(_dm, file);
        }

        /// <summary>
        /// 【图色】把BGR(按键格式)的颜色格式转换为RGB
        /// </summary>
        /// <param name="bgrColor">bgr格式的颜色字符串</param>
        /// <returns>RGB格式的字符串</returns>
        public string Bgr2Rgb(string bgrColor)
        {
            return Marshal.PtrToStringUni(NativeMethods.BGR2RGB(_dm, bgrColor));
        }

        /// <summary>
        /// 【图色】把RGB的颜色格式转换为BGR(按键格式)
        /// </summary>
        /// <param name="rgbColor">rgb格式的颜色字符串</param>
        /// <returns>BGR格式的字符串</returns>
        public string Rgb2Bgr(string rgbColor)
        {
            return Marshal.PtrToStringUni(NativeMethods.RGB2BGR(_dm, rgbColor));
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的图像,保存为file(24位位图)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <returns>0.失败，1.成功</returns>
        public int Capture(int x1, int y1, int x2, int y2, string file)
        {
            return NativeMethods.Capture(_dm, x1, y1, x2, y2, file);
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的动画，保存为gif格式
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <param name="delay">动画间隔，单位毫秒。 如果为0，表示只截取静态图片</param>
        /// <param name="mis">总共截取多久的动画，单位毫秒。</param>
        /// <returns>0.失败，1.成功</returns>
        public int CaptureGif(int x1, int y1, int x2, int y2, string file, int delay, int mis)
        {
            return NativeMethods.CaptureGif(_dm, x1, y1, x2, y2, file, delay, mis);
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的图像,保存为file(JPG压缩格式)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <param name="quality">jpg压缩比率(1-100) 越大图片质量越好</param>
        /// <returns>0.失败，1.成功</returns>
        public int CaptureJpg(int x1, int y1, int x2, int y2, string file, int quality)
        {
            return NativeMethods.CaptureJpg(_dm, x1, y1, x2, y2, file, quality);
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的图像,保存为file(PNG压缩格式)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <returns>0.失败，1.成功</returns>
        public int CapturePng(int x1, int y1, int x2, int y2, string file)
        {
            return NativeMethods.CapturePng(_dm, x1, y1, x2, y2, file);
        }

        /// <summary>
        /// 【图色】比较指定坐标点(x,y)的颜色
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="color">颜色字符串,可以支持偏色,多色,例如 "ffffff-202020|000000-000000" 这个表示白色偏色为202020,和黑色偏色为000000。
        /// 颜色最多支持10种颜色组合</param>
        /// <param name="sim">相似度(0.1-1.0)</param>
        /// <returns>0.失败，1.成功</returns>
        public int CmpColor(int x, int y, string color, double sim = 1.0)
        {
            return NativeMethods.CmpColor(_dm, x, y, color, sim);
        }

        /// <summary>
        /// 【图色】查找指定区域内的颜色,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <param name="intX">返回X坐标</param>
        /// <param name="intY">返回Y坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int FindColor(int x1, int y1, int x2, int y2, string color, double sim, int dir, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.FindColor(_dm, x1, y1, x2, y2, color, sim, dir, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定区域内的颜色,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反，如找不到，返回"-1|-1"
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回X和Y坐标 形式如"x|y", 比如"100|200"，如找不到，返回"-1|-1"</returns>
        public string FindColorE(int x1, int y1, int x2, int y2, string color, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindColorE(_dm, x1, y1, x2, y2, color, sim, dir));
        }

        /// <summary>
        /// 【图色】查找指定区域内的所有颜色,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回所有颜色信息的坐标值,然后通过GetResultCount等接口来解析</returns>
        public string FindColorEx(int x1, int y1, int x2, int y2, string color, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindColorEx(_dm, x1, y1, x2, y2, color, sim, dir));
        }

        /// <summary>
        /// 【图色】根据指定的多点查找颜色坐标
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="firstColor">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000"，
        /// 这里的含义和按键自带Color插件的意义相同，只不过我的可以支持偏色，
        /// 所有的偏移色坐标都相对于此颜色</param>
        /// <param name="offsetColor">偏移颜色 可以支持任意多个点 格式和按键自带的Color插件意义相同，
        /// 格式为"x1|y1|RRGGBB-DRDGDB,……xn|yn|RRGGBB-DRDGDB"，
        /// 比如"1|3|aabbcc,-5|-3|123456-000000"等任意组合都可以，支持偏色，
        /// 还可以支持反色模式，比如"1|3|-aabbcc,-5|-3|-123456-000000","-"表示除了指定颜色之外的颜色</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <param name="intX">返回X坐标(坐标为first_color所在坐标)</param>
        /// <param name="intY">返回Y坐标(坐标为first_color所在坐标)</param>
        /// <returns>0.失败，1.成功</returns>
        public int FindMultiColor(int x1,
            int y1,
            int x2,
            int y2,
            string firstColor,
            string offsetColor,
            double sim,
            int dir,
            out int intX,
            out int intY)
        {
            object x, y;
            int result = NativeMethods.FindMultiColor(_dm, x1, y1, x2, y2, firstColor, offsetColor, sim, dir, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】根据指定的多点查找颜色坐标
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="firstColor">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000"，
        /// 这里的含义和按键自带Color插件的意义相同，只不过我的可以支持偏色，
        /// 所有的偏移色坐标都相对于此颜色</param>
        /// <param name="offsetColor">偏移颜色 可以支持任意多个点 格式和按键自带的Color插件意义相同，
        /// 格式为"x1|y1|RRGGBB-DRDGDB,……xn|yn|RRGGBB-DRDGDB"，
        /// 比如"1|3|aabbcc,-5|-3|123456-000000"等任意组合都可以，支持偏色，
        /// 还可以支持反色模式，比如"1|3|-aabbcc,-5|-3|-123456-000000","-"表示除了指定颜色之外的颜色</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回X和Y坐标 形式如"x|y", 比如"100|200"</returns>
        public string FindMultiColorE(int x1, int y1, int x2, int y2, string firstColor, string offsetColor, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindMultiColorE(_dm, x1, y1, x2, y2, firstColor, offsetColor, sim, dir));
        }

        /// <summary>
        /// 【图色】根据指定的多点查找所有颜色坐标
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="firstColor">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000"，
        /// 这里的含义和按键自带Color插件的意义相同，只不过我的可以支持偏色，
        /// 所有的偏移色坐标都相对于此颜色</param>
        /// <param name="offsetColor">偏移颜色 可以支持任意多个点 格式和按键自带的Color插件意义相同，
        /// 格式为"x1|y1|RRGGBB-DRDGDB,……xn|yn|RRGGBB-DRDGDB"，
        /// 比如"1|3|aabbcc,-5|-3|123456-000000"等任意组合都可以，支持偏色，
        /// 还可以支持反色模式，比如"1|3|-aabbcc,-5|-3|-123456-000000","-"表示除了指定颜色之外的颜色</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回所有颜色信息的坐标值,然后通过GetResultCount等接口来解析，
        /// 坐标是first_color所在的坐标</returns>
        public string FindMultiColorEx(int x1, int y1, int x2, int y2, string firstColor, string offsetColor, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindMultiColorEx(_dm, x1, y1, x2, y2, firstColor, offsetColor, sim, dir));
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picName">图片名,可以是多个图片,比如"test.bmp|test2.bmp|test3.bmp"</param>
        /// <param name="deltaColor">颜色色偏 比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <param name="intX">返回图片左上角的X坐标</param>
        /// <param name="intY">返回图片左上角的Y坐标</param>
        /// <returns>返回找到的图片的序号,从0开始索引.如果没找到返回-1</returns>
        public int FindPic(int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir, out int intX, out int intY)
        {
            object x, y;
            int result = NativeMethods.FindPic(_dm, x1, y1, x2, y2, picName, deltaColor, sim, dir, out x, out y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// 这个函数可以查找多个图片,并且返回所有找到的图像的坐标
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picName">图片名,可以是多个图片,比如"test.bmp|test2.bmp|test3.bmp"</param>
        /// <param name="deltaColor">颜色色偏 比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回找到的图片序号(从0开始索引)以及X和Y坐标 形式如"index|x|y", 比如"3|100|200"</returns>
        public string FindPicE(int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindPicE(_dm, x1, y1, x2, y2, picName, deltaColor, sim, dir));
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理。
        /// 这个函数可以查找多个图片,并且返回所有找到的图像的坐标
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picName">图片名,可以是多个图片,比如"test.bmp|test2.bmp|test3.bmp"</param>
        /// <param name="deltaColor">颜色色偏 比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="dir">查找方向：
        /// 0.从左到右,从上到下 
        /// 1.从左到右,从下到上 
        /// 2.从右到左,从上到下 
        /// 3.从右到左,从下到上 
        /// 4.从中心往外查找
        /// 5.从上到下,从左到右 
        /// 6.从上到下,从右到左
        /// 7.从下到上,从左到右
        /// 8.从下到上,从右到左</param>
        /// <returns>返回的是所有找到的坐标格式如下:"id,x,y|id,x,y..|id,x,y" (图片左上角的坐标)
        /// 比如"0,100,20|2,30,40" 表示找到了两个,第一个,对应的图片是图像序号为0的图片,坐标是(100,20),第二个是序号为2的图片,坐标(30,40)
        /// (由于内存限制,返回的图片数量最多为1500个左右)</returns>
        public string FindPicEx(int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindPicEx(_dm, x1, y1, x2, y2, picName, deltaColor, sim, dir));
        }

        /// <summary>
        /// 【图色】释放指定的图片,此函数不必要调用,除非你想节省内存
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp|2.bmp|3.bmp" 等,可以使用通配符,比如
        /// "*.bmp" 这个对应了所有的bmp文件
        /// "a?c*.bmp" 这个代表了所有第一个字母是a 第三个字母是c 第二个字母任意的所有bmp文件
        /// "abc???.bmp|1.bmp|aa??.bmp" 可以这样任意组合</param>
        /// <returns>0.失败，1.成功</returns>
        public int FreePic(string picName)
        {
            return NativeMethods.FreePic(_dm, picName);
        }

        /// <summary>
        /// 【图色】获取范围(x1,y1,x2,y2)颜色的均值,返回格式"H.S.V"
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <returns>颜色字符串</returns>
        public string GetAveHsv(int x1, int y1, int x2, int y2)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetAveHSV(_dm, x1, y1, x2, y2));
        }

        /// <summary>
        /// 【图色】获取范围(x1,y1,x2,y2)颜色的均值,返回格式"RRGGBB"
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <returns>颜色字符串</returns>
        public string GetAveRgb(int x1, int y1, int x2, int y2)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetAveRGB(_dm, x1, y1, x2, y2));
        }

        /// <summary>
        /// 【图色】获取(x,y)的颜色,颜色返回格式"RRGGBB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColor(int x, int y)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetColor(_dm, x, y));
        }

        /// <summary>
        /// 【图色】获取(x,y)的颜色,颜色返回格式"BBGGRR"
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColorBgr(int x, int y)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetColorBGR(_dm, x, y));
        }

        /// <summary>
        /// 【图色】获取(x,y)的HSV颜色,颜色返回格式"H.S.V"
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColorHsv(int x, int y)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetColorHSV(_dm, x, y));
        }

        /// <summary>
        /// 【图色】获取指定区域的颜色数量,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020".注意，这里只支持RGB颜色</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>颜色数量</returns>
        public int GetColorNum(int x1, int y1, int x2, int y2, string color, double sim)
        {
            return NativeMethods.GetColorNum(_dm, x1, y1, x2, y2, color, sim);
        }

        /// <summary>
        /// 【图色】获取指定图片的尺寸，如果指定的图片已经被加入缓存，则从缓存中获取信息.此接口也会把此图片加入缓存
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp"</param>
        /// <returns>形式如 "w,h" 比如"30,20"</returns>
        public string GetPicSize(string picName)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetPicSize(_dm, picName));
        }

        /// <summary>
        /// 【图色】获取指定区域的图像,用二进制数据的方式返回,（不适合按键使用）方便二次开发
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <returns>返回的是指定区域的二进制颜色数据,每个颜色是4个字节,表示方式为(00RRGGBB)</returns>
        public int GetScreenData(int x1, int y1, int x2, int y2)
        {
            return NativeMethods.GetScreenData(_dm, x1, y1, x2, y2);
        }

        /// <summary>
        /// 【图色】转换图片格式为24位BMP格式
        /// </summary>
        /// <param name="picName">要转换的图片名</param>
        /// <param name="bmpName">要保存的BMP图片名</param>
        /// <returns>0.失败，1.成功</returns>
        public int ImageToBmp(string picName, string bmpName)
        {
            return NativeMethods.ImageToBmp(_dm, picName, bmpName);
        }

        /// <summary>
        /// 判断指定的区域，在指定的时间内(秒),图像数据是否一直不变.(卡屏).
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="mis">需要等待的时间,单位是秒</param>
        /// <returns>false.图像有变化，true.图像无变化</returns>
        public bool IsDisplayDead(int x1, int y1, int x2, int y2, int mis)
        {
            return NativeMethods.IsDisplayDead(_dm, x1, y1, x2, y2, mis) == 1;
        }

        /// <summary>
        /// 【图色】预先加载指定的图片,这样在操作任何和图片相关的函数时,将省去了加载图片的时间。
        /// 调用此函数后,没必要一定要调用FreePic,插件自己会自动释放。
        /// 另外,此函数不是必须调用的,所有和图形相关的函数只要调用过一次，图片会自动加入缓存.
        /// 如果想对一个已经加入缓存的图片进行修改，那么必须先用FreePic释放此图片在缓存中占用的内存，
        /// 然后重新调用图片相关接口，就可以重新加载此图片
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp|2.bmp|3.bmp" 等,可以使用通配符,比如
        /// "*.bmp" 这个对应了所有的bmp文件
        /// "a?c*.bmp" 这个代表了所有第一个字母是a 第三个字母是c 第二个字母任意的所有bmp文件
        /// "abc???.bmp|1.bmp|aa??.bmp" 可以这样任意组合</param>
        /// <returns>0.失败，1.成功</returns>
        public int LoadPic(string picName)
        {
            return NativeMethods.LoadPic(_dm, picName);
        }

        /// <summary>
        /// 【图色】根据通配符获取文件集合. 方便用于FindPic和FindPicEx
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp|2.bmp|3.bmp" 等,可以使用通配符,比如
        /// "*.bmp" 这个对应了所有的bmp文件
        /// "a?c*.bmp" 这个代表了所有第一个字母是a 第三个字母是c 第二个字母任意的所有bmp文件
        /// "abc???.bmp|1.bmp|aa??.bmp" 可以这样任意组合</param>
        /// <returns>返回的是通配符对应的文件集合，每个图片以|分割</returns>
        public string MatchPicName(string picName)
        {
            return Marshal.PtrToStringUni(NativeMethods.MatchPicName(_dm, picName));
        }

        /// <summary>
        /// 【图色】设置图片密码，如果图片本身没有加密，那么此设置不影响不加密的图片，一样正常使用。
        /// </summary>
        /// <param name="pwd">图片密码</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetPicPwd(string pwd)
        {
            return NativeMethods.SetPicPwd(_dm, pwd);
        }

        #endregion

        #region 内存

        /// <summary>
        /// 【内存】把双精度浮点数转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的双精度浮点数</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string DoubleToData(double value)
        {
            return Marshal.PtrToStringUni(NativeMethods.DoubleToData(_dm, value));
        }

        /// <summary>
        /// 【内存】把单精度浮点数转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的单精度浮点数</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string FloatToData(float value)
        {
            return Marshal.PtrToStringUni(NativeMethods.FloatToData(_dm, value));
        }

        /// <summary>
        /// 【内存】把整数转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的整数</param>
        /// <param name="type">0. 4字节整形数（一般都选这个），1. 2字节整形数，2. 1字节整形数</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string IntToData(int value, int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.IntToData(_dm, value, type));
        }

        /// <summary>
        /// 【内存】把字符串转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的字符串</param>
        /// <param name="type">0.返回Ascii表达的字符串，1.返回Unicode表达的字符串</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string StringToData(string value, int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.StringToData(_dm, value, type));
        }

        /// <summary>
        /// 【内存】搜索指定的二进制数据
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="data">要搜索的二进制数据 以字符串的形式描述 比如"00 01 23 45 67 86 ab ce f1"等</param>
        /// <returns>返回搜索到的地址集合，地址格式如下:
        /// "addr1|addr2|addr3…|addrn"
        /// 比如"400050|423435|453430"</returns>
        public string FindData(int hwnd, string addrRange, string data)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindData(_dm, hwnd, addrRange, data));
        }

        /// <summary>
        /// 【内存】搜索指定范围[<paramref name="minValue"/>,<paramref name="maxValue"/>]的双精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="minValue">搜索的双精度数值最小值</param>
        /// <param name="maxValue">搜索的双精度数值最大值</param>
        /// <returns>返回搜索到的地址集合，地址格式如下:
        /// "addr1|addr2|addr3…|addrn"
        /// 比如"400050|423435|453430"</returns>
        public string FindDouble(int hwnd, string addrRange, double minValue, double maxValue)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindDouble(_dm, hwnd, addrRange, minValue, maxValue));
        }

        /// <summary>
        /// 【内存】搜索指定范围[<paramref name="minValue"/>,<paramref name="maxValue"/>]的单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="minValue">搜索的单精度数值最小值</param>
        /// <param name="maxValue">搜索的单精度数值最大值</param>
        /// <returns>返回搜索到的地址集合，地址格式如下:
        /// "addr1|addr2|addr3…|addrn"
        /// 比如"400050|423435|453430"</returns>
        public string FindFloat(int hwnd, string addrRange, float minValue, float maxValue)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindFloat(_dm, hwnd, addrRange, minValue, maxValue));
        }

        /// <summary>
        /// 【内存】搜索指定范围[<paramref name="minValue"/>,<paramref name="maxValue"/>]的整数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="minValue">搜索的整数数值最小值</param>
        /// <param name="maxValue">搜索的整数数值最大值</param>
        /// <param name="type">搜索的整数类型,取值如下：
        /// 0. 32位，1. 16位，2. 8位</param>
        /// <returns>返回搜索到的地址集合，地址格式如下:
        /// "addr1|addr2|addr3…|addrn"
        /// 比如"400050|423435|453430"</returns>
        public string FindInt(int hwnd, string addrRange, int minValue, int maxValue, int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindInt(_dm, hwnd, addrRange, minValue, maxValue, type));
        }

        /// <summary>
        /// 【内存】搜索指定的字符串
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="value">搜索的字符串</param>
        /// <param name="type">搜索的字符串类型，取值如下：0.Ascii字符串，1.Unicode字符串</param>
        /// <returns>返回搜索到的地址集合，地址格式如下:
        /// "addr1|addr2|addr3…|addrn"
        /// 比如"400050|423435|453430"</returns>
        public string FindString(int hwnd, string addrRange, string value, int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindString(_dm, hwnd, addrRange, value, type));
        }

        /// <summary>
        /// 【内存】根据指定的窗口句柄，来获取对应窗口句柄进程下的指定模块的基址
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="moduleName">模块名</param>
        /// <returns>模块的基址</returns>
        public int GetModuleBaseAddr(int hwnd, string moduleName)
        {
            return NativeMethods.GetModuleBaseAddr(_dm, hwnd, moduleName);
        }

        /// <summary>
        /// 【内存】读取指定地址的二进制数据
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="length">二进制数据的长度</param>
        /// <returns>读取到的数值,以16进制表示的字符串 每个字节以空格相隔 比如"12 34 56 78 ab cd ef"</returns>
        public string ReadData(int hwnd, string address, int length)
        {
            return Marshal.PtrToStringUni(NativeMethods.ReadData(_dm, hwnd, address, length));
        }

        /// <summary>
        /// 【内存】读取指定地址的双精度浮点数
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public double ReadDouble(int hwnd, string address)
        {
            return NativeMethods.ReadDouble(_dm, hwnd, address);
        }

        /// <summary>
        /// 【内存】读取指定地址的单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public float ReadFloat(int hwnd, string address)
        {
            return NativeMethods.ReadFloat(_dm, hwnd, address);
        }

        /// <summary>
        /// 【内存】读取指定地址的整数数值，类型可以是8位，16位 或者 32位
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="type">搜索的整数类型,取值如下：
        /// 0. 32位，1. 16位，2. 8位</param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public int ReadInt(int hwnd, string address, int type)
        {
            return NativeMethods.ReadInt(_dm, hwnd, address, type);
        }

        /// <summary>
        /// 【内存】读取指定地址的字符串，可以是GBK字符串或者是Unicode字符串.(必须事先知道内存区的字符串编码方式)
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="type">搜索的字符串类型，取值如下：0.GBK字符串，1.Unicode字符串</param>
        /// <param name="length">需要读取的字节数目</param>
        /// <returns>读取到的字符串,注意这里无法判断读取是否成功</returns>
        public string ReadString(int hwnd, string address, int type, int length)
        {
            return Marshal.PtrToStringUni(NativeMethods.ReadString(_dm, hwnd, address, type, length));
        }

        /// <summary>
        /// 【内存】对指定地址写入二进制数据
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="data">二进制数据，以字符串形式描述，比如"12 34 56 78 90 ab cd"</param>
        /// <returns>0.失败，1.成功</returns>
        public long WriteData(int hwnd, string address, string data)
        {
            return NativeMethods.WriteData(_dm, hwnd, address, data);
        }

        /// <summary>
        /// 【内存】对指定地址写入双精度浮点数
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="value">双精度浮点数</param>
        /// <returns>0.失败，1.成功</returns>
        public int WriteDouble(int hwnd, string address, double value)
        {
            return NativeMethods.WriteDouble(_dm, hwnd, address, value);
        }

        /// <summary>
        /// 【内存】对指定地址写入单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="value">单精度浮点数</param>
        /// <returns>0.失败，1.成功</returns>
        public int WriteFloat(int hwnd, string address, float value)
        {
            return NativeMethods.WriteFloat(_dm, hwnd, address, value);
        }

        /// <summary>
        /// 【内存】对指定地址写入整数数值，类型可以是8位，16位 或者 32位
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="type">搜索的整数类型,取值如下：
        /// 0. 32位，1. 16位，2. 8位</param>
        /// <param name="value">整型数值</param>
        /// <returns>0.失败，1.成功</returns>
        public int WriteInt(int hwnd, string address, int type, int value)
        {
            return NativeMethods.WriteInt(_dm, hwnd, address, type, value);
        }

        /// <summary>
        /// 【内存】对指定地址写入字符串，可以是Ascii字符串或者是Unicode字符串
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="address">用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个/// 地址。+表示地址加，-表示地址减。
        /// 模块名必须用&lt;&gt;符号来圈起来，例如：
        /// 1. "4DA678" 最简单的方式，用绝对数值来表示地址
        /// 2. "&lt;360SE.exe&gt;+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// 3. "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// 4. "[&lt;360SE.exe&gt;+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// 5. "[[[&lt;360SE.exe&gt;+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// 总之熟悉CE的人 应该对这个地址描述都很熟悉,我就不多举例了</param>
        /// <param name="type">搜索的字符串类型，取值如下：0.Ascii字符串，1.Unicode字符串</param>
        /// <param name="value">字符串值</param>
        /// <returns>0.失败，1.成功</returns>
        public int WriteString(int hwnd, string address, int type, string value)
        {
            return NativeMethods.WriteString(_dm, hwnd, address, type, value);
        }

        #endregion

        #region 系统

        /// <summary>
        /// 【系统】蜂鸣器
        /// </summary>
        /// <param name="fre">频率</param>
        /// <param name="delay">时长</param>
        /// <returns>0.失败，1.成功</returns>
        public int Beep(int fre, int delay)
        {
            return NativeMethods.Beep(_dm, fre, delay);
        }

        /// <summary>
        /// 【系统】检测当前系统是否有开启UAC(用户账户控制).
        /// </summary>
        /// <returns>是否开启</returns>
        public bool CheckUAC()
        {
            return NativeMethods.CheckUAC(_dm) == 1;
        }

        /// <summary>
        /// 设置当前系统的UAC(用户账户控制).
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetUAC(bool enable)
        {
            return NativeMethods.SetUAC(_dm, enable ? 1 : 0);
        }

        /// <summary>
        /// 【系统】设置指定毫秒数的延时
        /// </summary>
        /// <param name="mis">延时毫秒数</param>
        public void Delay(int mis)
        {
            Thread.Sleep(mis);
        }

        /// <summary>
        /// 【系统】关闭电源管理，不会进入睡眠
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int DisablePowerSave()
        {
            return NativeMethods.DisablePowerSave(_dm);
        }

        /// <summary>
        /// 【系统】关闭屏幕保护
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int DisableScreenSave()
        {
            return NativeMethods.DisablePowerSave(_dm);
        }

        /// <summary>
        /// 【系统】退出系统(注销 重启 关机)
        /// </summary>
        /// <param name="type">0.注销系统，1.关机，2.重新启动</param>
        /// <returns>0.失败，1.成功</returns>
        public int ExitOs(int type)
        {
            return NativeMethods.ExitOs(_dm, type);
        }

        /// <summary>
        /// 【系统】获取剪贴板的内容
        /// </summary>
        /// <returns>以字符串表示的剪贴板内容</returns>
        public string GetClipboard()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetClipboard(_dm));
        }

        /// <summary>
        /// 【系统】获取剪贴板的内容
        /// </summary>
        /// <param name="value">以字符串表示的剪贴板内容</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetClipboard(string value)
        {
            return NativeMethods.SetClipboard(_dm, value);
        }

        /// <summary>
        /// 【系统】得到特定类型的路径
        /// </summary>
        /// <param name="type">取值为以下类型：
        /// 0.获取当前路径
        /// 1.获取系统路径(system32路径)
        /// 2.获取windows路径(windows所在路径)
        /// 3.获取临时目录路径(temp)
        /// 4.获取当前进程(exe)所在的路径</param>
        public string GetDir(int type)
        {
            return Marshal.PtrToStringUni(NativeMethods.GetDir(_dm, type));
        }

        /// <summary>
        /// 【系统】获取本机的硬盘序列号.支持ide scsi硬盘. 要求调用进程必须有管理员权限. 否则返回空串
        /// </summary>
        /// <returns></returns>
        public string GetDiskSerial()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetDiskSerial(_dm));
        }

        /// <summary>
        /// 【系统】获取本机的机器码.(不带网卡) 要求调用进程必须有管理员权限. 否则返回空串
        /// </summary>
        /// <returns>字符串表达的机器机器码</returns>
        public string GetMachineCodeNoMac()
        {
            return Marshal.PtrToStringUni(NativeMethods.GetMachineCodeNoMac(_dm));
        }

        /// <summary>
        /// 【系统】从网络获取当前北京时间，如获取失败返回DateTime.MinValue
        /// </summary>
        /// <returns>网络时间，如获取失败返回DateTime.MinValue</returns>
        public DateTime GetNetTime()
        {
            string result = Marshal.PtrToStringUni(NativeMethods.GetNetTime(_dm));
            if (string.IsNullOrEmpty(result) || result == "0000-00-00 00:00:00")
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(result);
        }

        /// <summary>
        /// 【系统】判断当前系统是否是64位操作系统
        /// </summary>
        /// <returns>是否是64位系统</returns>
        public bool Is64Bit()
        {
            return NativeMethods.Is64Bit(_dm) == 1;
        }

        /// <summary>
        /// 【系统】得到操作系统的类型
        /// </summary>
        /// <returns>0 : win95/98/me/nt4.0，1 : xp/2000，2 : 2003，3 : win7/vista/2008</returns>
        public int GetOSType()
        {
            return NativeMethods.GetOsType(_dm);
        }

        /// <summary>
        /// 【系统】获取屏幕的色深
        /// </summary>
        /// <returns>返回系统颜色深度.(16或者32等)</returns>
        public int GetScreenDepth()
        {
            return NativeMethods.GetScreenDepth(_dm);
        }

        /// <summary>
        /// 【系统】获取屏幕的高度
        /// </summary>
        /// <returns>返回屏幕的高度</returns>
        public int GetScreenHeight()
        {
            return NativeMethods.GetScreenHeight(_dm);
        }

        /// <summary>
        /// 【系统】获取屏幕的宽度
        /// </summary>
        /// <returns>返回屏幕的宽度</returns>
        public int GetScreenWidth()
        {
            return NativeMethods.GetScreenWidth(_dm);
        }

        /// <summary>
        /// 【系统】获取当前系统从开机到现在所经历过的时间，单位是毫秒
        /// </summary>
        /// <returns></returns>
        public long GetTimeSpanFromOsStarted()
        {
            return NativeMethods.GetTime(_dm);
        }

        /// <summary>
        /// 【系统】播放指定的MP3或者wav文件
        /// </summary>
        /// <param name="file">指定的音乐文件，可以采用文件名或者绝对路径的形式</param>
        /// <returns>0 : 失败，非0表示当前播放的ID。可以用Stop来控制播放结束</returns>
        public int MediaPlay(string file)
        {
            return NativeMethods.Play(_dm, file);
        }

        /// <summary>
        /// 【系统】停止指定的音乐
        /// </summary>
        /// <param name="id">Play返回的播放id</param>
        /// <returns>0.失败，1.成功</returns>
        public int MediaStop(int id)
        {
            return NativeMethods.Stop(_dm, id);
        }

        /// <summary>
        /// 【系统】设置系统的分辨率 系统色深
        /// </summary>
        /// <param name="width">屏幕宽度</param>
        /// <param name="height">屏幕高度</param>
        /// <param name="depth">系统色深</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetScreen(int width, int height, int depth)
        {
            return NativeMethods.SetScreen(_dm, width, height, depth);
        }

        #endregion

        #region 后台

        /// <summary>
        /// 【后台】绑定指定的窗口,并指定这个窗口的屏幕颜色获取方式,鼠标仿真模式,键盘仿真模式,以及模式设定,高级用户可以参考BindWindowEx更加灵活强大
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="display">窗口绑定屏幕颜色获取方式</param>
        /// <param name="mouse">窗口绑定鼠标仿真模式</param>
        /// <param name="keypad">窗口绑定键盘仿真模式</param>
        /// <param name="mode">窗口绑定模式</param>
        /// <returns>0.失败，1.成功</returns>
        public int BindWindow(int hwnd, DmBindDisplay display, DmBindMouse mouse, DmBindKeypad keypad, DmBindMode mode)
        {
            return NativeMethods.BindWindow(_dm, hwnd, display.ToString(), mouse.ToString(), keypad.ToString(), (int)mode);
        }

        /// <summary>
        /// 【后台】绑定指定的窗口,并指定这个窗口的屏幕颜色获取方式,鼠标仿真模式,键盘仿真模式 高级用户使用
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="display">屏幕颜色获取方式 取值有以下几种：
        /// "normal" : 正常模式,平常我们用的前台截屏模式
        /// "gdi" : gdi模式,用于窗口采用GDI方式刷新时. 此模式占用CPU较大.
        /// "gdi2" : gdi2模式,此模式兼容性较强,但是速度比gdi模式要慢许多,如果gdi模式发现后台不刷新时,可以考虑用gdi2模式.
        /// "dx2" : dx2模式,用于窗口采用dx模式刷新,如果dx方式会出现窗口进程崩溃的状况,可以考虑采用这种.采用这种方式要保证窗口有一部分在屏幕外.win7或者vista不需要移动也可后台. 此模式占用CPU较大.
        /// "dx3" : dx3模式,同dx2模式,但是如果发现有些窗口后台不刷新时,可以考虑用dx3模式,此模式比dx2模式慢许多. 此模式占用CPU较大.
        /// dx模式,用于窗口采用dx模式刷新,取值可以是以下任意组合，组合采用"|"符号进行连接  注意此模式需要管理员权限
        /// 1. "dx.graphic.2d"  2d窗口的dx图色模式  此模式比较耗费资源，慎用.
        /// 2. "dx.graphic.3d"  3d窗口的dx图色模式,注意采用这个模式，必须关闭窗口3D视频设置的全屏抗锯齿选项. 此模式比较耗费资源，慎用.</param>
        /// <param name="mouse">鼠标仿真模式 取值有以下几种：
        /// "normal" : 正常模式,平常我们用的前台鼠标模式
        /// "windows": Windows模式,采取模拟windows消息方式 同按键的后台插件.
        /// "windows3": Windows3模式，采取模拟windows消息方式,可以支持有多个子窗口的窗口后台
        /// dx模式,取值可以是以下任意组合. 组合采用"|"符号进行连接 注意此模式需要管理员权限
        /// 1. "dx.mouse.position.lock.api"  此模式表示通过封锁系统API，来锁定鼠标位置.
        /// 2. "dx.mouse.position.lock.message" 此模式表示通过封锁系统消息，来锁定鼠标位置.
        /// 3. "dx.mouse.focus.input.api" 此模式表示通过封锁系统API来锁定鼠标输入焦点.
        /// 4. "dx.mouse.focus.input.message"此模式表示通过封锁系统消息来锁定鼠标输入焦点.
        /// 5. "dx.mouse.clip.lock.api" 此模式表示通过封锁系统API来锁定刷新区域。注意，使用这个模式，在绑定前，必须要让窗口完全显示出来.
        /// 6. "dx.mouse.input.lock.api" 此模式表示通过封锁系统API来锁定鼠标输入接口.
        /// 7. "dx.mouse.state.api" 此模式表示通过封锁系统API来锁定鼠标输入状态.
        /// 8. "dx.mouse.state.message" 此模式表示通过封锁系统消息来锁定鼠标输入状态.
        /// 9. "dx.mouse.api"  此模式表示通过封锁系统API来模拟dx鼠标输入.
        /// 10. "dx.mouse.cursor"  开启此模式，可以后台获取鼠标特征码. (此模式仅支持按键和简单游平台)</param>
        /// <param name="keypad">键盘仿真模式 取值有以下几种：
        /// "normal" : 正常模式,平常我们用的前台键盘模式
        /// "windows": Windows模式,采取模拟windows消息方式 同按键的后台插件.
        /// dx模式,取值可以是以下任意组合. 组合采用"|"符号进行连接 注意此模式需要管理员权限
        /// 1. "dx.keypad.input.lock.api" 此模式表示通过封锁系统API来锁定键盘输入接口.
        /// 2. "dx.keypad.state.api" 此模式表示通过封锁系统API来锁定键盘输入状态.
        /// 3. "dx.keypad.api" 此模式表示通过封锁系统API来模拟dx键盘输入.</param>
        /// <param name="public">公共属性 dx模式共有  注意以下列表中,前面打五角星的表示需要管理员权限。
        /// 取值可以是以下任意组合. 组合采用"|"符号进行连接 这个值可以为空
        /// 1. ★ "dx.public.active.api" 此模式表示通过封锁系统API来锁定窗口激活状态.  注意，部分窗口在此模式下会耗费大/// 量资源 慎用. 
        /// 2. ★ "dx.public.active.message" 此模式表示通过封锁系统消息来锁定窗口激活状态.  注意，部分窗口在此模式下会耗费大量资源慎用. 另外如果要让此模式生效，必须在绑定前，让绑定窗口处于激活状态(或者绑定以后再激活),否则此模式将失效.
        /// 3.    "dx.public.disable.window.position" 此模式将锁定绑定窗口位置.不可与"dx.public.fake.window.min"共用.
        /// 4.    "dx.public.disable.window.size" 此模式将锁定绑定窗口,禁止改变大小. 不可与"dx.public.fake.window.min"共用.
        /// 5.    "dx.public.disable.window.minmax" 此模式将禁止窗口最大化和最小化,但是付出的代价是窗口同时也会被置顶. 不可与"dx.public.fake.window.min"共用.
        /// 6.    "dx.public.fake.window.min" 此模式将允许目标窗口在最小化状态时，仍然能够像非最小化一样操作.此模式仅支持简单游与按键，小精灵等其它一律不支持. 另注意，此模式会导致任务栏顺序重排，所以如果是多开模式下，会看起来比较混乱，建议单开使用，多开不建议使用.</param>
        /// <param name="mode">窗口绑定模式</param>
        /// <returns>0.失败，1.成功</returns>
        public int BindWindowEx(int hwnd, string display, string mouse, string keypad, string @public, DmBindMode mode)
        {
            return NativeMethods.BindWindowEx(_dm, hwnd, display, mouse, keypad, @public, (int)mode);
        }

        /// <summary>
        /// 【后台】降低目标窗口所在进程的CPU占用。
        /// 注意: 此接口必须在绑定窗口成功以后调用，而且必须保证目标窗口可以支持dx.graphic.3d方式截图，否则降低CPU无效。
        /// 因为降低CPU是通过降低窗口刷新速度来实现，所以注意，开启此功能以后会导致窗口刷新速度变慢
        /// </summary>
        /// <param name="rate">取值范围0到100   取值为0 表示关闭CPU优化. 这个值越大表示降低CPU效果越好</param>
        /// <returns>0.失败，1.成功</returns>
        public int DownCpu(int rate)
        {
            return NativeMethods.DownCpu(_dm, rate);
        }

        /// <summary>
        /// 【后台】设置是否允许绑定窗口所在进程的输入法，此方法需在绑定之后调用
        /// </summary>
        /// <param name="enable">是否允许</param>
        /// <returns>0.失败，1.成功</returns>
        public int EnableIme(bool enable)
        {
            return NativeMethods.EnableIme(_dm, enable ? 1 : 0);
        }

        /// <summary>
        /// 【后台】设置是否开启高速dx键鼠模式。 默认是关闭
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <returns>0.失败，1.成功</returns>
        public int EnableSpeedDx(bool enable = false)
        {
            return NativeMethods.EnableSpeedDx(_dm, enable ? 1 : 0);
        }

        /// <summary>
        /// 【后台】禁止外部输入到指定窗口。
        /// 此接口只针对dx键鼠，普通鼠标无效，可用于绑定为dx2鼠标时，需要锁定输入
        /// </summary>
        /// <param name="lock">0.关闭锁定，1.开启锁定（键鼠均锁），2.只锁定鼠标，3.只锁定键盘</param>
        /// <returns>0.失败，1.成功</returns>
        public int LockInput(int @lock)
        {
            return NativeMethods.LockInput(_dm, @lock);
        }

        /// <summary>
        /// 【后台】设置前台鼠标在屏幕上的活动范围
        /// </summary>
        /// <param name="x1">区域的左上X坐标. 屏幕坐标</param>
        /// <param name="y1">区域的左上Y坐标. 屏幕坐标</param>
        /// <param name="x2">区域的右下X坐标. 屏幕坐标</param>
        /// <param name="y2">区域的右下Y坐标. 屏幕坐标</param>
        /// <returns>0.失败，1.成功</returns>
        public int LockMouseRect(int x1, int y1, int x2, int y2)
        {
            return NativeMethods.LockMouseRect(_dm, x1, y1, x2, y2);
        }

        /// <summary>
        /// 【后台】设置dx截图最长等待时间。内部默认是3000毫秒. 一般用不到调整这个
        /// 注: 此接口仅对图色为dx.graphic.3d   dx.graphic.3d.8  dx.graphic.2d   dx.graphic.2d.2有效. 其他图色模式无效.
        /// 默认情况下，截图需要等待一个延时，超时就认为截图失败. 这个接口可以调整这个延时. 
        /// 某些时候或许有用.比如当窗口图色卡死(这时获取图色一定都是超时)，并且要判断窗口卡死，那么这个设置就很有用了。
        /// </summary>
        /// <param name="mis">等待时间，单位是毫秒。 注意这里不能设置的过小，否则可能会导致截图失败,从而导致图色函数和文字识别失败</param>
        /// <returns>0.失败，1.成功</returns>
        public int SetDisplayDelay(int mis)
        {
            return NativeMethods.SetDisplayDelay(_dm, mis);
        }

        /// <summary>
        /// 【后台】窗口解除绑定
        /// </summary>
        /// <returns></returns>
        public int UnBindWindow()
        {
            return NativeMethods.UnBindWindow(_dm);
        }

        #endregion

        #region 算法

        /// <summary>
        /// 【算法】根据部分Ex接口的返回值，排除指定范围区域内的坐标
        /// </summary>
        /// <param name="allPos">坐标描述串。  一般是FindStrEx,FindStrFastEx,FindStrWithFontEx, FindColorEx, FindMultiColorEx,和FindPicEx的返回值</param>
        /// <param name="type">取值为0或者1。
        /// 如果all_pos的内容是由FindPicEx,FindStrEx,FindStrFastEx,FindStrWithFontEx返回，那么取值为0
        /// 如果all_pos的内容是由FindColorEx, FindMultiColorEx返回，那么取值为1</param>
        /// <param name="x1">左上角横坐标</param>
        /// <param name="y1">左上角纵坐标</param>
        /// <param name="x2">右下角横坐标</param>
        /// <param name="y2">右下角纵坐标</param>
        /// <returns>经过筛选以后的返回值，格式和type指定的一致</returns>
        public string ExcludePos(string allPos, int type, int x1, int y1, int x2, int y2)
        {
            return Marshal.PtrToStringUni(NativeMethods.ExcludePos(_dm, allPos, type, x1, y1, x2, y2));
        }

        /// <summary>
        /// 【算法】根据部分Ex接口的返回值，然后在所有坐标里找出距离指定坐标最近的那个坐标
        /// </summary>
        /// <param name="allPos">坐标描述串。  一般是FindStrEx,FindStrFastEx,FindStrWithFontEx, FindColorEx, FindMultiColorEx,和FindPicEx的返回值.</param>
        /// <param name="type">取值为0或者1。
        /// 如果all_pos的内容是由FindPicEx,FindStrEx,FindStrFastEx,FindStrWithFontEx返回，那么取值为0
        /// 如果all_pos的内容是由FindColorEx, FindMultiColorEx返回，那么取值为1</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>返回的格式和type有关，如果type为0，那么返回的格式是"id,x,y"
        /// 如果type为1,那么返回的格式是"x,y".
        ///</returns>
        public string FindNearestPos(string allPos, int type, int x, int y)
        {
            return Marshal.PtrToStringUni(NativeMethods.FindNearestPos(_dm, allPos, type, x, y));
        }

        /// <summary>
        /// 【算法】根据部分Ex接口的返回值，然后对所有坐标根据对指定坐标的距离进行从小到大的排序
        /// </summary>
        /// <param name="allPos">坐标描述串。  一般是FindStrEx,FindStrFastEx,FindStrWithFontEx, FindColorEx, FindMultiColorEx,和FindPicEx的返回值.</param>
        /// <param name="type">取值为0或者1。
        /// 如果all_pos的内容是由FindPicEx,FindStrEx,FindStrFastEx,FindStrWithFontEx返回，那么取值为0
        /// 如果all_pos的内容是由FindColorEx, FindMultiColorEx返回，那么取值为1</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>返回的格式和type指定的格式一致</returns>
        public string SortPosDistance(string allPos, int type, int x, int y)
        {
            return Marshal.PtrToStringUni(NativeMethods.SortPosDistance(_dm, allPos, type, x, y));
        }

        #endregion

        #region 汇编

        /// <summary>
        /// 【汇编】添加指定的MASM汇编指令
        /// </summary>
        /// <param name="asmIns">MASM汇编指令,大小写均可以  比如 mov eax,1</param>
        /// <returns>0.失败，1.成功</returns>
        public int AsmAdd(string asmIns)
        {
            return NativeMethods.AsmAdd(_dm, asmIns);
        }

        /// <summary>
        /// 【汇编】执行用AsmAdd加到缓冲中的指令
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="mode">模式：0.在本进程中进行执行，这时hwnd无效，1.对hwnd指定的进程内执行,注入模式为创建远程线程</param>
        /// <returns>0.失败，1.成功</returns>
        public int AsmCall(int hwnd, int mode)
        {
            return NativeMethods.AsmCall(_dm, hwnd, mode);
        }

        /// <summary>
        /// 【汇编】清除汇编指令缓冲区 用AsmAdd添加到缓冲的指令全部清除
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int AsmClear()
        {
            return NativeMethods.AsmClear(_dm);
        }

        /// <summary>
        /// 【汇编】把汇编缓冲区的指令转换为机器码 并用16进制字符串的形式输出
        /// </summary>
        /// <param name="baseAddress">用AsmAdd添加到缓冲区的第一条指令所在的地址</param>
        /// <returns>机器码，比如 "aa bb cc"这样的形式</returns>
        public string AsmCode(int baseAddress)
        {
            return Marshal.PtrToStringUni(NativeMethods.AsmCode(_dm, baseAddress));
        }

        /// <summary>
        /// 【汇编】把指定的机器码转换为汇编语言输出
        /// </summary>
        /// <param name="asmCode">机器码，形式如 "aa bb cc"这样的16进制表示的字符串(空格无所谓)</param>
        /// <param name="baseAddress">指令所在的地址</param>
        /// <param name="isUpper">表示转换的汇编语言是否以大写输出</param>
        /// <returns>MASM汇编语言字符串</returns>
        public string Assemble(string asmCode, int baseAddress, bool isUpper)
        {
            int upper = isUpper ? 1 : 0;
            return Marshal.PtrToStringUni(NativeMethods.Assemble(_dm, asmCode, baseAddress, upper));
        }

        #endregion

        #region 答题

        /// <summary>
        /// 【答题】可以把上次FaqPost的发送取消,接着下一次FaqPost
        /// </summary>
        /// <returns>0.失败，1.成功</returns>
        public int FaqCancel()
        {
            return NativeMethods.FaqCancel(_dm);
        }

        /// <summary>
        /// 【答题】截取指定范围内的动画或图像，并返回此句柄
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="quality">图像或动画品质,或者叫压缩率,此值越大图像质量越好 取值范围（1-100）</param>
        /// <param name="delay">截取动画时用,表示相隔两帧间的时间间隔,单位毫秒 （如果只是截取图像,这个参数必须是0）</param>
        /// <param name="mis">表示总共截取多久的动画,单位毫秒 （如果只是截取图像,这个参数必须是0）</param>
        /// <returns>图像或者动画句柄</returns>
        public int FaqCapture(int x1, int y1, int x2, int y2, int quality, int delay, int mis)
        {
            return NativeMethods.FaqCapture(_dm, x1, y1, x2, y2, quality, delay, mis);
        }

        /// <summary>
        /// 【答题】获取由FaqPost发送后，由服务器返回的答案
        /// </summary>
        /// <returns>根据FaqPost中 request_type取值的不同,返回值不同
        /// 如果此函数调用失败,那么返回值如下
        /// "Error:错误描述" 
        /// 如果函数调用成功,那么返回值如下
        /// "OK:答案"
        /// 当request_type 为0时,答案的格式为"x,y" (不包含引号)
        /// 当request_type 为1时,答案的格式为"1" "2" "3" "4" "5" "6" (不包含引号)
        /// 当request_type 为2时,答案就是要求的答案比如 "李白" (不包含引号)
        /// 如果返回为空字符串，表示FaqPost还未处理完毕,或者没有调用过FaqPost.</returns>
        public string FaqFetch()
        {
            return Marshal.PtrToStringUni(NativeMethods.FaqFetch(_dm));
        }

        /// <summary>
        /// 【答题】获取句柄所对应的数据包的大小,单位是字节
        /// </summary>
        /// <param name="handle">由FaqCapture返回的句柄</param>
        /// <returns>数据包大小,一般用于判断数据大小,选择合适的压缩比率.</returns>
        public int FaqGetSize(int handle)
        {
            return NativeMethods.FaqGetSize(_dm, handle);
        }

        /// <summary>
        /// 【答题】发送指定的图像句柄到指定的服务器,并立即返回(异步操作).
        /// </summary>
        /// <param name="server">服务器地址以及端口,格式为(ip:port),例如 "192.168.1.100:12345"</param>
        /// <param name="handle">由FaqCapture获取到的句柄</param>
        /// <param name="requestType">0.要求获取坐标，1.要求获取选项,比如(ABCDE)，2.要求获取文字答案</param>
        /// <param name="timeout">表示等待多久,单位是毫秒</param>
        /// <returns>0.失败，一般情况下是由于上个FaqPost还没有处理完毕(服务器还没返回)，1.成功</returns>
        public int FaqPost(string server, int handle, int requestType, int timeout)
        {
            return NativeMethods.FaqPost(_dm, server, handle, requestType, timeout);
        }

        /// <summary>
        /// 【答题】发送指定的图像句柄到指定的服务器,并等待返回结果(同步等待).
        /// </summary>
        /// <param name="server">服务器地址以及端口,格式为(ip:port),例如 "192.168.1.100:12345"
        /// 多个地址可以用"|"符号连接。比如"192.168.1.100:12345|192.168.1.101:12345"。</param>
        /// <param name="handle">由FaqCapture获取到的句柄</param>
        /// <param name="requestType">0.要求获取坐标，1.要求获取选项,比如(ABCDE)，2.要求获取文字答案</param>
        /// <param name="timeout">表示等待多久,单位是毫秒</param>
        /// <returns>根据request_type取值的不同,返回值不同。
        /// 如果此函数调用失败,那么返回值如下：
        /// "Error:错误描述" 。
        /// 如果函数调用成功,那么返回值如下：
        /// "OK:答案"。
        /// 当request_type 为0时,答案的格式为"x,y" (不包含引号)
        /// 当request_type 为1时,答案的格式为"1" "2" "3" "4" "5" "6" (不包含引号)
        /// 当request_type 为2时,答案就是要求的答案比如 "李白" (不包含引号)</returns>
        public string FaqSend(string server, int handle, int requestType, int timeout)
        {
            return Marshal.PtrToStringUni(NativeMethods.FaqSend(_dm, server, handle, requestType, timeout));
        }

        #endregion

        #region Foobar

        /// <summary>
        /// 【Foobar】创建一个椭圆窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄,如果此值为0,那么就在桌面创建此窗口</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">椭圆区域的宽度</param>
        /// <param name="h">椭圆区域的高度</param>
        /// <returns>创建成功的窗口句柄</returns>
        public int CreateFoobarEllipse(int hwnd, int x, int y, int w, int h)
        {
            return NativeMethods.CreateFoobarEllipse(_dm, hwnd, x, y, w, h);
        }

        /// <summary>
        /// 【Foobar】创建一个矩形窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄,如果此值为0,那么就在桌面创建此窗口</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">矩形区域的宽度</param>
        /// <param name="h">矩形区域的高度</param>
        /// <returns>创建成功的窗口句柄</returns>
        public int CreateFoobarRect(int hwnd, int x, int y, int w, int h)
        {
            return NativeMethods.CreateFoobarRect(_dm, hwnd, x, y, w, h);
        }

        /// <summary>
        /// 【Foobar】创建一个圆角矩形窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄,如果此值为0,那么就在桌面创建此窗口</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">矩形区域的宽度</param>
        /// <param name="h">矩形区域的高度</param>
        /// <param name="rw">圆角的宽度</param>
        /// <param name="rh">圆角的高度</param>
        /// <returns>创建成功的窗口句柄</returns>
        public int CreateFoobarRoundRect(int hwnd, int x, int y, int w, int h, int rw, int rh)
        {
            return NativeMethods.CreateFoobarRoundRect(_dm, hwnd, x, y, w, h, rw, rh);
        }

        /// <summary>
        /// 【Foobar】根据指定的位图创建一个自定义形状的窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄,如果此值为0,那么就在桌面创建此窗口</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="picName">位图名字</param>
        /// <param name="transColor">透明色(RRGGBB)</param>
        /// <param name="sim">透明色的相似值 0.1-1.0</param>
        /// <returns>创建成功的窗口句柄</returns>
        public int CreateFoobarCustom(int hwnd, int x, int y, string picName, string transColor, double sim)
        {
            return NativeMethods.CreateFoobarCustom(_dm, hwnd, x, y, picName, transColor, sim);
        }

        /// <summary>
        /// 【Foobar】设置指定Foobar窗口的字体
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="fontName">系统字体名,注意,必须保证系统中有此字体</param>
        /// <param name="size">字体大小</param>
        /// <param name="flag">0.正常字体，1.粗体，2.斜体，4.下划线
        /// 文字可以是以上的组合 比如粗斜体就是1+2,斜体带下划线就是:2+4等</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarSetFont(int hwnd, string fontName, int size, int flag)
        {
            return NativeMethods.FoobarSetFont(_dm, hwnd, fontName, size, flag);
        }

        /// <summary>
        /// 设置保存指定的Foobar滚动文本区信息到文件
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="file">保存的文件名</param>
        /// <param name="enable">false.关闭向文件输出，true.开启向文件输出</param>
        /// <param name="header">输出的附加头信息. (比如行数 日期 时间信息) 格式是如下格式串的顺序组合.如果为空串，表示无附加头.
        /// "%L0nd%" 表示附加头信息带有行号，并且是按照十进制输出. n表示按多少个十进制数字补0对齐. 比如"%L04d%",输出的行号为0001  0002 0003等. "%L03d",输出的行号为001 002 003..等.
        /// "%L0nx%"表示附加头信息带有行号，并且是按照16进制小写输出. n表示按多少个16进制数字补0对齐. 比如"%L04x%",输出的行号为0009  000a 000b等. "%L03x",输出的行号为009 00a 00b..等.
        /// "%L0nX%"表示附加头信息带有行号，并且是按照16进制大写输出. n表示按多少个16进制数字补0对齐. 比如"%L04X%",输出的行号为0009  000A 000B等. "%L03X",输出的行号为009 00A 00B..等.
        /// "%yyyy%"表示年. 比如2012
        /// "%MM%"表示月. 比如12
        /// "%dd%"表示日. 比如28
        /// "%hh%"表示小时. 比如13
        /// "%mm%"表示分钟. 比如59
        /// "%ss%"表示秒. 比如48.</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarSetSave(int hwnd, string file, bool enable, string header)
        {
            return NativeMethods.FoobarSetSave(_dm, hwnd, file, enable ? 1 : 0, header);
        }

        /// <summary>
        /// 【Foobar】清除指定的Foobar滚动文本区
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarClearText(int hwnd)
        {
            return NativeMethods.FoobarClearText(_dm, hwnd);
        }

        /// <summary>
        /// 【Foobar】关闭一个Foobar,注意,必须调用此函数来关闭窗口,用SetWindowState也可以关闭,但会造成内存泄漏
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarClose(int hwnd)
        {
            return NativeMethods.FoobarClose(_dm, hwnd);
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口绘制图像 此图片不能是加密的图片
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="picName">图像文件名</param>
        /// <param name="transColor">图像透明色</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarDrawPic(int hwnd, int x, int y, string picName, string transColor)
        {
            return NativeMethods.FoobarDrawPic(_dm, hwnd, x, y, picName, transColor);
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口绘制文字
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">矩形区域的宽度</param>
        /// <param name="h">矩形区域的高度</param>
        /// <param name="text">字符串</param>
        /// <param name="color">文字颜色值</param>
        /// <param name="align">对方方式：1.左对齐，2.中间对方，4.右对齐</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarDrawText(int hwnd, int x, int y, int w, int h, string text, string color, int align)
        {
            return NativeMethods.FoobarDrawText(_dm, hwnd, x, y, w, h, text, color, align);
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口内部填充矩形
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x1">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y1">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="x2">右下角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y2">右下角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="color">填充的颜色值</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarFillRect(int hwnd, int x1, int y1, int x2, int y2, string color)
        {
            return NativeMethods.FoobarFillRect(_dm, hwnd, x1, y1, x2, y2, color);
        }

        /// <summary>
        /// 【Foobar】锁定指定的Foobar窗口,不能通过鼠标来移动
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarLock(int hwnd)
        {
            return NativeMethods.FoobarLock(_dm, hwnd);
        }

        /// <summary>
        /// 【Foobar】解锁指定的Foobar窗口,可以通过鼠标来移动
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarUnlock(int hwnd)
        {
            return NativeMethods.FoobarUnlock(_dm, hwnd);
        }

        /// <summary>
        /// 【Foobar】向指定的Foobar窗口区域内输出滚动文字
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="text">文本内容</param>
        /// <param name="color">文本颜色</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarPrintText(int hwnd, string text, string color)
        {
            return NativeMethods.FoobarPrintText(_dm, hwnd, text, color);
        }

        /// <summary>
        /// 【Foobar】设置滚动文本区的文字行间距,默认是3
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="lineGap">文本行间距</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarTextLineGap(int hwnd, int lineGap = 3)
        {
            return NativeMethods.FoobarTextLineGap(_dm, hwnd, lineGap);
        }

        /// <summary>
        /// 【Foobar】设置滚动文本区的文字输出方向,默认是0
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="dir">文字方向：0.表示向下输出，1.表示向上输出</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarTextPrintDir(int hwnd, int dir = 0)
        {
            return NativeMethods.FoobarTextPrintDir(_dm, hwnd, dir);
        }

        /// <summary>
        /// 【Foobar】设置指定Foobar窗口的滚动文本框范围,默认的文本框范围是窗口区域
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">区域的宽度</param>
        /// <param name="h">区域的高度</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarTextRect(int hwnd, int x, int y, int w, int h)
        {
            return NativeMethods.FoobarTextRect(_dm, hwnd, x, y, w, h);
        }

        /// <summary>
        /// 【Foobar】刷新指定的Foobar窗口
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <returns>0.失败，1.成功</returns>
        public int FoobarUpdate(int hwnd)
        {
            return NativeMethods.FoobarUpdate(_dm, hwnd);
        }

        #endregion

        #region 杂项

        /// <summary>
        /// 【杂项】激活指定窗口所在进程的输入法
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="inputMethod">输入法名字。具体输入法名字对应表查看注册表中以下位置：
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layouts。
        /// 下面的每一项下的Layout Text的值就是输入法名字，比如 "中文 - QQ拼音输入法"，以此类推。</param>
        /// <returns>0.失败，1.成功</returns>
        public int ActiveInputMethod(int hwnd, string inputMethod)
        {
            return NativeMethods.ActiveInputMethod(_dm, hwnd, inputMethod);
        }

        /// <summary>
        /// 【杂项】检测指定窗口所在线程输入法是否开启
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="inputMethod">输入法名字。具体输入法名字对应表查看注册表中以下位置：
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layouts。
        /// 下面的每一项下的Layout Text的值就是输入法名字，比如 "中文 - QQ拼音输入法"，以此类推。</param>
        /// <returns>是否开启</returns>
        public bool CheckInputMethod(int hwnd, string inputMethod)
        {
            return NativeMethods.CheckInputMethod(_dm, hwnd, inputMethod) == 1;
        }

        /// <summary>
        /// 【杂项】检测系统中是否安装了指定输入法
        /// </summary>
        /// <param name="inputMethod">输入法名字。具体输入法名字对应表查看注册表中以下位置：
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layouts。
        /// 下面的每一项下的Layout Text的值就是输入法名字，比如 "中文 - QQ拼音输入法"，以此类推。</param>
        /// <returns>是否存在</returns>
        public bool HasInputMethod(string inputMethod)
        {
            return NativeMethods.FindInputMethod(_dm, inputMethod) == 1;
        }

        #endregion

        #endregion

        #region 静态方法

        internal static bool ReleaseResource(string name, bool overwritten = false, string outPath = null)
        {
            string filename = string.IsNullOrEmpty(outPath) ? name : Path.Combine(outPath, name);
            if (!File.Exists(filename) || overwritten)
            {
                Assembly assembly = typeof(DmPlugin).Assembly;
                string resName = assembly.GetManifestResourceNames().FirstOrDefault(m => m.Contains(name));
                if (resName == null)
                {
                    return false;
                }
                Stream stream = assembly.GetManifestResourceStream(resName);
                if (stream == null)
                {
                    return false;
                }
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                }
                return File.Exists(filename);
            }
            return true;
        }

        #endregion

        #region 继承释放接口方法

        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            Dispose();
        }

        ~DmPlugin()
        {
            //必须为false
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // 清理非托管资源
            if (_dm != IntPtr.Zero)
            {
                NativeMethods.UnBindWindow(_dm);
                _dm = IntPtr.Zero;
                NativeMethods.FreeDM();
            }
            //让类型知道自己已经被释放
            _disposed = true;
        }

        #endregion
    }
}