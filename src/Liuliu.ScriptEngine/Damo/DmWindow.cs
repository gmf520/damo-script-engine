// -----------------------------------------------------------------------
//  <copyright file="DmWindow.cs" company="柳柳软件">
//      Copyright (c) 2014 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-27 13:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Liuliu.ScriptEngine.Damo;


namespace Liuliu.ScriptEngine
{
    /// <summary>
    /// 大漠窗口类
    /// </summary>
    public class DmWindow
    {
        private readonly DmPlugin _dm;
        private readonly int _hwnd;
        private readonly DmSystem _system;

        /// <summary>
        /// 初始化一个<see cref="DmWindow"/>类型的新实例
        /// </summary>
        public DmWindow(DmPlugin dm, int hwnd)
        {
            _hwnd = hwnd;
            _dm = dm;
            _system = new DmSystem(dm);
            BindType = WindowBindType.None;
            IsBind = false;
        }

        /// <summary>
        /// 窗口句柄
        /// </summary>
        public int Hwnd { get { return _hwnd; } }

        /// <summary>
        /// 窗口的大漠对象
        /// </summary>
        public DmPlugin Dm { get { return _dm; } }

        /// <summary>
        /// 窗口所属操作系统
        /// </summary>
        public DmSystem System { get { return _system; } }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title
        {
            get { return Dm.GetWindowTitle(_hwnd); }
            set
            {
                if (!IsAlive)
                {
                    return;
                }
                Dm.SetWindowText(_hwnd, value);
            }
        }

        /// <summary>
        /// 窗口类型
        /// </summary>
        public string Class { get { return Dm.GetWindowClass(_hwnd); } }

        /// <summary>
        /// 窗口进程ID
        /// </summary>
        public int ProcessId { get { return Dm.GetWindowProcessId(_hwnd); } }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool IsBind { get; private set; }

        /// <summary>
        /// 获取 窗口的绑定模式
        /// </summary>
        public WindowBindType BindType { get; private set; }

        /// <summary>
        /// 获取 窗口外部输入锁定类型
        /// </summary>
        public InputLockType InputLockType { get; private set; }

        /// <summary>
        /// 窗口进程路径
        /// </summary>
        public string ProcessPath { get { return Dm.GetWindowProcessPath(_hwnd); } }

        /// <summary>
        /// 窗口客户端尺寸
        /// 
        /// </summary>
        public Tuple<int, int> ClientSize
        {
            get
            {
                int w, h;
                Dm.GetClientSize(_hwnd, out w, out h);
                return new Tuple<int, int>(w, h);
            }
            set
            {
                if (!IsAlive)
                {
                    return;
                }
                Dm.SetClientSize(_hwnd, value.Item1, value.Item2);
            }
        }


        /// <summary>
        /// 窗口矩形（四个角坐标）
        /// </summary>
        public Tuple<int, int, int, int> Rect
        {
            get
            {
                int x1, y1, x2, y2;
                Dm.GetWindowRect(_hwnd, out x1, out y1, out x2, out y2);
                return new Tuple<int, int, int, int>(x1, y1, x2, y2);
            }
        }

        /// <summary>
        /// 获取 窗口是否存在
        /// </summary>
        public bool IsAlive { get { return Dm.GetWindowState(_hwnd, 0); } }

        /// <summary>
        /// 获取 窗口是否激活
        /// </summary>
        public bool IsActive { get { return Dm.GetWindowState(_hwnd, 1); } }

        /// <summary>
        /// 获取 窗口是否显示
        /// </summary>
        public bool IsShowed { get { return Dm.GetWindowState(_hwnd, 2); } }

        /// <summary>
        /// 是否最小化
        /// </summary>
        public bool IsMin { get { return Dm.GetWindowState(_hwnd, 3); } }

        /// <summary>
        /// 是否最大化
        /// </summary>
        public bool IsMax { get { return Dm.GetWindowState(_hwnd, 4); } }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsOnTop { get { return Dm.GetWindowState(_hwnd, 5); } }

        /// <summary>
        /// 获取 窗口是否无响应
        /// </summary>
        public bool IsDead { get { return Dm.GetWindowState(_hwnd, 6); } }

        /// <summary>
        /// 前台绑定，独占鼠标
        /// </summary>
        public bool BindNormal()
        {
            if (IsBind)
            {
                if (BindType == WindowBindType.Normal)
                {
                    return true;
                }
                UnBind();
            }
            bool dmRet = Dm.BindWindow(_hwnd, DmBindDisplay.normal, DmBindMouse.normal, DmBindKeypad.normal, DmBindMode._0);
            if (dmRet)
            {
                IsBind = false;
                BindType = WindowBindType.None;
                return false;
            }
            BindType = WindowBindType.Normal;
            IsBind = true;
            return true;
        }

        /// <summary>
        /// 半后台绑定，不锁定鼠标输入
        /// </summary>
        public bool BindHalfBackground()
        {
            bool isMin = IsMin;
            if (IsBind)
            {
                if (BindType == WindowBindType.Half)
                {
                    return true;
                }
                UnBind();
            }
            RestoreAndNotActive();
            bool dmRet;
            if (Dm.IsFree)
            {
                dmRet = Dm.BindWindow(_hwnd, DmBindDisplay.dx, DmBindMouse.dx2, DmBindKeypad.dx, DmBindMode._0); 
            }
            else
            {
                dmRet = Dm.BindWindowEx(
                    _hwnd,
                    "dx2",
                    "dx.mouse.position.lock.api|dx.mouse.focus.input.api|dx.mouse.focus.input.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.cursor",
                    "dx.keypad.input.lock.api|dx.keypad.state.api|dx.keypad.api",
                    "dx.public.active.api|dx.public.active.message",
                    DmBindMode._4);
            }
            if (dmRet)
            {
                IsBind = false;
                BindType = WindowBindType.None;
                return false;
            }
            BindType = WindowBindType.Half;
            IsBind = true;
            if (isMin)
            {
                Min();
            }
            return true;
        }

        /// <summary>
        /// 全后台绑定，锁定鼠标输入
        /// </summary>
        public bool BindFullBackground()
        {
            if (IsBind)
            {
                if (BindType == WindowBindType.Full)
                {
                    return true;
                }
                UnBind();
            }
            RestoreAndNotActive();
            bool dmRet;
            if (Dm.IsFree)
            {
                dmRet = Dm.BindWindow(_hwnd, DmBindDisplay.dx, DmBindMouse.dx, DmBindKeypad.dx, DmBindMode._0); 
            }
            else
            {
                dmRet = Dm.BindWindowEx(
                    _hwnd,
                    "dx2",
                    "dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.cursor",
                    "dx.keypad.input.lock.api|dx.keypad.state.api|dx.keypad.api",
                    "dx.public.active.api|dx.public.active.message",
                    DmBindMode._4);
            }
            if (dmRet)
            {
                IsBind = false;
                BindType = WindowBindType.None;
                return false;
            }
            BindType = WindowBindType.Full;
            IsBind = true;
            return true;
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <returns></returns>
        public bool UnBind()
        {
            if (!IsBind)
            {
                return true;
            }
            int dmRet = Dm.UnBindWindow();
            if (dmRet == 0)
            {
                IsBind = true;
                return false;
            }
            IsBind = false;
            BindType = WindowBindType.None;
            return true;
        }

        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="x">窗口左上角X坐标</param>
        /// <param name="y">窗口左上角Y坐标</param>
        /// <returns>移动是否成功</returns>
        public bool Move(int x, int y)
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.MoveWindow(_hwnd, x, y);
            Dm.Delay(500);
            Tuple<int, int, int, int> rect = Rect;
            return rect.Item1 == x && rect.Item2 == y;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            if (!IsAlive)
            {
                return true;
            }
            Dm.SetWindowState(_hwnd, 0);
            return !IsAlive;
        }

        /// <summary>
        /// 激活窗口
        /// </summary>
        /// <returns></returns>
        public bool Active()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 1);
            return IsActive;
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <returns></returns>
        public bool Min()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 2);
            return IsMin;
        }

        /// <summary>
        /// 最小化窗口并清理资源
        /// </summary>
        /// <returns></returns>
        public bool MinAndClean()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 3);
            return IsMin;
        }

        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <returns></returns>
        public bool Max()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 4);
            return IsMax;
        }

        /// <summary>
        /// 还原窗口但不激活
        /// </summary>
        /// <returns></returns>
        public bool RestoreAndNotActive()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 5);
            return !IsMin && !IsActive;
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <returns></returns>
        public bool Hide()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 6);
            return !IsShowed;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <returns></returns>
        public bool Show()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 7);
            return IsShowed;
        }

        /// <summary>
        /// 窗口置顶
        /// </summary>
        /// <returns></returns>
        public bool OnTop()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 8);
            return IsOnTop;
        }

        /// <summary>
        /// 取消置顶窗口
        /// </summary>
        /// <returns></returns>
        public bool CancelOnTop()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 9);
            return !IsOnTop;
        }

        /// <summary>
        /// 禁用窗口
        /// </summary>
        /// <returns></returns>
        public bool Disable()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 10);
            return !IsActive;
        }

        /// <summary>
        /// 启用窗口
        /// </summary>
        /// <returns></returns>
        public bool Enabled()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 11);
            return IsActive;
        }

        /// <summary>
        /// 还原并激活窗口
        /// </summary>
        /// <returns></returns>
        public bool RestoreAndActive()
        {
            if (!IsAlive)
            {
                return false;
            }
            Dm.SetWindowState(_hwnd, 12);
            return IsActive;
        }

        /// <summary>
        /// 设置窗口尺寸
        /// </summary>
        /// <param name="width">窗口宽度</param>
        /// <param name="height">窗口高度</param>
        /// <returns></returns>
        public bool SetSize(int width, int height)
        {
            if (!IsAlive)
            {
                return false;
            }

            Dm.SetWindowSize(_hwnd, width, height);
            Tuple<int, int, int, int> rect = Rect;
            return rect.Item3 - rect.Item1 == width && rect.Item4 - rect.Item2 == height;
        }

        /// <summary>
        /// 设置窗口透明度
        /// </summary>
        /// <param name="trans">透明度值</param>
        /// <returns></returns>
        public bool SetTransparent(int trans)
        {
            if (!IsAlive)
            {
                return false;
            }
            return Dm.SetWindowTransparent(_hwnd, trans);
        }

        /// <summary>
        /// 向窗口执行粘贴命令
        /// </summary>
        /// <returns></returns>
        public bool SendPaste()
        {
            if (!IsAlive)
            {
                return false;
            }
            return Dm.SendPaste(_hwnd);
        }

        /// <summary>
        /// 向窗口发送字符串
        /// </summary>
        /// <param name="content">要发送的内容</param>
        /// <returns></returns>
        public bool SendString(string content)
        {
            if (!IsAlive)
            {
                return false;
            }
            return Dm.SendString(_hwnd, content);
        }

        /// <summary>
        /// 向窗口发送字符串2
        /// </summary>
        /// <param name="content">要发达的内容</param>
        /// <returns></returns>
        public bool SendString2(string content)
        {
            if (!IsAlive)
            {
                return false;
            }
            return Dm.SendString2(_hwnd, content);
        }

        /// <summary>
        /// 锁定窗口输入
        /// </summary>
        /// <param name="lockType">锁定类型</param>
        /// <param name="isSetType">是否改变窗口锁定类型</param>
        /// <returns></returns>
        public bool SetInputLock(InputLockType lockType, bool isSetType = true)
        {
            bool flag = _dm.LockInput((int)lockType);
            if (flag && isSetType)
            {
                InputLockType = lockType;
            }
            return flag;
        }

        /// <summary>
        /// 任务栏闪烁，默认闪烁10次，间隔500毫秒
        /// </summary>
        /// <param name="count">闪烁总次数</param>
        /// <param name="interval">闪烁时间间隔，单位毫秒</param>
        public void FlashWindow(int count = 10, int interval = 500)
        {
            IntPtr hwnd = new IntPtr(_hwnd);
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    bool invert = i % 2 == 0;
                    FlashWindow(hwnd, invert);
                    Thread.Sleep(interval);
                    if (IsActive)
                    {
                        break;
                    }
                }
                FlashWindow(hwnd, true);
            });
        }

        #region 私有方法

        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool invert);

        #endregion
    }


    /// <summary>
    /// 窗口绑定类型
    /// </summary>
    public enum WindowBindType
    {
        /// <summary>
        /// 表示窗口未绑定
        /// </summary>
        None,
        /// <summary>
        /// 表示窗口前台绑定
        /// </summary>
        Normal,
        /// <summary>
        /// 表示窗口半后台绑定
        /// </summary>
        Half,
        /// <summary>
        /// 表示窗口全后台绑定
        /// </summary>
        Full
    }


    /// <summary>
    /// 表示窗口输入锁定类型
    /// </summary>
    public enum InputLockType
    {
        /// <summary>
        /// 不锁定输入
        /// </summary>
        None = 0,
        /// <summary>
        /// 锁定鼠标键盘输入
        /// </summary>
        MouseAndKeyboard = 1,
        /// <summary>
        /// 锁定鼠标输入
        /// </summary>
        Mouse = 2,
        /// <summary>
        /// 锁定键盘输入 
        /// </summary>
        Keyboard = 3
    }
}