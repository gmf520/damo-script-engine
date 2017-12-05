// -----------------------------------------------------------------------
//  <copyright file="DmPlugin.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 23:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

using Liuliu.ScriptEngine.Damo;


namespace Liuliu.ScriptEngine
{
    /// <summary>
    /// 大漠插件C#免注册调用类
    /// 作者：清风抚断云（QQ:274838061），柳柳英侠（QQ:123202901）整理
    /// 大漠版本：6.1720
    /// 时间：2017-09-04 21:21:18
    /// </summary>
    /// <example>
    /// DmPlugin dm = new DmPlugin();
    /// string version = dm.Ver();
    /// </example>
    public class DmPlugin : IDisposable
    {
        private readonly URComLoader _urCom;
        private readonly IDmsoft _dm;

        /// <summary>
        /// 初始化一个<see cref="DmPlugin"/>类型的新实例
        /// </summary>
        public DmPlugin(string dmPath = "dm.dll")
        {
            _urCom = new URComLoader();
            var obj = _urCom.CreateObjectFromPath(dmPath, Guid.Parse("26037A0E-7CBD-4FFF-9C63-56F2D0770214"), true);
            _dm = obj as IDmsoft;
        }

        public IDmsoft Dm
        {
            get { return _dm; }
        }

        public bool IsFree
        {
            get { return false; }
        }

        public void Dispose()
        {
            _urCom?.Dispose();
            GC.SuppressFinalize(this);
        }

        #region 大漠接口方法

        #region 基本设置

        /// <summary>
        /// 【基本】设置是否开启或者关闭插件内部的图片缓存机制. (默认是打开).
        /// 注: 有些时候，系统内存比较吃紧，这时候再打开内部缓存，可能会导致缓存分配在虚拟内存，这样频繁换页，反而导致图色效率下降.这时候就建议关闭图色缓存.
        /// 所有图色缓存机制都是对本对象的，也就是说，调用图色缓存机制的函数仅仅对本对象生效.每个对象都有一个图色缓存队列.
        /// </summary>
        /// <returns>是否打开状态</returns>
        public bool EnablePicCache(bool enable)
        {
            return _dm.EnablePicCache(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【基本】获取注册在系统中的dm.dll的路径
        /// </summary>
        public string GetBasePath()
        {
            return _dm.GetBasePath();
        }

        /// <summary>
        /// 【基本】返回当前进程已经创建的dm对象个数.
        /// </summary>
        public int GetDmCount()
        {
            return _dm.GetDmCount();
        }

        /// <summary>
        /// 【基本】返回当前大漠对象的ID值，这个值对于每个对象是唯一存在的。可以用来判定两个大漠对象是否一致.
        /// </summary>
        public int GetID()
        {
            return _dm.GetID();
        }

        /// <summary>
        /// 【基本】获取插件命令的最后错误
        /// </summary>
        /// <returns>
        /// 返回值表示错误值。 0表示无错误.
        /// -1 : 表示你使用了绑定里的收费功能，但是没注册，无法使用.
        /// -2 : 使用模式0 2 时出现，因为目标窗口有保护.常见于win7以上系统.或者有安全软件拦截插件.解决办法: 关闭所有安全软件，然后再重新尝试.如果还不行就可以肯定是目标窗口有特殊保护.
        /// -3 : 使用模式0 2 时出现，可能目标窗口有保护，也可能是异常错误.可以尝试换绑定模式或许可以解决.
        /// -4 : 使用模式101 103时出现，这是异常错误.
        /// -5 : 使用模式101 103时出现, 这个错误的解决办法就是关闭目标窗口，重新打开再绑定即可.也可能是运行脚本的进程没有管理员权限.
        /// -6 : 被安全软件拦截。典型的是金山.360等.如果是360关闭即可。 如果是金山，必须卸载，关闭是没用的.
        /// -7 -9 : 使用模式101 103时出现,异常错误.还有可能是安全软件的问题，比如360等。尝试卸载360.
        /// -8 -10 : 使用模式101 103时出现, 目标进程可能有保护,也可能是插件版本过老，试试新的或许可以解决. -8可以尝试使用DmGuard中的np2盾配合.
        /// -11 : 使用模式101 103时出现, 目标进程有保护.告诉我解决。
        /// -12 : 使用模式101 103时出现, 目标进程有保护.告诉我解决。
        /// -13 : 使用模式101 103时出现, 目标进程有保护.或者是因为上次的绑定没有解绑导致。 尝试在绑定前调用ForceUnBindWindow.
        /// -14 : 可能系统缺少部分DLL,尝试安装d3d.
        /// -16 : 可能使用了绑定模式 0 和 101，然后可能指定了一个子窗口.导致不支持.可以换模式2或者103来尝试.另外也可以考虑使用父窗口或者顶级窗口.来避免这个错误。还有可能是目标窗口没有正常解绑 然后再次绑定的时候.
        /// -17 : 模式101 103时出现.这个是异常错误.告诉我解决.
        /// -18 : 句柄无效.
        /// -19 : 使用模式0 2 101时出现, 说明你的系统不支持这几个模式.可以尝试其他模式.
        /// -20 : 使用模式101 103 时出现, 说明目标进程里没有解绑，并且子绑定达到了最大.尝试在返回这个错误时，调用ForceUnBindWindow来强制解除绑定.
        /// -21 : 使用模式101 103 时出现, 说明目标进程里没有解绑.尝试在返回这个错误时，调用ForceUnBindWindow来强制解除绑定.
        /// -22 : 使用模式0 2, 绑定64位进程窗口时出现, 因为安全软件拦截插件释放的EXE文件导致.
        /// -23 : 使用模式0 2, 绑定64位进程窗口时出现, 因为安全软件拦截插件释放的DLL文件导致.
        /// -24 : 使用模式0 2, 绑定64位进程窗口时出现, 因为安全软件拦截插件运行释放的EXE.
        /// -25 : 使用模式0 2, 绑定64位进程窗口时出现, 因为安全软件拦截插件运行释放的EXE.
        /// -26 : 使用模式0 2, 绑定64位进程窗口时出现, 因为目标窗口有保护.常见于win7以上系统.或者有安全软件拦截插件.解决办法: 关闭所有安全软件，然后再重新尝试.如果还不行就可以肯定是目标窗口有特殊保护.
        /// -27 : 绑定64位进程窗口时出现，因为使用了不支持的模式，目前暂时只支持模式0 2 101 103
        /// -28 : 绑定32位进程窗口时出现，因为使用了不支持的模式，目前暂时只支持模式0 2 101 103
        /// -100 : 调用读写内存函数后，发现无效的窗口句柄
        /// -101 : 读写内存函数失败
        /// -200 : AsmCall失败
        /// </returns>
        public int GetLastError()
        {
            return _dm.GetLastError();
        }

        /// <summary>
        /// 【基本】获取全局路径.(可用于调试)
        /// </summary>
        public string GetPath()
        {
            return _dm.GetPath();
        }

        /// <summary>
        /// 【基本】调用此函数来注册，从而使用插件的高级功能.推荐使用此函数.
        /// </summary>
        /// <param name="code">注册码. (从大漠插件后台获取)</param>
        /// <param name="ver">版本附加信息. 可以在后台详细信息查看.可留空. 长度不能超过20. 并且只能包含数字和字母以及小数点. 这个版本信息不是插件版本.</param>
        /// <returns>
        /// 整形数:
        /// -1 : 无法连接网络,(可能防火墙拦截, 如果可以正常访问大漠插件网站，那就可以肯定是被防火墙拦截)
        /// -2 : 进程没有以管理员方式运行. (出现在win7 win8 vista 2008.建议关闭uac)
        /// 0 : 失败(未知错误)
        ///     1 : 成功
        /// 2 : 余额不足
        /// 3 : 绑定了本机器，但是账户余额不足50元.
        /// 4 : 注册码错误
        /// 5 : 你的机器或者IP在黑名单列表中或者不在白名单列表中.
        /// 6 : 非法使用插件.
        /// 7 : 你的帐号因为非法使用被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 77： 机器码或者IP因为非法使用，而被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 封禁是全局的，如果使用了别人的软件导致77，也一样会导致所有注册码均无法注册。解决办法是更换IP，更换MAC.
        /// -8 : 版本附加信息长度超过了20
        /// -9 : 版本附加信息里包含了非法字母.
        ///     空 : 这是不可能返回空的，如果出现空，那肯定是当前使用的版本不对,老的插件里没这个函数导致返回为空.最好参考文档中的标准写法,判断插件版本号.
        /// </returns>
        public int Reg(string code, string ver = null)
        {
            return _dm.Reg(code, ver);
        }

        /// <summary>
        /// 【基本】调用此函数来注册，从而使用插件的高级功能. 可以根据指定的IP列表来注册. 新手不建议使用!
        /// </summary>
        /// <param name="code">注册码. (从大漠插件后台获取)</param>
        /// <param name="ver">版本附加信息. 可以在后台详细信息查看.可留空. 长度不能超过20. 并且只能包含数字和字母以及小数点. 这个版本信息不是插件版本.</param>
        /// <param name="ip">插件注册的ip地址.可以用|来组合,依次对ip中的地址进行注册，直到成功. ip地址列表在VIP群中获取</param>
        /// <returns>
        /// 整形数:
        /// -1 : 无法连接网络,(可能防火墙拦截, 如果可以正常访问大漠插件网站，那就可以肯定是被防火墙拦截)
        /// -2 : 进程没有以管理员方式运行. (出现在win7 win8 vista 2008.建议关闭uac)
        /// 0 : 失败(未知错误)
        ///     1 : 成功
        /// 2 : 余额不足
        /// 3 : 绑定了本机器，但是账户余额不足50元.
        /// 4 : 注册码错误
        /// 5 : 你的机器或者IP在黑名单列表中或者不在白名单列表中.
        /// 6 : 非法使用插件.
        /// 7 : 你的帐号因为非法使用被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 77： 机器码或者IP因为非法使用，而被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 封禁是全局的，如果使用了别人的软件导致77，也一样会导致所有注册码均无法注册。解决办法是更换IP，更换MAC.
        /// -8 : 版本附加信息长度超过了20
        /// -9 : 版本附加信息里包含了非法字母.
        ///     空 : 这是不可能返回空的，如果出现空，那肯定是当前使用的版本不对,老的插件里没这个函数导致返回为空.最好参考文档中的标准写法,判断插件版本号.
        /// -10 : 非法的参数ip
        /// </returns>
        public int RegEx(string code, string ver, string ip)
        {
            return _dm.RegEx(code, ver, ip);
        }

        /// <summary>
        /// 【基本】调用此函数来注册，从而使用插件的高级功能. 可以根据指定的IP列表来注册.新手不建议使用! 此函数同RegEx函数的不同在于,此函数用于注册的机器码是不带mac地址的.
        /// </summary>
        /// <param name="code">注册码. (从大漠插件后台获取)</param>
        /// <param name="ver">版本附加信息. 可以在后台详细信息查看.可留空. 长度不能超过20. 并且只能包含数字和字母以及小数点. 这个版本信息不是插件版本.</param>
        /// <param name="ip">插件注册的ip地址.可以用|来组合,依次对ip中的地址进行注册，直到成功. ip地址列表在VIP群中获取</param>
        /// <returns>
        /// 整形数:
        /// -1 : 无法连接网络,(可能防火墙拦截, 如果可以正常访问大漠插件网站，那就可以肯定是被防火墙拦截)
        /// -2 : 进程没有以管理员方式运行. (出现在win7 win8 vista 2008.建议关闭uac)
        /// 0 : 失败(未知错误)
        ///     1 : 成功
        /// 2 : 余额不足
        /// 3 : 绑定了本机器，但是账户余额不足50元.
        /// 4 : 注册码错误
        /// 5 : 你的机器或者IP在黑名单列表中或者不在白名单列表中.
        /// 6 : 非法使用插件.
        /// 7 : 你的帐号因为非法使用被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 77： 机器码或者IP因为非法使用，而被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 封禁是全局的，如果使用了别人的软件导致77，也一样会导致所有注册码均无法注册。解决办法是更换IP，更换MAC.
        /// -8 : 版本附加信息长度超过了20
        /// -9 : 版本附加信息里包含了非法字母.
        ///     空 : 这是不可能返回空的，如果出现空，那肯定是当前使用的版本不对,老的插件里没这个函数导致返回为空.最好参考文档中的标准写法,判断插件版本号.
        /// -10 : 非法的参数ip
        /// </returns>
        public int RegExNoMac(string code, string ver, string ip)
        {
            return _dm.RegExNoMac(code, ver, ip);
        }

        /// <summary>
        /// 【基本】调用此函数来注册，从而使用插件的高级功能.推荐使用此函数. 新手不建议使用! 此函数同Reg函数的不同在于,此函数用于注册的机器码是不带mac地址的.
        /// </summary>
        /// <param name="code">注册码. (从大漠插件后台获取)</param>
        /// <param name="ver">版本附加信息. 可以在后台详细信息查看.可留空. 长度不能超过20. 并且只能包含数字和字母以及小数点. 这个版本信息不是插件版本.</param>
        /// <returns>
        /// 整形数:
        /// -1 : 无法连接网络,(可能防火墙拦截, 如果可以正常访问大漠插件网站，那就可以肯定是被防火墙拦截)
        /// -2 : 进程没有以管理员方式运行. (出现在win7 win8 vista 2008.建议关闭uac)
        /// 0 : 失败(未知错误)
        ///     1 : 成功
        /// 2 : 余额不足
        /// 3 : 绑定了本机器，但是账户余额不足50元.
        /// 4 : 注册码错误
        /// 5 : 你的机器或者IP在黑名单列表中或者不在白名单列表中.
        /// 6 : 非法使用插件.
        /// 7 : 你的帐号因为非法使用被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 77： 机器码或者IP因为非法使用，而被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）
        /// 封禁是全局的，如果使用了别人的软件导致77，也一样会导致所有注册码均无法注册。解决办法是更换IP，更换MAC.
        /// -8 : 版本附加信息长度超过了20
        /// -9 : 版本附加信息里包含了非法字母.
        ///     空 : 这是不可能返回空的，如果出现空，那肯定是当前使用的版本不对,老的插件里没这个函数导致返回为空.最好参考文档中的标准写法,判断插件版本号.
        /// </returns>
        public int RegNoMac(string code, string ver)
        {
            return _dm.RegNoMac(code, ver);
        }

        /// <summary>
        /// 【基本】设定图色的获取方式，默认是显示器或者后台窗口(具体参考BindWindow) 
        /// </summary>
        /// <param name="mode">
        /// 字符串: 图色输入模式取值有以下几种
        /// 1. "screen" 这个是默认的模式，表示使用显示器或者后台窗口
        /// 2. "pic:file" 指定输入模式为指定的图片,如果使用了这个模式，则所有和图色相关的函数均视为对此图片进行处理，比如文字识别查找图片 颜色 等等一切图色函数.
        /// 需要注意的是，设定以后，此图片就已经加入了缓冲，如果更改了源图片内容，那么需要释放此缓冲，重新设置.
        /// 3. "mem:addr,size" 指定输入模式为指定的图片,此图片在内存当中. addr为图像内存地址,size为图像内存大小.
        /// 如果使用了这个模式，则所有和图色相关的函数,均视为对此图片进行处理.
        /// 比如文字识别 查找图片 颜色 等等一切图色函数.
        /// </param>
        /// <returns>操作是否成功</returns>
        public bool SetDisplayInput(string mode)
        {
            return _dm.SetDisplayInput(mode) == 1;
        }

        /// <summary>
        /// 【基本】设置EnumWindow  EnumWindowByProcess  EnumWindowSuper FindWindow以及FindWindowEx的最长延时. 内部默认超时是10秒.
        /// 注: 有些时候，窗口过多，并且窗口结构过于复杂，可能枚举的时间过长. 那么需要调用这个函数来延长时间。避免漏掉窗口.
        /// </summary>
        /// <param name="delay">延迟时间，单位毫秒</param>
        /// <returns>操作是否成功</returns>
        public bool SetEnumWindowDelay(int delay)
        {
            return _dm.SetEnumWindowDelay(delay) == 1;
        }

        /// <summary>
        /// 【基本】设置全局路径,设置了此路径后,所有接口调用中,相关的文件都相对于此路径. 比如图片,字库等.
        /// </summary>
        /// <param name="path">路径,可以是相对路径,也可以是绝对路径</param>
        /// <returns>操作是否成功</returns>
        public bool SetPath(string path)
        {
            return _dm.SetPath(path) == 1;
        }

        /// <summary>
        /// 【基本】设置是否弹出错误信息,默认是打开.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetShowErrorMsg(bool show)
        {
            return _dm.SetShowErrorMsg(show ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【基本】返回当前插件版本号
        /// </summary>
        public string Ver()
        {
            return _dm.Ver();
        }

        #endregion

        #region 窗口

        /// <summary>
        /// 【窗口】把窗口坐标转换为屏幕坐标
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">窗口X坐标</param>
        /// <param name="y">窗口Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool ClientToScreen(int hwnd, ref object x, ref object y)
        {
            return _dm.ClientToScreen(hwnd, ref x, ref y) == 1;
        }

        /// <summary>
        /// 【窗口】根据指定进程名,枚举系统中符合条件的进程PID,并且按照进程打开顺序排序.
        /// </summary>
        /// <param name="name">进程名,比如qq.exe</param>
        /// <returns>返回所有匹配的进程PID,并按打开顺序排序,格式"pid1,pid2,pid3"</returns>
        public string EnumProcess(string name)
        {
            return _dm.EnumProcess(name);
        }

        /// <summary>
        /// 【窗口】根据指定条件,枚举系统中符合条件的窗口,可以枚举到按键自带的无法枚举到的窗口
        /// </summary>
        /// <param name="parent">获得的窗口句柄是该窗口的子窗口的窗口句柄,取0时为获得桌面句柄</param>
        /// <param name="title">窗口标题. 此参数是模糊匹配</param>
        /// <param name="className">窗口类名. 此参数是模糊匹配</param>
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
            return _dm.EnumWindow(parent, title, className, filter);
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
            return _dm.EnumWindowByProcess(processName, title, className, filter);
        }

        /// <summary>
        /// 【窗口】根据指定进程pid以及其它条件,枚举系统中符合条件的窗口,可以枚举到按键自带的无法枚举到的窗口
        /// </summary>
        /// <param name="pid">进程pid.</param>
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
        public string EnumWindowByProcessId(int pid, string title, string className, int filter)
        {
            return _dm.EnumWindowByProcessId(pid, title, className, filter);
        }

        /// <summary>
        /// 【窗口】根据两组设定条件来枚举指定窗口.
        /// </summary>
        /// <param name="spec1">查找串1. (内容取决于flag1的值)</param>
        /// <param name="flag1">0表示spec1的内容是标题
        /// 1表示spec1的内容是程序名字. (比如notepad)
        /// 2表示spec1的内容是类名
        /// 3表示spec1的内容是程序路径.(不包含盘符, 比如\windows\system32)
        /// 4表示spec1的内容是父句柄.(十进制表达的串)
        /// 5表示spec1的内容是父窗口标题
        /// 6表示spec1的内容是父窗口类名
        /// 7表示spec1的内容是顶级窗口句柄.(十进制表达的串)
        /// 8表示spec1的内容是顶级窗口标题
        /// 9表示spec1的内容是顶级窗口类名</param>
        /// <param name="type1">0精确判断 1模糊判断 </param>
        /// <param name="spec2">查找串2. (内容取决于flag2的值)</param>
        /// <param name="flag2">0表示spec2的内容是标题
        /// 1表示spec2的内容是程序名字. (比如notepad)
        /// 2表示spec2的内容是类名
        /// 3表示spec2的内容是程序路径.(不包含盘符, 比如\windows\system32)
        /// 4表示spec2的内容是父句柄.(十进制表达的串)
        /// 5表示spec2的内容是父窗口标题
        /// 6表示spec2的内容是父窗口类名
        /// 7表示spec2的内容是顶级窗口句柄.(十进制表达的串)
        /// 8表示spec2的内容是顶级窗口标题
        /// 9表示spec2的内容是顶级窗口类名</param>
        /// <param name="type2">0精确判断 1模糊判断</param>
        /// <param name="sort">0不排序.1对枚举出的窗口进行排序,按照窗口打开顺序.</param>
        public string EnumWindowSuper(string spec1, int flag1, int type1, string spec2, int flag2, int type2, int sort)
        {
            return _dm.EnumWindowSuper(spec1, flag1, type1, spec2, flag2, type2, sort);
        }

        /// <summary>
        /// 【窗口】查找第一个符合类名或者标题名的顶层窗口
        /// </summary>
        /// <param name="className">窗口类名，如果为空，则匹配所有</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindow(string className, string title)
        {
            return _dm.FindWindow(className, title);
        }

        /// <summary>
        /// 【窗口】根据指定的进程名字，来查找可见窗口.
        /// </summary>
        /// <param name="processName">进程名. 比如(notepad.exe).这里是精确匹配,但不区分大小写.</param>
        /// <param name="className">窗口类名，如果为空，则匹配所有. 这里的匹配是模糊匹配.</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有.这里的匹配是模糊匹配.</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindowByProcess(string processName, string className, string title)
        {
            return _dm.FindWindowByProcess(processName, className, title);
        }

        /// <summary>
        /// 【窗口】根据指定的进程Id，来查找可见窗口.
        /// </summary>
        /// <param name="processId">进程id</param>
        /// <param name="className">窗口类名，如果为空，则匹配所有. 这里的匹配是模糊匹配.</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有.这里的匹配是模糊匹配.</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindowByProcessId(int processId, string className, string title)
        {
            return _dm.FindWindowByProcessId(processId, className, title);
        }

        /// <summary>
        /// 【窗口】查找第一个符合类名或者标题名的顶层窗口,如果指定了parent,则在parent的第一层子窗口中查找。
        /// </summary>
        /// <param name="parent">父窗口句柄，如果为空，则匹配所有顶层窗口</param>
        /// <param name="className">窗口类名，如果为空，则匹配所有</param>
        /// <param name="title">窗口标题,如果为空，则匹配所有</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindowEx(int parent, string className, string title)
        {
            return _dm.FindWindowEx(parent, className, title);
        }

        /// <summary>
        /// 【窗口】根据两组设定条件来查找指定窗口.
        /// </summary>
        /// <param name="spec1">查找串1. (内容取决于flag1的值)</param>
        /// <param name="flag1">0表示spec1的内容是标题
        /// 1表示spec1的内容是程序名字. (比如notepad)
        /// 2表示spec1的内容是类名
        /// 3表示spec1的内容是程序路径.(不包含盘符, 比如\windows\system32)
        /// 4表示spec1的内容是父句柄.(十进制表达的串)
        /// 5表示spec1的内容是父窗口标题
        /// 6表示spec1的内容是父窗口类名
        /// 7表示spec1的内容是顶级窗口句柄.(十进制表达的串)
        /// 8表示spec1的内容是顶级窗口标题
        /// 9表示spec1的内容是顶级窗口类名</param>
        /// <param name="type1">0精确判断 1模糊判断 </param>
        /// <param name="spec2">查找串2. (内容取决于flag2的值)</param>
        /// <param name="flag2">0表示spec2的内容是标题
        /// 1表示spec2的内容是程序名字. (比如notepad)
        /// 2表示spec2的内容是类名
        /// 3表示spec2的内容是程序路径.(不包含盘符, 比如\windows\system32)
        /// 4表示spec2的内容是父句柄.(十进制表达的串)
        /// 5表示spec2的内容是父窗口标题
        /// 6表示spec2的内容是父窗口类名
        /// 7表示spec2的内容是顶级窗口句柄.(十进制表达的串)
        /// 8表示spec2的内容是顶级窗口标题
        /// 9表示spec2的内容是顶级窗口类名</param>
        /// <param name="type2">0精确判断 1模糊判断</param>
        /// <returns>整形数表示的窗口句柄，没找到返回0</returns>
        public int FindWindowSuper(string spec1, int flag1, int type1, string spec2, int flag2, int type2)
        {
            return _dm.FindWindowSuper(spec1, flag1, type1, spec2, flag2, type2);
        }

        /// <summary>
        /// 【窗口】获取窗口客户区域在屏幕上的位置
        /// </summary>
        public int GetClientRect(int hwnd, out int x1, out int y1, out int x2, out int y2)
        {
            int result = _dm.GetClientRect(hwnd, out object X1, out object Y1, out object X2, out object Y2);
            x1 = (int)X1;
            y1 = (int)Y1;
            x2 = (int)X2;
            y2 = (int)Y2;
            return result;
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
            int result = _dm.GetClientSize(hwnd, out object w, out object h);
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
            return _dm.GetForegroundFocus();
        }

        /// <summary>
        /// 【窗口】获取顶层活动窗口,可以获取到按键自带插件无法获取到的句柄
        /// </summary>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetForegroundWindow()
        {
            return _dm.GetForegroundWindow();
        }

        /// <summary>
        /// 【窗口】获取鼠标指向的窗口句柄,可以获取到按键自带的插件无法获取到的句柄
        /// </summary>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetMousePointWindow()
        {
            return _dm.GetMousePointWindow();
        }

        /// <summary>
        /// 【窗口】获取给定坐标的窗口句柄,可以获取到按键自带的插件无法获取到的句柄
        /// </summary>
        /// <param name="x">屏幕X坐标</param>
        /// <param name="y">屏幕Y坐标</param>
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetPointWindow(int x, int y)
        {
            return _dm.GetPointWindow(x, y);
        }

        /// <summary>
        /// 【窗口】根据指定的pid获取进程详细信息,(进程名,进程全路径,CPU占用率(百分比),内存占用量(字节))
        /// </summary>
        /// <returns>格式"进程名|进程路径|cpu|内存"</returns>
        public string GetProcessInfo(int pid)
        {
            return _dm.GetProcessInfo(pid);
        }

        /// <summary>
        /// 【窗口】获取特殊窗口
        /// </summary>
        /// <param name="flag">标志：0为获取桌面窗口，1为获取任务栏窗口</param>
        /// <returns>以整型数表示的窗口句柄</returns>
        public int GetSpecialWindow(int flag)
        {
            return _dm.GetSpecialWindow(flag);
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
        /// <returns>返回整型表示的窗口句柄</returns>
        public int GetWindow(int hwnd, int flag)
        {
            return _dm.GetWindow(hwnd, flag);
        }

        /// <summary>
        /// 【窗口】获取窗口的类名
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>窗口的类名</returns>
        public string GetWindowClass(int hwnd)
        {
            return _dm.GetWindowClass(hwnd);
        }

        /// <summary>
        /// 【窗口】获取指定窗口所在的进程ID。
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>返回整型表示的是进程ID</returns>
        public int GetWindowProcessId(int hwnd)
        {
            return _dm.GetWindowProcessId(hwnd);
        }

        /// <summary>
        /// 【窗口】获取指定窗口所在的进程的exe文件全路径
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>返回字符串表示的是exe全路径名</returns>
        public string GetWindowProcessPath(int hwnd)
        {
            return _dm.GetWindowProcessPath(hwnd);
        }

        /// <summary>
        /// 【窗口】获取窗口在屏幕上的位置
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x1">窗口左上角X坐标</param>
        /// <param name="y1">窗口左上角Y坐标</param>
        /// <param name="x2">窗口右下角X坐标</param>
        /// <param name="y2">窗口右下角Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool GetWindowRect(int hwnd, out int x1, out int y1, out int x2, out int y2)
        {
            int result = _dm.GetWindowRect(hwnd, out object a, out object b, out object c, out object d);
            x1 = (int)a;
            y1 = (int)b;
            x2 = (int)c;
            y2 = (int)d;
            return result == 1;
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
        public bool GetWindowState(int hwnd, int flag)
        {
            return _dm.GetWindowState(hwnd, flag) == 1;
        }

        /// <summary>
        /// 【窗口】获取窗口的标题
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>窗口的标题</returns>
        public string GetWindowTitle(int hwnd)
        {
            return _dm.GetWindowTitle(hwnd);
        }

        /// <summary>
        /// 【窗口】移动指定窗口到指定位置
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool MoveWindow(int hwnd, int x, int y)
        {
            return _dm.MoveWindow(hwnd, x, y) == 1;
        }

        /// <summary>
        /// 【窗口】把屏幕坐标转换为窗口坐标
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="x">窗口X坐标</param>
        /// <param name="y">窗口Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool ScreenToClient(int hwnd, ref object x, ref object y)
        {
            return _dm.ScreenToClient(hwnd, ref x, ref y) == 1;
        }

        /// <summary>
        /// 【窗口】向指定窗口发送粘贴命令. 把剪贴板的内容发送到目标窗口
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <returns>操作是否成功</returns>
        public bool SendPaste(int hwnd)
        {
            return _dm.SendPaste(hwnd) == 1;
        }

        /// <summary>
        /// 【窗口】向指定窗口发送文本数据
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="content">发送的文本数据</param>
        /// <returns>操作是否成功</returns>
        public bool SendString(int hwnd, string content)
        {
            int result = _dm.SendString(hwnd, content);
            //Delay(100);
            return result == 1;
        }

        /// <summary>
        /// 【窗口】向指定窗口发送文本数据。此接口为老的SendString，如果新的SendString不能输入，可以尝试此接口。
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="content">发送的文本数据</param>
        /// <returns>操作是否成功</returns>
        public bool SendString2(int hwnd, string content)
        {
            int result = _dm.SendString2(hwnd, content);
            //Delay(100);
            return result == 1;
        }

        /// <summary>
        /// 【窗口】向绑定的窗口发送文本数据.必须配合dx.public.input.ime属性
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SendStringIme(string content)
        {
            return _dm.SendStringIme(content) == 1;
        }

        /// <summary>
        /// 【窗口】利用真实的输入法，对指定的窗口输入文字
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="content">发送的文本数据</param>
        /// <param name="mode">0 : 向hwnd的窗口输入文字(前提是必须先用模式200安装了输入法)
        /// 1 : 同模式0,如果由于保护无效，可以尝试此模式.(前提是必须先用模式200安装了输入法)
        /// 2 : 同模式0,如果由于保护无效，可以尝试此模式. (前提是必须先用模式200安装了输入法)
        /// 200 : 向系统中安装输入法,多次调用没问题.全局只用安装一次.
        /// 300 : 卸载系统中的输入法.全局只用卸载一次.多次调用没关系.</param>
        /// <remarks>
        /// 注: 如果要同时对此窗口进行绑定，并且绑定的模式是1 3 5 7 101 103，那么您必须要在绑定之前,先执行加载输入法的操作. 否则会造成绑定失败!.
        /// 卸载时，没有限制.
        ///     还有，在后台输入时，如果目标窗口有判断是否在激活状态才接受输入文字,那么可以配合绑定窗口中的假激活属性来保证文字正常输入.诸如此类.基本上用这个没有输入不了的文字.
        ///     比如
        ///     BindWindow hwnd,"normal","normal","normal","dx.public.active.api|dx.public.active.message",0
        /// dm.SendStringIme2 hwnd,"哈哈",0
        /// </remarks>
        /// <returns>操作是否成功</returns>
        public bool SendStringIme2(int hwnd, string content, int mode)
        {
            return _dm.SendStringIme2(hwnd, content, mode) == 1;
        }

        /// <summary>
        /// 【窗口】设置窗口客户区域的宽度和高度
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetClientSize(int hwnd, int width, int height)
        {
            return _dm.SetClientSize(hwnd, width, height) == 1;
        }

        /// <summary>
        /// 【窗口】设置窗口的大小
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetWindowSize(int hwnd, int width, int height)
        {
            return _dm.SetWindowSize(hwnd, width, height) == 1;
        }

        /// <summary>
        /// 【窗口】设置窗口的状态
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="flag">
        /// 0 : 关闭指定窗口
        /// 1 : 激活指定窗口
        /// 2 : 最小化指定窗口,但不激活
        /// 3 : 最小化指定窗口,并释放内存,但同时也会激活窗口.(释放内存可以考虑用FreeProcessMemory函数)
        /// 4 : 最大化指定窗口,同时激活窗口.
        /// 5 : 恢复指定窗口 ,但不激活
        /// 6 : 隐藏指定窗口
        /// 7 : 显示指定窗口
        /// 8 : 置顶指定窗口
        /// 9 : 取消置顶指定窗口
        /// 10 : 禁止指定窗口
        /// 11 : 取消禁止指定窗口
        /// 12 : 恢复并激活指定窗口
        /// 13 : 强制结束窗口所在进程.
        /// 14 : 闪烁指定的窗口
        /// 15 : 使指定的窗口获取输入焦点
        /// </param>
        /// <returns>操作是否成功</returns>
        public bool SetWindowState(int hwnd, int flag)
        {
            return _dm.SetWindowState(hwnd, flag) == 1;
        }

        /// <summary>
        /// 【窗口】设置窗口的标题
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetWindowText(int hwnd, string title)
        {
            return _dm.SetWindowText(hwnd, title) == 1;
        }

        /// <summary>
        /// 【窗口】设置窗口的透明度
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="trans">透明度取值(0-255) 越小透明度越大 0为完全透明(不可见) 255为完全显示(不透明)</param>
        /// <returns>操作是否成功</returns>
        public bool SetWindowTransparent(int hwnd, int trans)
        {
            return _dm.SetWindowTransparent(hwnd, trans) == 1;
        }
        #endregion

        #region 键盘鼠标

        /// <summary>
        /// 【键鼠】设置当前系统鼠标的精确度开关. 如果所示。 此接口仅仅对开启了硬件模拟鼠标的MoveR接口起作用.
        /// </summary>
        /// <param name="enable">false 关闭指针精确度开关.  true打开指针精确度开关. 一般推荐关闭.</param>
        /// <returns>设置之前的精确度开关</returns>
        public bool EnableMouseAccuracy(bool enable)
        {
            return _dm.EnableMouseAccuracy(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【键鼠】获取系统鼠标在屏幕上的位置
        /// </summary>
        /// <param name="x">返回X坐标</param>
        /// <param name="y">返回Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool GetCursorPos(out int x, out int y)
        {
            bool result = _dm.GetCursorPos(out object a, out object b) == 1;
            x = (int)a;
            y = (int)b;
            return result;
        }

        /// <summary>
        /// 【键鼠】获取鼠标特征码。当BindWindow或者BindWindowEx中的mouse参数含有dm.mouse.cursor时，
        /// 获取到的是后台鼠标特征，否则是前台鼠标特征。
        /// </summary>
        /// <returns>成功时返回鼠标特征码，失败时返回空的串</returns>
        public string GetCursorShape()
        {
            return _dm.GetCursorShape();
        }

        /// <summary>
        /// 【键鼠】获取鼠标特征码. 当BindWindow或者BindWindowEx中的mouse参数含有dx.mouse.cursor时，
        /// 获取到的是后台鼠标特征，否则是前台鼠标特征.
        /// </summary>
        /// <param name="type">获取鼠标特征码的方式. 和工具中的方式1 方式2对应. 方式1此参数值为0. 方式2此参数值为1</param>
        /// <returns>成功时返回鼠标特征码，失败时返回空的串</returns>
        public string GetCursorShapeEx(int type)
        {
            return _dm.GetCursorShapeEx(type);
        }

        /// <summary>
        /// 【键鼠】获取鼠标热点位置。当BindWindow或者BindWindowEx中的mouse参数含有dm.mouse.cursor时，
        /// 获取到的是后台鼠标热点位置，否则是前台鼠标热点位置。
        /// </summary>
        /// <returns>成功时，返回形如"x,y"的字符串  失败时，返回空的串.</returns>
        public string GetCursorSpot()
        {
            return _dm.GetCursorSpot();
        }

        /// <summary>
        /// 【键鼠】获取指定的按键状态.(前台信息,不是后台)
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>0.弹起，1.按下</returns>
        public int GetKeyState(int keyCode)
        {
            return _dm.GetKeyState(keyCode);
        }

        /// <summary>
        /// 【键鼠】获取系统鼠标的移动速度.  如图所示红色区域. 一共分为11个级别. 从1开始,11结束 此接口仅仅对开启了硬件模拟鼠标的MoveR接口起作用.
        /// </summary>
        /// <returns>0:失败 其他值,当前系统鼠标的移动速度</returns>
        public int GetMouseSpeed()
        {
            return _dm.GetMouseSpeed();
        }

        /// <summary>
        /// 【键鼠】按住指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>操作是否成功</returns>
        public bool KeyDown(int keyCode)
        {
            return _dm.KeyDown(keyCode) == 1;
        }

        /// <summary>
        /// 【键鼠】按住指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟键名</param>
        /// <returns>操作是否成功</returns>
        public bool KeyDownChar(string keyStr)
        {
            return _dm.KeyDownChar(keyStr) == 1;
        }

        /// <summary>
        /// 【键鼠】按下指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <param name="count">次数</param>
        /// <returns>操作是否成功</returns>
        public bool KeyPress(int keyCode, int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && _dm.KeyPress(keyCode) == 1;
            }
            return flag;
        }

        /// <summary>
        /// 【键鼠】按下指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟按键名</param>
        /// <param name="count">次数</param>
        /// <returns>操作是否成功</returns>
        public bool KeyPressChar(string keyStr, int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && _dm.KeyPressChar(keyStr) == 1;
            }
            return flag;
        }

        /// <summary>
        /// 【键鼠】根据指定的字符串序列，依次按顺序按下其中的字符.
        /// </summary>
        /// <param name="keystr">需要按下的字符串序列. 比如"1234","abcd","7389,1462"等</param>
        /// <param name="delay">每按下一个按键，需要延时多久. 单位毫秒.这个值越大，按的速度越慢</param>
        /// <returns>操作是否成功</returns>
        public bool KeyPressStr(string keystr, int delay)
        {
            return _dm.KeyPressStr(keystr, delay) == 1;
        }

        /// <summary>
        /// 【键鼠】弹起指定的虚拟键码
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <returns>操作是否成功</returns>
        public bool KeyUp(int keyCode)
        {
            return _dm.KeyUp(keyCode) == 1;
        }

        /// <summary>
        /// 【键鼠】弹起指定的虚拟键名
        /// </summary>
        /// <param name="keyStr">虚拟键名</param>
        /// <returns>操作是否成功</returns>
        public bool KeyUpChar(string keyStr)
        {
            return _dm.KeyUpChar(keyStr) == 1;
        }

        /// <summary>
        /// 【键鼠】按下鼠标左键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftClick(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && _dm.LeftClick() == 1;
            }
            return flag;
        }

        /// <summary>
        /// 【键鼠】按下鼠标左键并延时
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftClickWithDelay(int mis)
        {
            bool flag = _dm.LeftDown() == 1;
            //Delay(mis);
            return flag && _dm.LeftUp() == 1;
        }

        /// <summary>
        /// 【键鼠】按下Ctrl键并单击鼠标左键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftClickCtrl()
        {
            bool flag = _dm.KeyDown(17) == 1;
            //Delay(50);
            flag = flag && _dm.LeftClick() == 1;
            //Delay(50);
            flag = flag && _dm.KeyUp(17) == 1;
            return flag;
        }

        /// <summary>
        /// 【键鼠】双击鼠标左键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftDoubleClick()
        {
            return _dm.LeftDoubleClick() == 1;
        }

        /// <summary>
        /// 【键鼠】按住鼠标左键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftDown()
        {
            return _dm.LeftDown() == 1;
        }

        /// <summary>
        /// 【键鼠】弹起鼠标左键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeftUp()
        {
            return _dm.LeftUp() == 1;
        }

        /// <summary>
        /// 【键鼠】按下鼠标中键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool MiddleClick()
        {
            return _dm.MiddleClick() == 1;
        }

        /// <summary>
        /// 【键鼠】按住鼠标中键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool MiddleDown()
        {
            return _dm.MiddleDown() == 1;
        }

        /// <summary>
        /// 【键鼠】弹起鼠标中键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool MiddleUp()
        {
            return _dm.MiddleUp() == 1;
        }

        /// <summary>
        /// 【键鼠】鼠标相对于上次的位置移动rx,ry  从6.1548版本开始,如果您使用了硬件模拟的前台鼠标,最好配合SetMouseSpeed和EnableMouseAccuracy函数来使用.
        /// </summary>
        /// <param name="rx">相对于上次的X偏移</param>
        /// <param name="ry">相对于上次的Y偏移</param>
        /// <returns>操作是否成功</returns>
        public bool MoveR(int rx, int ry)
        {
            return _dm.MoveR(rx, ry) == 1;
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool MoveTo(int x, int y)
        {
            return _dm.MoveTo(x, y) == 1;
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
            return _dm.MoveToEx(x, y, w, h);
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)，并按下左键
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToClick(int x, int y)
        {
            bool flag = _dm.MoveTo(x, y) == 1;
            //Delay(300);
            flag = flag && _dm.LeftClick() == 1;
            //Delay(100);
            return flag;
        }

        /// <summary>
        /// 【键鼠】把鼠标移动到目的点(x,y)，并按下右键
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToRightClick(int x, int y)
        {
            bool flag = _dm.MoveTo(x, y) == 1;
            //Delay(400);
            flag = flag && _dm.RightClick() == 1;
            //Delay(400);
            return flag;
        }

        /// <summary>
        /// 【键鼠】按下鼠标右键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool RightClick()
        {
            return _dm.RightClick() == 1;
        }

        /// <summary>
        /// 【键鼠】按住鼠标右键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool RightDown()
        {
            return _dm.RightDown() == 1;
        }

        /// <summary>
        /// 【键鼠】弹起鼠标右键
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool RightUp()
        {
            return _dm.RightUp() == 1;
        }

        /// <summary>
        /// 【键鼠】设置按键时,键盘按下和弹起的时间间隔。高级用户使用。某些窗口可能需要调整这个参数才可以正常按键。
        /// </summary>
        /// <param name="type">键盘类型,取值有以下：
        /// normal.对应normal鼠标 默认内部延时为 30ms
        /// windows.对应windows 鼠标 默认内部延时为 10ms
        /// dx.对应dx鼠标默认内部延时为40ms</param>
        /// <param name="delay">延时,单位是毫秒</param>
        /// <returns>操作是否成功</returns>
        public bool SetKeypadDelay(string type, int delay)
        {
            return _dm.SetKeypadDelay(type, delay) == 1;
        }

        /// <summary>
        /// 【键鼠】设置鼠标单击或者双击时,鼠标按下和弹起的时间间隔。高级用户使用。某些窗口可能需要调整这个参数才可以正常点击。
        /// </summary>
        /// <param name="type">鼠标类型,取值有以下：
        /// normal.对应normal鼠标 默认内部延时为 30ms
        /// windows.对应windows 鼠标 默认内部延时为 10ms
        /// dx.对应dx鼠标默认内部延时为40ms</param>
        /// <param name="delay">延时,单位是毫秒</param>
        /// <returns>操作是否成功</returns>
        public bool SetMouseDelay(string type, int delay)
        {
            return _dm.SetMouseDelay(type, delay) == 1;
        }

        /// <summary>
        /// 【键鼠】设置系统鼠标的移动速度.  如图所示红色区域. 一共分为11个级别. 从1开始,11结束。此接口仅仅对开启了硬件模拟鼠标的MoveR接口起作用
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetMouseSpeed(int speed)
        {
            return _dm.SetMouseSpeed(speed) == 1;
        }

        /// <summary>
        /// 【键鼠】设置前台键鼠的模拟方式
        /// </summary>
        /// <param name="mode">
        /// 0 正常模式(默认模式)
        /// 1 硬件模拟
        /// 2 硬件模拟2(ps2)（仅仅支持标准的3键鼠标，即左键，右键，中键，带滚轮的鼠标,2键和5键等扩展鼠标不支持）
        /// 3 硬件模拟3
        /// </param>
        /// <returns>
        /// 0  : 插件没注册
        /// -1 : 32位系统不支持
        /// -2 : 驱动释放失败.
        /// -3 : 驱动加载失败.可能是权限不够.参考UAC权限设置.或者是被安全软件拦截.
        ///     如果是WIN10 1607之后的系统，出现这个错误，可参考这里
        /// 1  : 成功
        /// </returns>
        public int SetSimMode(int mode)
        {
            return _dm.SetSimMode(mode);
        }

        /// <summary>
        /// 【键鼠】等待指定的按键按下 (前台,不是后台)
        /// </summary>
        /// <param name="keyCode">虚拟按键码</param>
        /// <param name="timeout">等待多久,单位毫秒. 如果是0，表示一直等待</param>
        /// <returns>操作是否成功</returns>
        /// <returns>0.超时，1.指定的按键按下</returns>
        public int WaitKey(int keyCode, int timeout)
        {
            return _dm.WaitKey(keyCode, timeout);
        }

        /// <summary>
        /// 【键鼠】滚轮向下滚
        /// </summary>
        /// <param name="count">次数</param>
        /// <returns>操作是否成功</returns>
        public bool WheelDown(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && _dm.WheelDown() == 1;
            }
            return flag;
        }

        /// <summary>
        /// 【键鼠】滚轮向上滚
        /// </summary>
        /// <param name="count">次数</param>
        /// <returns>操作是否成功</returns>
        public bool WheelUp(int count = 1)
        {
            bool flag = true;
            for (int i = 0; i < count; i++)
            {
                flag = flag && _dm.WheelUp() == 1;
            }
            return flag;
        }

        #endregion

        #region 文字识别

        /// <summary>
        /// 【文字】给指定的字库中添加一条字库信息
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <param name="dictInfo">字库描述串，具体参考大漠综合工具中的字符定义</param>
        /// <returns>操作是否成功</returns>
        public bool AddDict(int index, string dictInfo)
        {
            return _dm.AddDict(index, dictInfo) == 1;
        }

        /// <summary>
        /// 【文字】清空指定的字库
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <returns>操作是否成功</returns>
        public bool ClearDict(int index)
        {
            return _dm.ClearDict(index) == 1;
        }

        /// <summary>
        /// 【文字】允许当前调用的对象使用全局字库。  如果你的程序中对象太多,并且每个对象都用到了同样的字库,可以考虑用全局字库,这样可以节省大量内存.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableShareDict(bool enable)
        {
            return _dm.EnableShareDict(enable ? 1 : 0) == 1;
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
            return _dm.FetchWord(x1, y1, x2, y2, color, word);
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
            int result = _dm.FindStr(x1, y1, x2, y2, str, color, sim, out object x, out object y);
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
            return _dm.FindStrE(x1, y1, x2, y2, str, color, sim);
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
            return _dm.FindStrEx(x1, y1, x2, y2, str, color, sim);
        }

        /// <summary>
        /// 【文字】在屏幕范围(x1,y1,x2,y2)内,查找string(可以是任意字符串的组合),并返回符合color_format的所有坐标位置,相似度sim同Ocr接口描述.
        /// (多色, 差色查找类似于Ocr接口, 不再重述). 此函数同FindStrEx,只是返回值不同.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回所有找到的坐标集合,格式如下:
        /// "str,x0,y0| str,x1,y1|......| str,xn,yn"
        /// 比如"长安,100,20|大雁塔,30,40" 表示找到了两个,第一个是长安 ,坐标是(100,20),第二个是大雁塔,坐标(30,40)</returns>
        public string FindStrExS(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return _dm.FindStrExS(x1, y1, x2, y2, str, color, sim);
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
            int result = _dm.FindStrFast(x1, y1, x2, y2, str, color, sim, out object x, out object y);
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
            return _dm.FindStrFastE(x1, y1, x2, y2, str, color, sim);
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
            return _dm.FindStrFastEx(x1, y1, x2, y2, str, color, sim);
        }

        /// <summary>
        /// 【文字】同FindStrExS。注: 此函数比FindStrEx要快很多，尤其是在字库很大时，或者模糊识别时，效果非常明显。
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
        public string FindStrFastExS(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return _dm.FindStrFastExS(x1, y1, x2, y2, str, color, sim);
        }

        /// <summary>
        /// 【文字】同FindStrS.注: 此函数比FindStrEx要快很多，尤其是在字库很大时，或者模糊识别时，效果非常明显。
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
        /// <param name="intX">返回X坐标 没找到返回-1</param>
        /// <param name="intY">返回Y坐标 没找到返回-1</param>
        /// <returns>返回找到的字符串. 没找到的话返回长度为0的字符串.</returns>
        public string FindStrFastS(int x1, int y1, int x2, int y2, string str, string color, double sim, out int intX, out int intY)
        {
            string result = _dm.FindStrFastS(x1, y1, x2, y2, str, color, sim, out object x, out object y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】在屏幕范围(x1,y1,x2,y2)内,查找string(可以是任意个字符串的组合),并返回符合color_format的坐标位置,相似度sim同Ocr接口描述.
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
        /// <returns>返回找到的字符串. 没找到的话返回长度为0的字符串.</returns>
        public string FindStrS(int x1, int y1, int x2, int y2, string str, string color, double sim, out int intX, out int intY)
        {
            string result = _dm.FindStrS(x1, y1, x2, y2, str, color, sim, out object x, out object y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】同FindStr，但是不使用SetDict设置的字库，而利用系统自带的字库，速度比FindStr稍慢.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="str">待查找的字符串,可以是字符串组合，比如"长安|洛阳|大雁塔",中间用"|"来分割字符串</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="fontName">系统字体名,比如"宋体"</param>
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程.</param>
        /// <param name="flag">整形数:字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.
        ///  1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <param name="intX">返回X坐标 没找到返回-1</param>
        /// <param name="intY">返回Y坐标 没找到返回-1</param>
        /// <returns>返回字符串的索引 没找到返回-1, 比如"长安|洛阳",若找到长安，则返回0</returns>
        public int FindStrWithFont(int x1, int y1, int x2, int y2, string str, string color, double sim, string fontName, int fontSize, int flag, out int intX, out int intY)
        {
            int result = _dm.FindStrWithFont(x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag, out object x, out object y);
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
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程.</param>
        /// <param name="flag">整形数:字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.
        ///  1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <returns>返回字符串序号以及X和Y坐标,形式如"id|x|y", 比如"0|100|200",没找到时，id和X以及Y均为-1，"-1|-1|-1"</returns>
        public string FindStrWithFontE(int x1, int y1, int x2, int y2, string str, string color, double sim, string fontName, int fontSize, int flag)
        {
            return _dm.FindStrWithFontE(x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag);
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
        /// <param name="fontSize">系统字体尺寸，这个尺寸一定要以大漠综合工具获取的为准.如果获取尺寸看视频教程.</param>
        /// <param name="flag">整形数:字体类别 取值可以是以下值的组合,比如1+2+4+8,2+4. 0表示正常字体.
        ///  1 : 粗体 2 : 斜体 4 : 下划线 8 : 删除线 </param>
        /// <returns>返回所有找到的坐标集合,格式如下:
        /// "id,x0,y0|id,x1,y1|......|id,xn,yn"
        /// 比如"0,100,20|2,30,40" 表示找到了两个,第一个,对应的是序号为0的字符串,坐标是(100,20),第二个是序号为2的字符串,坐标(30,40)</returns>
        public string FindStrWithFontEx(int x1, int y1, int x2, int y2, string str, string color, double sim, string fontName, int fontSize, int flag)
        {
            return _dm.FindStrWithFontEx(x1, y1, x2, y2, str, color, sim, fontName, fontSize, flag);
        }

        /// <summary>
        /// 【文字】获取指定字库中指定条目的字库信息.
        /// </summary>
        /// <param name="index">字库序号(0-99)</param>
        /// <param name="fontIndex">字库条目序号(从0开始计数,数值不得超过指定字库的字库上限,具体参考GetDictCount)</param>
        /// <returns>返回字库条目信息. 失败返回空串.</returns>
        public string GetDict(int index, int fontIndex)
        {
            return _dm.GetDict(index, fontIndex);
        }

        /// <summary>
        /// 【文字】获取指定的字库中的字符数量
        /// </summary>
        /// <param name="index">字库序号(0-9)</param>
        /// <returns>字库数量</returns>
        public int GetDictCount(int index)
        {
            return _dm.GetDictCount(index);
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
            return _dm.GetDictInfo(str, fontName, fontSize, flag);
        }

        /// <summary>
        /// 【文字】获取当前使用的字库序号(0-9)
        /// </summary>
        /// <returns>字库序号(0-9)</returns>
        public int GetNowDict()
        {
            return _dm.GetNowDict();
        }

        /// <summary>
        /// 【文字】对插件部分接口的返回值进行解析,并返回ret中的坐标个数
        /// </summary>
        /// <param name="str">部分接口的返回串</param>
        /// <returns>返回ret中的坐标个数</returns>
        public int GetResultCount(string str)
        {
            return _dm.GetResultCount(str);
        }

        /// <summary>
        /// 【文字】对插件部分接口的返回值进行解析,并根据指定的第index个坐标,返回具体的值
        /// </summary>
        /// <param name="str">部分接口的返回串</param>
        /// <param name="index">第几个坐标</param>
        /// <param name="intX">返回X坐标</param>
        /// <param name="intY">返回Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool GetResultPos(string str, int index, out int intX, out int intY)
        {
            bool result = _dm.GetResultPos(str, index, out object x, out object y) == 1;
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
            return _dm.GetWordResultCount(str);
        }

        /// <summary>
        /// 【文字】在使用GetWords进行词组识别以后,可以用此接口进行识别各个词组的坐标
        /// </summary>
        /// <param name="str">GetWords的返回值</param>
        /// <param name="index">第几个坐标</param>
        /// <param name="intX">返回X坐标</param>
        /// <param name="intY">返回Y坐标</param>
        /// <returns>操作是否成功</returns>
        public bool GetWordResultPos(string str, int index, out int intX, out int intY)
        {
            bool result = _dm.GetWordResultPos(str, index, out object x, out object y) == 1;
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【文字】在使用GetWords进行词组识别以后,可以用此接口进行识别各个词组的内容
        /// </summary>
        /// <param name="str">GetWords的返回值</param>
        /// <param name="index">第几个坐标</param>
        /// <returns>返回的第index个词组内容</returns>
        public string GetWordResultStr(string str, int index)
        {
            return _dm.GetWordResultStr(str, index);
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
            return _dm.GetWords(x1, y1, x2, y2, color, sim);
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
            return _dm.GetWordsNoDict(x1, y1, x2, y2, color);
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
            return _dm.Ocr(x1, y1, x2, y2, color, sim);
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
            return _dm.OcrEx(x1, y1, x2, y2, color, sim);
        }

        /// <summary>
        /// 【文字】识别屏幕范围(x1,y1,x2,y2)内符合color_format的字符串,并且相似度为sim,sim取值范围(0.1-1.0),
        /// 这个值越大越精确,越大速度越快,越小速度越慢,请斟酌使用!
        /// 这个函数可以返回识别到的字符串，以及每个字符的坐标.这个同OcrEx,另一种返回形式.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色格式串</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>返回识别到的字符串 格式如  "识别到的信息|x0,y0|…|xn,yn"</returns>
        public string OcrExOne(int x1, int y1, int x2, int y2, string color, double sim = 1.0)
        {
            return _dm.OcrExOne(x1, y1, x2, y2, color, sim);
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
            return _dm.OcrInFile(x1, y1, x2, y2, picName, color, sim);
        }

        /// <summary>
        /// 【文字】保存指定的字库到指定的文件中
        /// </summary>
        /// <param name="index">字库索引序号 取值为0-9对应10个字库</param>
        /// <param name="file">文件名</param>
        /// <returns>操作是否成功</returns>
        public bool SaveDict(int index, string file)
        {
            return _dm.SaveDict(index, file) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的列距,默认列距是1
        /// </summary>
        /// <param name="colGap">文字列距</param>
        /// <returns>操作是否成功</returns>
        public bool SetColGapNoDict(int colGap = 1)
        {
            return _dm.SetColGapNoDict(colGap) == 1;
        }

        /// <summary>
        /// 【文字】设置字库文件
        /// </summary>
        /// <param name="index">字库的序号,取值为0-9,目前最多支持10个字库</param>
        /// <param name="file">字库文件名</param>
        /// <returns>操作是否成功</returns>
        public bool SetDict(int index, string file)
        {
            return _dm.SetDict(index, file) == 1;
        }

        /// <summary>
        /// 【文字】从内存中设置字库.
        /// </summary>
        /// <param name="index">字库的序号,取值为0-99,目前最多支持100个字库</param>
        /// <param name="addr">数据地址</param>
        /// <param name="size">字库长度</param>
        /// <returns>操作是否成功</returns>
        public bool SetDictMem(int index, int addr, int size)
        {
            return _dm.SetDictMem(index, addr, size) == 1;
        }

        /// <summary>
        /// 【文字】设置字库的密码,在SetDict前调用,目前的设计是,所有字库通用一个密码
        /// </summary>
        /// <param name="pwd">字库密码</param>
        /// <returns>操作是否成功</returns>
        public bool SetDictPwd(string pwd)
        {
            return _dm.SetDictPwd(pwd) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在使用文字识别功能前，设定是否开启精准识别。
        /// </summary>
        /// <param name="isExactOcr">是否开启精确识别，默认为开启。</param>
        /// <returns>操作是否成功</returns>
        public bool SetExactOcr(bool isExactOcr = true)
        {
            int exactOcr = isExactOcr ? 1 : 0;
            return _dm.SetExactOcr(exactOcr) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别前,如果待识别区域有多行文字,可以设定列间距,默认的列间距是0,如果根据情况设定,可以提高识别精度。一般不用设定。
        /// </summary>
        /// <param name="minColGap">最小列间距</param>
        /// <returns>操作是否成功</returns>
        public bool SetMinColGap(int minColGap = 1)
        {
            return _dm.SetMinColGap(minColGap) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别前,如果待识别区域有多行文字,可以设定行间距,默认的行间距是1
        /// </summary>
        /// <param name="minRowGap">最小行间距</param>
        /// <returns>操作是否成功</returns>
        public bool SetMinRowGap(int minRowGap = 1)
        {
            return _dm.SetMinRowGap(minRowGap) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的行距,默认行距是1
        /// </summary>
        /// <param name="rowGap">文字行距</param>
        /// <returns>操作是否成功</returns>
        public bool SetRowGapNoDict(int rowGap = 1)
        {
            return _dm.SetRowGapNoDict(rowGap) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别词组前,可设定词组间的间隔,默认的词组间隔是5
        /// </summary>
        /// <param name="wordGap">单词间距</param>
        /// <returns>操作是否成功</returns>
        public bool SetWordGap(int wordGap = 5)
        {
            return _dm.SetWordGap(wordGap) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定词组间的间隔,默认的词组间隔是1
        /// </summary>
        /// <param name="wordGap">单词间距</param>
        /// <returns>操作是否成功</returns>
        public bool SetWordGapNoDict(int wordGap = 1)
        {
            return _dm.SetWordGapNoDict(wordGap) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在识别词组前,可设定文字的平均行高,默认的词组行高是10
        /// </summary>
        /// <param name="lineHeight">行高</param>
        /// <returns>操作是否成功</returns>
        public bool SetWordLineHeight(int lineHeight = 10)
        {
            return _dm.SetWordLineHeight(lineHeight) == 1;
        }

        /// <summary>
        /// 【文字】高级用户使用,在不使用字库进行词组识别前,可设定文字的平均行高,默认的词组行高是10
        /// </summary>
        /// <param name="lineHeight">行高</param>
        /// <returns>操作是否成功</returns>
        public bool SetWordLineHeightNoDict(int lineHeight = 10)
        {
            return _dm.SetWordLineHeightNoDict(lineHeight) == 1;
        }

        /// <summary>
        /// 【文字】表示使用哪个字库文件进行识别(index范围:0-9)
        /// </summary>
        /// <param name="index">字库编号(0-9)</param>
        /// <returns>操作是否成功</returns>
        public bool UseDict(int index)
        {
            return _dm.UseDict(index) == 1;
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
            return _dm.AppendPicAddr(picInfo, address, size);
        }

        /// <summary>
        /// 【图色】把BGR(按键格式)的颜色格式转换为RGB
        /// </summary>
        /// <param name="bgrColor">bgr格式的颜色字符串</param>
        /// <returns>RGB格式的字符串</returns>
        public string BGR2RGB(string bgrColor)
        {
            return _dm.BGR2RGB(bgrColor);
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的图像,保存为file(24位位图)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <returns>操作是否成功</returns>
        public bool Capture(int x1, int y1, int x2, int y2, string file)
        {
            return _dm.Capture(x1, y1, x2, y2, file) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool CaptureGif(int x1, int y1, int x2, int y2, string file, int delay, int mis)
        {
            return _dm.CaptureGif(x1, y1, x2, y2, file, delay, mis) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool CaptureJpg(int x1, int y1, int x2, int y2, string file, int quality)
        {
            return _dm.CaptureJpg(x1, y1, x2, y2, file, quality) == 1;
        }

        /// <summary>
        /// 【图色】抓取指定区域(x1, y1, x2, y2)的图像,保存为file(PNG压缩格式)
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录，当然这里也可以指定全路径名</param>
        /// <returns>操作是否成功</returns>
        public bool CapturePng(int x1, int y1, int x2, int y2, string file)
        {
            return _dm.CapturePng(x1, y1, x2, y2, file) == 1;
        }

        /// <summary>
        /// 【图色】抓取上次操作的图色区域，保存为file(24位位图)
        /// </summary>
        /// <param name="file">保存的文件名,保存的地方一般为SetPath中设置的目录。当然这里也可以指定全路径名</param>
        /// <returns>操作是否成功</returns>
        public bool CapturePre(string file)
        {
            return _dm.CapturePre(file) == 1;
        }

        /// <summary>
        /// 【图色】比较指定坐标点(x,y)的颜色
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="color">颜色字符串,可以支持偏色,多色,例如 "ffffff-202020|000000-000000" 这个表示白色偏色为202020,和黑色偏色为000000。
        /// 颜色最多支持10种颜色组合</param>
        /// <param name="sim">相似度(0.1-1.0)</param>
        /// <returns>操作是否成功</returns>
        public bool CmpColor(int x, int y, string color, double sim = 1.0)
        {
            return _dm.CmpColor(x, y, color, sim) == 1;
        }

        /// <summary>
        /// 【图色】开启图色调试模式，此模式会稍许降低图色和文字识别的速度.默认不开启
        /// </summary>
        /// <param name="enableDebug">是否开启，默认为不开启</param>
        /// <returns>操作是否成功</returns>
        public bool EnableDisplayDebug(bool enableDebug = false)
        {
            int debug = enableDebug ? 1 : 0;
            return _dm.EnableDisplayDebug(debug) == 1;
        }

        /// <summary>
        /// 【图色】允许调用GetColor GetColorBGR GetColorHSV 以及 CmpColor时，以截图的方式来获取颜色
        /// </summary>
        /// <param name="enable">是否允许</param>
        /// <returns>操作是否成功</returns>
        public bool EnableGetColorByCapture(bool enable)
        {
            return _dm.EnableGetColorByCapture(enable ? 1 : 0) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool FindColor(int x1, int y1, int x2, int y2, string color, double sim, int dir, out int intX, out int intY)
        {
            bool result = _dm.FindColor(x1, y1, x2, y2, color, sim, dir, out object x, out object y) == 1;
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定区域内的颜色块,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="count">在宽度为width,高度为height的颜色块中，符合color颜色的最小数量.(注意,这个颜色数量可以在综合工具的二值化区域中看到)</param>
        /// <param name="width">颜色块的宽度</param>
        /// <param name="height">颜色块的高度</param>
        /// <param name="intX">返回X坐标(指向颜色块的左上角)</param>
        /// <param name="intY">返回Y坐标(指向颜色块的左上角)</param>
        /// <returns>操作是否成功</returns>
        public bool FindColorBlock(int x1, int y1, int x2, int y2, string color, double sim, int count, int width, int height, out int intX, out int intY)
        {
            bool result = _dm.FindColorBlock(x1, y1, x2, y2, color, sim, count, width, height, out object x, out object y) == 1;
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定区域内的颜色块,颜色格式"RRGGBB-DRDGDB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <param name="count">在宽度为width,高度为height的颜色块中，符合color颜色的最小数量.(注意,这个颜色数量可以在综合工具的二值化区域中看到)</param>
        /// <param name="width">颜色块的宽度</param>
        /// <param name="height">颜色块的高度</param>
        /// <returns>返回所有颜色块信息的坐标值,然后通过GetResultCount等接口来解析 (由于内存限制,返回的颜色数量最多为1800个左右)</returns>
        public string FindColorBlockEx(int x1, int y1, int x2, int y2, string color, double sim, int count, int width, int height)
        {
            return _dm.FindColorBlockEx(x1, y1, x2, y2, color, sim, count, width, height);
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
            return _dm.FindColorE(x1, y1, x2, y2, color, sim, dir);
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
            return _dm.FindColorEx(x1, y1, x2, y2, color, sim, dir);
        }

        /// <summary>
        /// 【图色】查找指定区域内的所有颜色.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="color">颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020"</param>
        /// <param name="sim">相似度,取值范围0.1-1.0</param>
        /// <returns>是否所有颜色都找到</returns>
        public bool FindMulColor(int x1, int y1, int x2, int y2, string color, double sim)
        {
            return _dm.FindMulColor(x1, y1, x2, y2, color, sim) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool FindMultiColor(int x1,
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
            bool result = _dm.FindMultiColor(x1, y1, x2, y2, firstColor, offsetColor, sim, dir, out object x, out object y) == 1;
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
            return _dm.FindMultiColorE(x1, y1, x2, y2, firstColor, offsetColor, sim, dir);
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
            return _dm.FindMultiColorEx(x1, y1, x2, y2, firstColor, offsetColor, sim, dir);
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
            int result = _dm.FindPic(x1, y1, x2, y2, picName, deltaColor, sim, dir, out object x, out object y);
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
            return _dm.FindPicE(x1, y1, x2, y2, picName, deltaColor, sim, dir);
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
            return _dm.FindPicEx(x1, y1, x2, y2, picName, deltaColor, sim, dir);
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// 这个函数可以查找多个图片,并且返回所有找到的图像的坐标.此函数同FindPicEx.只是返回值不同.
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
        /// <returns>返回的是所有找到的坐标格式如下:"file,x,y| file,x,y..| file,x,y" (图片左上角的坐标)
        /// 比如"1.bmp,100,20|2.bmp,30,40" 表示找到了两个,第一个,对应的图片是1.bmp,坐标是(100,20),第二个是2.bmp,坐标(30,40)</returns>
        public string FindPicExS(int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir)
        {
            return _dm.FindPicExS(x1, y1, x2, y2, picName, deltaColor, sim, dir);
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picInfo">图片数据地址集合. 格式为"地址1,长度1|地址2,长度2.....|地址n,长度n". 可以用AppendPicAddr来组合.
        /// 地址表示24位位图资源在内存中的首地址，用十进制的数值表示长度表示位图资源在内存中的长度，用十进制数值表示.</param>
        /// <param name="deltaColor">颜色色偏比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
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
        public int FindPicMem(int x1, int y1, int x2, int y2, string picInfo, string deltaColor, double sim, int dir, out int intX, out int intY)
        {
            int result = _dm.FindPicMem(x1, y1, x2, y2, picInfo, deltaColor, sim, dir, out object x, out object y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// 这个函数可以查找多个图片,只返回第一个找到的X Y坐标.这个函数要求图片是数据地址.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picInfo">图片数据地址集合. 格式为"地址1,长度1|地址2,长度2.....|地址n,长度n". 可以用AppendPicAddr来组合.
        /// 地址表示24位位图资源在内存中的首地址，用十进制的数值表示长度表示位图资源在内存中的长度，用十进制数值表示.</param>
        /// <param name="deltaColor">颜色色偏比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
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
        public string FindPicMemE(int x1, int y1, int x2, int y2, string picInfo, string deltaColor, double sim, int dir)
        {
            return _dm.FindPicMemE(x1, y1, x2, y2, picInfo, deltaColor, sim, dir);
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// 这个函数可以查找多个图片,并且返回所有找到的图像的坐标.这个函数要求图片是数据地址.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picInfo">图片数据地址集合. 格式为"地址1,长度1|地址2,长度2.....|地址n,长度n". 可以用AppendPicAddr来组合.
        /// 地址表示24位位图资源在内存中的首地址，用十进制的数值表示长度表示位图资源在内存中的长度，用十进制数值表示.</param>
        /// <param name="deltaColor">颜色色偏比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
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
        /// 比如"0,100,20|2,30,40" 表示找到了两个,第一个,对应的图片是图像序号为0的图片,坐标是(100,20),第二个是序号为2的图片,坐标(30,40)</returns>
        public string FindPicMemEx(int x1, int y1, int x2, int y2, string picInfo, string deltaColor, double sim, int dir)
        {
            return _dm.FindPicMemEx(x1, y1, x2, y2, picInfo, deltaColor, sim, dir);
        }

        /// <summary>
        /// 【图色】查找指定区域内的图片,位图必须是24位色格式,支持透明色,当图像上下左右4个顶点的颜色一样时,则这个颜色将作为透明色处理.
        /// 这个函数可以查找多个图片,只返回第一个找到的X Y坐标.此函数同FindPic.只是返回值不同.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="picName">图片名,可以是多个图片,比如"test.bmp|test2.bmp|test3.bmp"</param>
        /// <param name="deltaColor">颜色色偏比如"203040" 表示RGB的色偏分别是20 30 40 (这里是16进制表示)</param>
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
        /// <returns>返回找到的图片的文件名. 没找到返回长度为0的字符串.</returns>
        public string FindPicS(int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir, out int intX, out int intY)
        {
            string result = _dm.FindPicS(x1, y1, x2, y2, picName, deltaColor, sim, dir, out var x, out var y);
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定的形状. 形状的描述同按键的抓抓. 具体可以参考按键的抓抓. 和按键的语法不同，需要用大漠综合工具的颜色转换.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="offsetColor">坐标偏移描述 可以支持任意多个点 格式和按键自带的Color插件意义相同
        /// 格式为"x1|y1|e1,……xn|yn|en"比如"1|3|1,-5|-3|0"等任意组合都可以</param>
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
        /// <returns>是否找到</returns>
        public bool FindShape(int x1, int y1, int x2, int y2, string offsetColor, double sim, int dir, out int intX, out int intY)
        {
            bool result = _dm.FindShape(x1, y1, x2, y2, offsetColor, sim, dir, out object x, out object y) == 1;
            intX = (int)x;
            intY = (int)y;
            return result;
        }

        /// <summary>
        /// 【图色】查找指定的形状. 形状的描述同按键的抓抓. 具体可以参考按键的抓抓. 和按键的语法不同，需要用大漠综合工具的颜色转换.
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="offsetColor">坐标偏移描述 可以支持任意多个点 格式和按键自带的Color插件意义相同
        /// 格式为"x1|y1|e1,……xn|yn|en"比如"1|3|1,-5|-3|0"等任意组合都可以</param>
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
        public string FindShapeE(int x1, int y1, int x2, int y2, string offsetColor, double sim, int dir)
        {
            return _dm.FindShapeE(x1, y1, x2, y2, offsetColor, sim, dir);
        }

        /// <summary>
        /// 【图色】查找所有指定的形状的坐标. 形状的描述同按键的抓抓. 具体可以参考按键的抓抓. 和按键的语法不同，需要用大漠综合工具的颜色转换
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="offsetColor">坐标偏移描述 可以支持任意多个点 格式和按键自带的Color插件意义相同
        /// 格式为"x1|y1|e1,……xn|yn|en"比如"1|3|1,-5|-3|0"等任意组合都可以</param>
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
        /// <returns>返回所有形状的坐标值,然后通过GetResultCount等接口来解析(由于内存限制,返回的坐标数量最多为1800个左右)</returns>
        public string FindShapeEx(int x1, int y1, int x2, int y2, string offsetColor, double sim, int dir)
        {
            return _dm.FindShapeEx(x1, y1, x2, y2, offsetColor, sim, dir);
        }

        /// <summary>
        /// 【图色】释放指定的图片,此函数不必要调用,除非你想节省内存
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp|2.bmp|3.bmp" 等,可以使用通配符,比如
        /// "*.bmp" 这个对应了所有的bmp文件
        /// "a?c*.bmp" 这个代表了所有第一个字母是a 第三个字母是c 第二个字母任意的所有bmp文件
        /// "abc???.bmp|1.bmp|aa??.bmp" 可以这样任意组合</param>
        /// <returns>操作是否成功</returns>
        public bool FreePic(string picName)
        {
            return _dm.FreePic(picName) == 1;
        }

        /// <summary>
        /// 【图色】获取范围(x1,y1,x2,y2)颜色的均值,返回格式"H.S.V"
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <returns>颜色字符串</returns>
        public string GetAveHSV(int x1, int y1, int x2, int y2)
        {
            return _dm.GetAveHSV(x1, y1, x2, y2);
        }

        /// <summary>
        /// 【图色】获取范围(x1,y1,x2,y2)颜色的均值,返回格式"RRGGBB"
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <returns>颜色字符串</returns>
        public string GetAveRGB(int x1, int y1, int x2, int y2)
        {
            return _dm.GetAveRGB(x1, y1, x2, y2);
        }

        /// <summary>
        /// 【图色】获取(x,y)的颜色,颜色返回格式"RRGGBB",注意,和按键的颜色格式相反
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColor(int x, int y)
        {
            return _dm.GetColor(x, y);
        }

        /// <summary>
        /// 【图色】获取(x,y)的颜色,颜色返回格式"BBGGRR"
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColorBGR(int x, int y)
        {
            return _dm.GetColorBGR(x, y);
        }

        /// <summary>
        /// 【图色】获取(x,y)的HSV颜色,颜色返回格式"H.S.V"
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>颜色字符串(注意这里都是小写字符，和工具相匹配)</returns>
        public string GetColorHSV(int x, int y)
        {
            return _dm.GetColorHSV(x, y);
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
            return _dm.GetColorNum(x1, y1, x2, y2, color, sim);
        }

        /// <summary>
        /// 【图色】获取指定图片的尺寸，如果指定的图片已经被加入缓存，则从缓存中获取信息.此接口也会把此图片加入缓存
        /// </summary>
        /// <param name="picName">文件名 比如"1.bmp"</param>
        /// <returns>形式如 "w,h" 比如"30,20"</returns>
        public string GetPicSize(string picName)
        {
            return _dm.GetPicSize(picName);
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
            return _dm.GetScreenData(x1, y1, x2, y2);
        }

        /// <summary>
        /// 【图色】转换图片格式为24位BMP格式
        /// </summary>
        /// <param name="picName">要转换的图片名</param>
        /// <param name="bmpName">要保存的BMP图片名</param>
        /// <returns>操作是否成功</returns>
        public bool ImageToBmp(string picName, string bmpName)
        {
            return _dm.ImageToBmp(picName, bmpName) == 1;
        }

        /// <summary>
        /// 【图色】判断指定的区域，在指定的时间内(秒),图像数据是否一直不变.(卡屏).
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="mis">需要等待的时间,单位是秒</param>
        /// <returns>false.图像有变化，true.图像无变化</returns>
        public bool IsDisplayDead(int x1, int y1, int x2, int y2, int mis)
        {
            return _dm.IsDisplayDead(x1, y1, x2, y2, mis) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool LoadPic(string picName)
        {
            return _dm.LoadPic(picName) == 1;
        }

        /// <summary>
        /// 【图色】预先加载指定的图片,这样在操作任何和图片相关的函数时,将省去了加载图片的时间。调用此函数后,没必要一定要调用FreePic,插件自己会自动释放.
        /// </summary>
        /// <param name="addr">BMP图像首地址.(完整的BMP图像，不是经过解析的. 和BMP文件里的内容一致)</param>
        /// <param name="size">BMP图像大小.(和BMP文件大小一致)</param>
        /// <param name="picName">文件名,指定这个地址对应的图片名. 用于找图时使用</param>
        /// <returns>操作是否成功</returns>
        public bool LoadPicByte(int addr, int size, string picName)
        {
            return _dm.LoadPicByte(addr, size, picName) == 1;
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
            return _dm.MatchPicName(picName);
        }

        /// <summary>
        /// 【图色】把RGB的颜色格式转换为BGR(按键格式)
        /// </summary>
        /// <param name="rgbColor">rgb格式的颜色字符串</param>
        /// <returns>BGR格式的字符串</returns>
        public string RGB2BGR(string rgbColor)
        {
            return _dm.RGB2BGR(rgbColor);
        }

        /// <summary>
        /// 【图色】设置图色,以及文字识别时,需要排除的区域.(支持所有图色接口,以及文字相关接口,但对单点取色,或者单点颜色比较的接口不支持)
        /// </summary>
        /// <param name="mode">模式,取值如下:0: 添加排除区域 1: 设置排除区域的颜色,默认颜色是FF00FF(此接口的原理是把排除区域设置成此颜色, 这样就可以让这块区域实效) 2: 请空排除区域</param>
        /// <param name="info">根据mode的取值来决定. 当mode为0时,此参数指添加的区域,可以多个区域,用"|"相连.格式为"x1,y1,x2,y2|....."
        /// 当mode为1时,此参数为排除区域的颜色,"RRGGBB" 当mode为2时,此参数无效</param>
        /// <returns>操作是否成功</returns>
        public bool SetExcludeRegion(int mode, string info)
        {
            return _dm.SetExcludeRegion(mode, info) == 1;
        }

        /// <summary>
        /// 【图色】设置图片密码，如果图片本身没有加密，那么此设置不影响不加密的图片，一样正常使用。
        /// </summary>
        /// <param name="pwd">图片密码</param>
        /// <returns>操作是否成功</returns>
        public bool SetPicPwd(string pwd)
        {
            return _dm.SetPicPwd(pwd) == 1;
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
            return _dm.DoubleToData(value);
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
            return _dm.FindData(hwnd, addrRange, data);
        }

        /// <summary>
        /// 【内存】搜索指定的二进制数据
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="data">要搜索的二进制数据 以字符串的形式描述 比如"00 01 23 45 67 86 ab ce f1"等</param>
        /// <param name="step">搜索步长</param>
        /// <param name="multiThread">表示是否开启多线程查找.  0不开启，1开启.开启多线程查找速度较快，但会耗费较多CPU资源.不开启速度较慢，但节省CPU</param>
        /// <param name="mode">1 表示开启快速扫描(略过只读内存)  0表示所有内存类型全部扫描.</param>
        /// <returns>返回搜索到的地址集合，地址格式如下: "addr1|addr2|addr3…|addrn"  比如"400050|423435|453430"  如果要想知道函数是否执行成功，请查看GetLastError函数</returns>
        public string FindDataEx(int hwnd, string addrRange, string data, int step, int multiThread, int mode)
        {
            return _dm.FindDataEx(hwnd, addrRange, data, step, multiThread, mode);
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
            return _dm.FindDouble(hwnd, addrRange, minValue, maxValue);
        }

        /// <summary>
        /// 【内存】搜索指定的双精度浮点数.
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="minValue">搜索的双精度数值最小值</param>
        /// <param name="maxValue">搜索的双精度数值最大值</param>
        /// <param name="step">搜索步长</param>
        /// <param name="multiThread">表示是否开启多线程查找.  0不开启，1开启.开启多线程查找速度较快，但会耗费较多CPU资源.不开启速度较慢，但节省CPU</param>
        /// <param name="mode">1 表示开启快速扫描(略过只读内存)  0表示所有内存类型全部扫描.</param>
        /// <returns>返回搜索到的地址集合，地址格式如下: "addr1|addr2|addr3…|addrn" 比如"400050|423435|453430"</returns>
        public string FindDoubleEx(int hwnd, string addrRange, double minValue, double maxValue, int step, int multiThread, int mode)
        {
            return _dm.FindDoubleEx(hwnd, addrRange, minValue, maxValue, step, multiThread, mode);
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
            return _dm.FindFloat(hwnd, addrRange, minValue, maxValue);
        }

        /// <summary>
        /// 【内存】搜索指定的单精度浮点数.
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄</param>
        /// <param name="addrRange">指定搜索的地址集合，字符串类型，这个地方可以是上次FindXXX的返回地址集合,可以进行二次搜索.(类似CE的再次扫描)
        /// 如果要进行地址范围搜索，那么这个值为的形如如下(类似于CE的新搜索)
        /// "00400000-7FFFFFFF" "80000000-BFFFFFFF" "00000000-FFFFFFFF" 等.</param>
        /// <param name="minValue">搜索的单精度数值最小值</param>
        /// <param name="maxValue">搜索的单精度数值最大值</param>
        /// <param name="step">搜索步长</param>
        /// <param name="multiThread">表示是否开启多线程查找.  0不开启，1开启.开启多线程查找速度较快，但会耗费较多CPU资源.不开启速度较慢，但节省CPU</param>
        /// <param name="mode">1 表示开启快速扫描(略过只读内存)  0表示所有内存类型全部扫描.</param>
        /// <returns>返回搜索到的地址集合，地址格式如下: "addr1|addr2|addr3…|addrn" 比如"400050|423435|453430"</returns>
        public string FindFloatEx(int hwnd, string addrRange, float minValue, float maxValue, int step, int multiThread, int mode)
        {
            return _dm.FindFloatEx(hwnd, addrRange, minValue, maxValue, step, multiThread, mode);
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
            return _dm.FindInt(hwnd, addrRange, minValue, maxValue, type);
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
        /// <param name="type">搜索的整数类型,取值如下： 0. 32位，1. 16位，2. 8位</param> 
        /// <param name="step">搜索步长</param>
        /// <param name="multiThread">表示是否开启多线程查找.  0不开启，1开启.开启多线程查找速度较快，但会耗费较多CPU资源.不开启速度较慢，但节省CPU</param>
        /// <param name="mode">1 表示开启快速扫描(略过只读内存)  0表示所有内存类型全部扫描.</param>
        /// <returns>返回搜索到的地址集合，地址格式如下: "addr1|addr2|addr3…|addrn" 比如"400050|423435|453430"</returns>
        public string FindIntEx(int hwnd, string addrRange, int minValue, int maxValue, int type, int step, int multiThread, int mode)
        {
            return _dm.FindIntEx(hwnd, addrRange, minValue, maxValue, type, step, multiThread, mode);
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
            return _dm.FindString(hwnd, addrRange, value, type);
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
        /// <param name="step">搜索步长</param>
        /// <param name="multiThread">表示是否开启多线程查找.  0不开启，1开启.开启多线程查找速度较快，但会耗费较多CPU资源.不开启速度较慢，但节省CPU</param>
        /// <param name="mode">1 表示开启快速扫描(略过只读内存)  0表示所有内存类型全部扫描.</param>
        /// <returns>返回搜索到的地址集合，地址格式如下: "addr1|addr2|addr3…|addrn" 比如"400050|423435|453430"</returns>
        public string FindStringEx(int hwnd, string addrRange, string value, int type, int step, int multiThread, int mode)
        {
            return _dm.FindStringEx(hwnd, addrRange, value, type, step, multiThread, mode);
        }

        /// <summary>
        /// 【内存】把单精度浮点数转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的单精度浮点数</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string FloatToData(float value)
        {
            return _dm.FloatToData(value);
        }

        /// <summary>
        /// 【内存】释放指定进程的不常用内存.
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <returns>操作是否成功</returns>
        public bool FreeProcessMemory(int hwnd)
        {
            return _dm.FreeProcessMemory(hwnd) == 1;
        }

        /// <summary>
        /// 【内存】获取指定窗口所在进程的启动命令行
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <returns>读取到的启动命令行</returns>
        public string GetCommandLine(int hwnd)
        {
            return _dm.GetCommandLine(hwnd);
        }

        /// <summary>
        /// 【内存】根据指定的窗口句柄，来获取对应窗口句柄进程下的指定模块的基址
        /// </summary>
        /// <param name="hwnd">指定读取的窗口句柄</param>
        /// <param name="moduleName">模块名</param>
        /// <returns>模块的基址</returns>
        public int GetModuleBaseAddr(int hwnd, string moduleName)
        {
            return _dm.GetModuleBaseAddr(hwnd, moduleName);
        }

        /// <summary>
        /// 【内存】把整数转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的整数</param>
        /// <param name="type">0. 4字节整形数（一般都选这个），1. 2字节整形数，2. 1字节整形数</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string IntToData(int value, int type)
        {
            return _dm.IntToData(value, type);
        }

        /// <summary>
        /// 【内存】根据指定pid打开进程，并返回进程句柄.
        /// </summary>
        /// <param name="pid">进程pid</param>
        /// <return>进程句柄, 可用于进程相关操作(读写操作等),记得操作完成以后，自己调用CloseHandle关闭句柄</return>>
        public int OpenProcess(int pid)
        {
            return _dm.OpenProcess(pid);
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
            return _dm.ReadData(hwnd, address, length);
        }

        /// <summary>
        /// 【内存】读取指定地址的二进制数据
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="len">二进制数据的长度</param>
        /// <returns>读取到的数值,以16进制表示的字符串 每个字节以空格相隔 比如"12 34 56 78 ab cd ef"</returns>
        public string ReadDataAddr(int hwnd, int addr, int len)
        {
            return _dm.ReadDataAddr(hwnd, addr, len);
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
            return _dm.ReadDouble(hwnd, address);
        }

        /// <summary>
        /// 【内存】读取指定地址的双精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <returns>读取到的数值</returns>
        public double ReadDoubleAddr(int hwnd, int addr)
        {
            return _dm.ReadDoubleAddr(hwnd, addr);
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
            return _dm.ReadFloat(hwnd, address);
        }

        /// <summary>
        /// 【内存】读取指定地址的单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <returns>读取到的数值</returns>
        public float ReadFloatAddr(int hwnd, int addr)
        {
            return _dm.ReadFloatAddr(hwnd, addr);
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
            return _dm.ReadInt(hwnd, address, type);
        }

        /// <summary>
        /// 【内存】读取指定地址的单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="type">整数类型,取值如下 0 : 32位 1 : 16 位 2 : 8位 3 : 64位</param>
        /// <returns>读取到的数值</returns>
        public int ReadIntAddr(int hwnd, int addr, int type)
        {
            return _dm.ReadIntAddr(hwnd, addr, type);
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
            return _dm.ReadString(hwnd, address, type, length);
        }

        /// <summary>
        /// 【内存】读取指定地址的单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="type">字符串类型,取值如下 0 : GBK字符串 1 : Unicode字符串 </param>
        /// <param name="len">需要读取的字节数目.如果为0，则自动判定字符串长度</param>
        /// <returns>读取到的数值</returns>
        public string ReadStringAddr(int hwnd, int addr, int type, int len)
        {
            return _dm.ReadStringAddr(hwnd, addr, type, len);
        }

        /// <summary>
        /// 【内存】设置是否把所有内存查找接口的结果保存入指定文件
        /// </summary>
        /// <param name="file">设置要保存的搜索结果文件名. 如果为空字符串表示取消此功能</param>
        /// <returns>操作是否成功</returns>
        public bool SetMemoryFindResultToFile(string file)
        {
            return _dm.SetMemoryFindResultToFile(file) == 1;
        }

        /// <summary>
        /// 【内存】设置是否把所有内存接口函数中的窗口句柄当作进程ID,以支持直接以进程ID来使用内存接口.
        /// </summary>
        /// <param name="enable">是否打开</param>
        /// <returns>操作是否成功</returns>
        public bool SetMemoryHwndAsProcessId(bool enable)
        {
            return _dm.SetMemoryHwndAsProcessId(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【内存】把字符串转换成二进制形式
        /// </summary>
        /// <param name="value">需要转化的字符串</param>
        /// <param name="type">0.返回Ascii表达的字符串，1.返回Unicode表达的字符串</param>
        /// <returns>字符串形式表达的二进制数据. 可以用于WriteData FindData FindDataEx等接口</returns>
        public string StringToData(string value, int type)
        {
            return _dm.StringToData(value, type);
        }

        /// <summary>
        /// 【内存】根据指定的PID，强制结束进程.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool TerminateProcess(int pid)
        {
            return _dm.TerminateProcess(pid) == 1;
        }

        /// <summary>
        /// 【内存】在指定的窗口所在进程分配一段内存
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">预期的分配地址。 如果是0表示自动分配，否则就尝试在此地址上分配内存.</param>
        /// <param name="size">需要分配的内存大小.</param>
        /// <param name="type">需要分配的内存类型，取值如下: 0 : 可读可写可执行 1 : 可读可执行，不可写 2 : 可读可写,不可执行 </param>
        /// <returns>分配的内存地址，如果是0表示分配失败.</returns>
        public int VirtualAllocEx(int hwnd, int addr, int size, int type)
        {
            return _dm.VirtualAllocEx(hwnd, addr, size, type);
        }

        /// <summary>
        /// 【内存】释放用VirtualAllocEx分配的内存
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">VirtualAllocEx返回的地址</param>
        /// <returns>操作是否成功</returns>
        public bool VirtualFreeEx(int hwnd, int addr)
        {
            return _dm.VirtualFreeEx(hwnd, addr) == 1;
        }

        /// <summary>
        /// 【内存】修改指定的窗口所在进程的地址的读写属性,修改为可读可写可执行
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">需要修改的地址</param>
        /// <param name="size">需要修改的地址大小.</param>
        /// <param name="type">修改的地址读写属性类型，取值如下: 0 : 可读可写可执行,此时old_protect参数无效 1 : 修改为old_protect指定的读写属性 </param>
        /// <param name="oldProtect">指定的读写属性 </param>
        /// <returns>修改是否成功</returns>
        public bool VirtualProtectEx(int hwnd, int addr, int size, int type, int oldProtect)
        {
            return _dm.VirtualProtectEx(hwnd, addr, size, type, oldProtect) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool WriteData(int hwnd, string address, string data)
        {
            return _dm.WriteData(hwnd, address, data) == 1;
        }

        /// <summary>
        /// 【内存】对指定地址写入二进制数据
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool WriteDataAddr(int hwnd, int addr, string data)
        {
            return _dm.WriteDataAddr(hwnd, addr, data) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool WriteDouble(int hwnd, string address, double value)
        {
            return _dm.WriteDouble(hwnd, address, value) == 1;
        }

        /// <summary>
        /// 【内存】对指定地址写入双精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="value">双精度浮点数</param>
        /// <returns>操作是否成功</returns>
        public bool WriteDoubleAddr(int hwnd, int addr, double value)
        {
            return _dm.WriteDoubleAddr(hwnd, addr, value) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool WriteFloat(int hwnd, string address, float value)
        {
            return _dm.WriteFloat(hwnd, address, value) == 1;
        }

        /// <summary>
        /// 【内存】对指定地址写入单精度浮点数
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="value">单精度浮点数</param>
        /// <returns>操作是否成功</returns>
        public bool WriteFloatAddr(int hwnd, int addr, float value)
        {
            return _dm.WriteFloatAddr(hwnd, addr, value) == 1;
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
        /// <returns>操作是否成功</returns>
        public int WriteInt(int hwnd, string address, int type, int value)
        {
            return _dm.WriteInt(hwnd, address, type, value);
        }

        /// <summary>
        /// 【内存】对指定地址写入整数数值，类型可以是8位，16位 32位 或者64位
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="type">整数类型,取值如下 0 : 32位 1 : 16 位 2 : 8位 3 : 64位</param>
        /// <param name="value">整形数值</param>
        /// <returns>操作是否成功</returns>
        public bool WriteIntAddr(int hwnd, int addr, int type, int value)
        {
            return _dm.WriteIntAddr(hwnd, addr, type, value) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool WriteString(int hwnd, string address, int type, string value)
        {
            return _dm.WriteString(hwnd, address, type, value) == 1;
        }

        /// <summary>
        /// 【内存】对指定地址写入字符串，可以是Ascii字符串或者是Unicode字符串
        /// </summary>
        /// <param name="hwnd">指定搜索的窗口句柄或者进程ID.  默认是窗口句柄. 如果要指定为进程ID,需要调用SetMemoryHwndAsProcessId.</param>
        /// <param name="addr">地址</param>
        /// <param name="type">字符串类型,取值如下 0 : Ascii字符串 1 : Unicode字符串</param>
        /// <param name="value">整形数值</param>
        /// <returns>操作是否成功</returns>
        public bool WriteStringAddr(int hwnd, int addr, int type, string value)
        {
            return _dm.WriteStringAddr(hwnd, addr, type, value) == 1;
        }

        #endregion

        #region 系统

        /// <summary>
        /// 【系统】蜂鸣器
        /// </summary>
        /// <param name="fre">频率</param>
        /// <param name="delay">时长</param>
        /// <returns>操作是否成功</returns>
        public bool Beep(int fre, int delay)
        {
            return _dm.Beep(fre, delay) == 1;
        }

        /// <summary>
        /// 【系统】检测当前系统是否有开启屏幕字体平滑.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool CheckFontSmooth()
        {
            return _dm.CheckFontSmooth() == 1;
        }

        /// <summary>
        /// 【系统】检测当前系统是否有开启UAC(用户账户控制).
        /// </summary>
        /// <returns>是否开启</returns>
        public bool CheckUAC()
        {
            return _dm.CheckUAC() == 1;
        }

        /// <summary>
        /// 【系统】设置指定毫秒数的延时
        /// </summary>
        /// <param name="mis">延时毫秒数</param>
        /// <returns>操作是否成功</returns>
        public bool Delay(int mis)
        {
            return _dm.delay(mis) == 1;
        }

        /// <summary>
        /// 【系统】延时指定范围内随机毫秒,过程中不阻塞UI操作. 一般高级语言使用.按键用不到.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool Delays(int misMin, int misMax)
        {
            return _dm.Delays(misMin, misMax) == 1;
        }

        /// <summary>
        /// 【系统】关闭当前系统屏幕字体平滑.同时关闭系统的ClearType功能.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool DisableFontSmooth()
        {
            return _dm.DisableFontSmooth() == 1;
        }

        /// <summary>
        /// 【系统】关闭电源管理，不会进入睡眠
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool DisablePowerSave()
        {
            return _dm.DisablePowerSave() == 1;
        }

        /// <summary>
        /// 【系统】关闭屏幕保护
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool DisableScreenSave()
        {
            return _dm.DisablePowerSave() == 1;
        }

        /// <summary>
        /// 【系统】开启当前系统屏幕字体平滑.同时开启系统的ClearType功能.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableFontSmooth()
        {
            return _dm.EnableFontSmooth() == 1;
        }

        /// <summary>
        /// 【系统】退出系统(注销 重启 关机)
        /// </summary>
        /// <param name="type">0.注销系统，1.关机，2.重新启动</param>
        /// <returns>操作是否成功</returns>
        public bool ExitOs(int type)
        {
            return _dm.ExitOs(type) == 1;
        }

        /// <summary>
        /// 【系统】获取剪贴板的内容
        /// </summary>
        /// <returns>以字符串表示的剪贴板内容</returns>
        public string GetClipboard()
        {
            return _dm.GetClipboard();
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
            return _dm.GetDir(type);
        }

        /// <summary>
        /// 【系统】获取本机的硬盘序列号.支持ide scsi硬盘. 要求调用进程必须有管理员权限. 否则返回空串
        /// </summary>
        /// <returns></returns>
        public string GetDiskSerial()
        {
            return _dm.GetDiskSerial();
        }

        /// <summary>
        /// 【系统】获取本机的显卡信息
        /// </summary>
        public string GetDisplayInfo()
        {
            return _dm.GetDisplayInfo();
        }

        /// <summary>
        /// 【系统】获取本机的机器码.(带网卡). 此机器码用于插件网站后台. 要求调用进程必须有管理员权限. 否则返回空串.
        /// </summary>
        public string GetMachineCode()
        {
            return _dm.GetMachineCode();
        }

        /// <summary>
        /// 【系统】获取本机的机器码.(不带网卡) 要求调用进程必须有管理员权限. 否则返回空串
        /// </summary>
        /// <returns>字符串表达的机器机器码</returns>
        public string GetMachineCodeNoMac()
        {
            return _dm.GetMachineCodeNoMac();
        }

        /// <summary>
        /// 【系统】从网络获取当前北京时间，如获取失败返回DateTime.MinValue
        /// </summary>
        /// <returns>网络时间，如获取失败返回DateTime.MinValue</returns>
        public DateTime GetNetTime()
        {
            string result = _dm.GetNetTime();
            if (string.IsNullOrEmpty(result) || result == "0000-00-00 00:00:00")
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(result);
        }

        /// <summary>
        /// 【系统】根据指定时间服务器IP,从网络获取当前北京时间.
        /// </summary>
        /// <param name="ip">IP或者域名,并且支持多个IP或者域名连接</param>
        /// <returns>时间格式. 和now返回一致. 比如"2001-11-01 23:14:08"</returns>
        public string GetNetTimeByIp(string ip)
        {
            return _dm.GetNetTimeByIp(ip);
        }

        /// <summary>
        /// 【系统】得到操作系统的类型
        /// </summary>
        /// <returns>0 : win95/98/me/nt4.0，1 : xp/2000，2 : 2003，3 : win7/vista/2008</returns>
        public int GetOSType()
        {
            return _dm.GetOsType();
        }

        /// <summary>
        /// 【系统】获取屏幕的色深
        /// </summary>
        /// <returns>返回系统颜色深度.(16或者32等)</returns>
        public int GetScreenDepth()
        {
            return _dm.GetScreenDepth();
        }

        /// <summary>
        /// 【系统】获取屏幕的高度
        /// </summary>
        /// <returns>返回屏幕的高度</returns>
        public int GetScreenHeight()
        {
            return _dm.GetScreenHeight();
        }

        /// <summary>
        /// 【系统】获取屏幕的宽度
        /// </summary>
        /// <returns>返回屏幕的宽度</returns>
        public int GetScreenWidth()
        {
            return _dm.GetScreenWidth();
        }

        /// <summary>
        /// 【系统】获取当前系统从开机到现在所经历过的时间，单位是毫秒
        /// </summary>
        /// <returns></returns>
        public long GetTimeSpanFromOsStarted()
        {
            return _dm.GetTime();
        }

        /// <summary>
        /// 【系统】判断当前系统是否是64位操作系统
        /// </summary>
        /// <returns>是否是64位系统</returns>
        public bool Is64Bit()
        {
            return _dm.Is64Bit() == 1;
        }

        /// <summary>
        /// 【系统】播放指定的MP3或者wav文件
        /// </summary>
        /// <param name="file">指定的音乐文件，可以采用文件名或者绝对路径的形式</param>
        /// <returns>0 : 失败，非0表示当前播放的ID。可以用Stop来控制播放结束</returns>
        public int MediaPlay(string file)
        {
            return _dm.Play(file);
        }

        /// <summary>
        /// 【系统】停止指定的音乐
        /// </summary>
        /// <param name="id">Play返回的播放id</param>
        /// <returns>操作是否成功</returns>
        public bool MediaStop(int id)
        {
            return _dm.Stop(id) == 1;
        }

        /// <summary>
        /// 【系统】运行指定的应用程序
        /// </summary>
        /// <param name="appPath">指定的可执行程序全路径</param>
        /// <param name="mode">0：普通模式，1：加强模式</param>
        /// <returns>操作是否成功</returns>
        public bool RunApp(string appPath, int mode)
        {
            return _dm.RunApp(appPath, mode) == 1;
        }

        /// <summary>
        /// 【系统】获取剪贴板的内容
        /// </summary>
        /// <param name="value">以字符串表示的剪贴板内容</param>
        /// <returns>操作是否成功</returns>
        public bool SetClipboard(string value)
        {
            return _dm.SetClipboard(value) == 1;
        }

        /// <summary>
        /// 【系统】设置当前系统的硬件加速级别
        /// </summary>
        /// <param name="level">取值范围为0-5.  0表示关闭硬件加速。5表示完全打开硬件加速</param>
        /// <returns>操作是否成功</returns>
        public bool SetDisplayAcceler(int level)
        {
            return _dm.SetDisplayAcceler(level) == 1;
        }

        /// <summary>
        /// 【系统】设置系统的分辨率 系统色深
        /// </summary>
        /// <param name="width">屏幕宽度</param>
        /// <param name="height">屏幕高度</param>
        /// <param name="depth">系统色深</param>
        /// <returns>操作是否成功</returns>
        public bool SetScreen(int width, int height, int depth)
        {
            return _dm.SetScreen(width, height, depth) == 1;
        }

        /// <summary>
        /// 【系统】设置当前系统的UAC(用户账户控制).
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <returns>操作是否成功</returns>
        public bool SetUAC(bool enable)
        {
            return _dm.SetUAC(enable ? 1 : 0) == 1;
        }

        #endregion

        #region 后台设置

        /// <summary>
        /// 【后台】绑定指定的窗口,并指定这个窗口的屏幕颜色获取方式,鼠标仿真模式,键盘仿真模式,以及模式设定,高级用户可以参考BindWindowEx更加灵活强大
        /// </summary>
        /// <param name="hwnd">指定的窗口句柄</param>
        /// <param name="display">窗口绑定屏幕颜色获取方式</param>
        /// <param name="mouse">窗口绑定鼠标仿真模式</param>
        /// <param name="keypad">窗口绑定键盘仿真模式</param>
        /// <param name="mode">窗口绑定模式</param>
        /// <returns>操作是否成功</returns>
        public bool BindWindow(int hwnd, DmBindDisplay display, DmBindMouse mouse, DmBindKeypad keypad, DmBindMode mode)
        {
            return _dm.BindWindow(hwnd, display.ToString(), mouse.ToString(), keypad.ToString(), (int)mode) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool BindWindowEx(int hwnd, string display, string mouse, string keypad, string @public, DmBindMode mode)
        {
            return _dm.BindWindowEx(hwnd, display, mouse, keypad, @public, (int)mode) == 1;
        }

        /// <summary>
        /// 【后台】降低目标窗口所在进程的CPU占用。
        /// 注意: 此接口必须在绑定窗口成功以后调用，而且必须保证目标窗口可以支持dx.graphic.3d方式截图，否则降低CPU无效。
        /// 因为降低CPU是通过降低窗口刷新速度来实现，所以注意，开启此功能以后会导致窗口刷新速度变慢
        /// </summary>
        /// <param name="rate">取值范围0到100   取值为0 表示关闭CPU优化. 这个值越大表示降低CPU效果越好</param>
        /// <returns>操作是否成功</returns>
        public bool DownCpu(int rate)
        {
            return _dm.DownCpu(rate) == 1;
        }

        /// <summary>
        /// 【后台】设置是否暂时关闭或者开启后台功能. 默认是开启
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableBind(bool enable)
        {
            return _dm.EnableBind(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】设置是否开启后台假激活功能. 默认是关闭. 一般用不到. 除非有人有特殊需求.
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableFakeActive(bool enable)
        {
            return _dm.EnableFakeActive(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】设置是否允许绑定窗口所在进程的输入法，此方法需在绑定之后调用
        /// </summary>
        /// <param name="enable">是否允许</param>
        /// <returns>操作是否成功</returns>
        public bool EnableIme(bool enable)
        {
            return _dm.EnableIme(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】是否在使用dx键盘时开启windows消息.默认开启
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableKeypadMsg(bool enable)
        {
            return _dm.EnableKeypadMsg(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】键盘消息发送补丁. 默认是关闭
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableKeypadPatch(bool enable)
        {
            return _dm.EnableKeypadPatch(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】键盘消息采用同步发送模式.默认异步
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="timeout">单位是毫秒,表示同步等待的最大时间</param>
        /// <returns>操作是否成功</returns>
        public bool EnableKeypadSync(bool enable, int timeout)
        {
            return _dm.EnableKeypadSync(enable ? 1 : 0, timeout) == 1;
        }

        /// <summary>
        /// 是否在使用dx鼠标时开启windows消息.默认开启
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableMouseMsg(bool enable)
        {
            return _dm.EnableMouseMsg(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】鼠标消息采用同步发送模式.默认异步
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="timeout">单位是毫秒,表示同步等待的最大时间</param>
        /// <returns>操作是否成功</returns>
        public bool EnableMouseSync(bool enable, int timeout)
        {
            return _dm.EnableMouseSync(enable ? 1 : 0, timeout) == 1;
        }

        /// <summary>
        /// 键盘动作模拟真实操作,点击延时随机
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableRealKeypad(bool enable)
        {
            return _dm.EnableRealKeypad(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】鼠标动作模拟真实操作,带移动轨迹,以及点击延时随机
        /// </summary>
        /// <param name="enable">0 关闭模拟 1 开启模拟(直线模拟) 2 开启模式(随机曲线, 更接近真实)</param>
        /// <param name="mousedelay">单位是毫秒. 表示在模拟鼠标移动轨迹时,每移动一次的时间间隔.这个值越大,鼠标移动越慢.</param>
        /// <param name="mousestep">示在模拟鼠标移动轨迹时,每移动一次的距离. 这个值越大，鼠标移动越快速.</param>
        /// <returns>操作是否成功</returns>
        public bool EnableRealMouse(int enable, int mousedelay, int mousestep)
        {
            return _dm.EnableRealMouse(enable, mousedelay, mousestep) == 1;
        }

        /// <summary>
        /// 【后台】设置是否开启高速dx键鼠模式。 默认是关闭
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnableSpeedDx(bool enable)
        {
            return _dm.EnableSpeedDx(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】强制解除绑定窗口,并释放系统资源
        /// </summary>
        /// <param name="hwnd">需要强制解除绑定的窗口句柄</param>
        /// <returns>操作是否成功</returns>
        public bool ForceUnBindWindow(int hwnd)
        {
            return _dm.ForceUnBindWindow(hwnd) == 1;
        }

        /// <summary>
        /// 【后台】获取当前对象已经绑定的窗口句柄. 无绑定返回0
        /// </summary>
        public int GetBindWindow()
        {
            return _dm.GetBindWindow();
        }

        /// <summary>
        /// 【后台】判定指定窗口是否已经被后台绑定. (前台无法判定)
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>操作是否成功</returns>
        public bool IsBind(int hwnd)
        {
            return _dm.IsBind(hwnd) == 1;
        }

        /// <summary>
        /// 【后台】锁定指定窗口的图色数据(不刷新)
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LockDisplay(bool @lock)
        {
            return _dm.LockDisplay(@lock ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】禁止外部输入到指定窗口。
        /// 此接口只针对dx键鼠，普通鼠标无效，可用于绑定为dx2鼠标时，需要锁定输入
        /// </summary>
        /// <param name="lock">0.关闭锁定，1.开启锁定（键鼠均锁），2.只锁定鼠标，3.只锁定键盘</param>
        /// <returns>操作是否成功</returns>
        public bool LockInput(int @lock)
        {
            return _dm.LockInput(@lock) == 1;
        }

        /// <summary>
        /// 【后台】设置前台鼠标在屏幕上的活动范围
        /// </summary>
        /// <param name="x1">区域的左上X坐标. 屏幕坐标</param>
        /// <param name="y1">区域的左上Y坐标. 屏幕坐标</param>
        /// <param name="x2">区域的右下X坐标. 屏幕坐标</param>
        /// <param name="y2">区域的右下Y坐标. 屏幕坐标</param>
        /// <returns>操作是否成功</returns>
        public bool LockMouseRect(int x1, int y1, int x2, int y2)
        {
            return _dm.LockMouseRect(x1, y1, x2, y2) == 1;
        }

        /// <summary>
        /// 【后台】设置开启或者关闭系统的Aero效果. (仅对WIN7及以上系统有效)
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SetAero(bool enable)
        {
            return _dm.SetAero(enable ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【后台】设置dx截图最长等待时间。内部默认是3000毫秒. 一般用不到调整这个
        /// 注: 此接口仅对图色为dx.graphic.3d   dx.graphic.3d.8  dx.graphic.2d   dx.graphic.2d.2有效. 其他图色模式无效.
        /// 默认情况下，截图需要等待一个延时，超时就认为截图失败. 这个接口可以调整这个延时. 
        /// 某些时候或许有用.比如当窗口图色卡死(这时获取图色一定都是超时)，并且要判断窗口卡死，那么这个设置就很有用了。
        /// </summary>
        /// <param name="mis">等待时间，单位是毫秒。 注意这里不能设置的过小，否则可能会导致截图失败,从而导致图色函数和文字识别失败</param>
        /// <returns>操作是否成功</returns>
        public bool SetDisplayDelay(int mis)
        {
            return _dm.SetDisplayDelay(mis) == 1;
        }

        /// <summary>
        /// 【后台】在不解绑的情况下,切换绑定窗口.(必须是同进程窗口)
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SwitchBindWindow(int hwnd)
        {
            return _dm.SwitchBindWindow(hwnd) == 1;
        }

        /// <summary>
        /// 【后台】窗口解除绑定
        /// </summary>
        /// <returns></returns>
        public int UnBindWindow()
        {
            return _dm.UnBindWindow();
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
            return _dm.ExcludePos(allPos, type, x1, y1, x2, y2);
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
            return _dm.FindNearestPos(allPos, type, x, y);
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
            return _dm.SortPosDistance(allPos, type, x, y);
        }

        #endregion

        #region 汇编

        /// <summary>
        /// 【汇编】添加指定的MASM汇编指令
        /// </summary>
        /// <param name="asmIns">MASM汇编指令,大小写均可以  比如 mov eax,1</param>
        /// <returns>操作是否成功</returns>
        public bool AsmAdd(string asmIns)
        {
            return _dm.AsmAdd(asmIns) == 1;
        }

        /// <summary>
        /// 【汇编】执行用AsmAdd加到缓冲中的指令
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="mode">模式：0.在本进程中进行执行，这时hwnd无效，1.对hwnd指定的进程内执行,注入模式为创建远程线程</param>
        /// <returns>操作是否成功</returns>
        public bool AsmCall(int hwnd, int mode)
        {
            return _dm.AsmCall(hwnd, mode) == 1;
        }

        /// <summary>
        /// 【汇编】清除汇编指令缓冲区 用AsmAdd添加到缓冲的指令全部清除
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool AsmClear()
        {
            return _dm.AsmClear() == 1;
        }

        /// <summary>
        /// 【汇编】把汇编缓冲区的指令转换为机器码 并用16进制字符串的形式输出
        /// </summary>
        /// <param name="baseAddress">用AsmAdd添加到缓冲区的第一条指令所在的地址</param>
        /// <returns>机器码，比如 "aa bb cc"这样的形式</returns>
        public string AsmCode(int baseAddress)
        {
            return _dm.AsmCode(baseAddress);
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
            return _dm.Assemble(asmCode, baseAddress, upper);
        }

        #endregion

        #region 文件

        /// <summary>
        /// 【文件】拷贝文件
        /// </summary>
        public bool CopyFile(string srcfile, string dstfile, bool over)
        {
            return _dm.CopyFile(srcfile, dstfile, over ? 1 : 0) == 1;
        }

        /// <summary>
        /// 【文件】创建指定目录
        /// </summary>
        public bool CreateFolder(string folder)
        {
            return _dm.CreateFolder(folder) == 1;
        }

        /// <summary>
        /// 【文件】解密指定的文件
        /// </summary>
        public bool DecodeFile(string file, string pwd)
        {
            return _dm.DecodeFile(file, pwd) == 1;
        }

        /// <summary>
        /// 【文件】删除文件
        /// </summary>
        public bool DeleteFile(string file)
        {
            return _dm.DeleteFile(file) == 1;
        }

        /// <summary>
        /// 【文件】删除指定目录
        /// </summary>
        public bool DeleteFolder(string folder)
        {
            return _dm.DeleteFolder(folder) == 1;
        }

        /// <summary>
        /// 【文件】删除指定的ini小节
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名. 如果这个变量为空串，则删除整个section小节</param>
        /// <param name="file">ini文件名</param> 
        public bool DeleteIni(string section, string key, string file)
        {
            return _dm.DeleteIni(section, key, file) == 1;
        }

        /// <summary>
        /// 【文件】删除指定的ini小节.支持加密文件
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名. 如果这个变量为空串，则删除整个section小节</param>
        /// <param name="file">ini文件名</param>
        /// <param name="pwd">密码</param> 
        public bool DeleteIniPwd(string section, string key, string file, string pwd)
        {
            return _dm.DeleteIniPwd(section, key, file, pwd) == 1;
        }

        /// <summary>
        /// 【文件】从internet上下载一个文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="saveFile"></param>
        /// <param name="timeout"></param>
        /// <returns>整形数: 1 : 成功 -1 : 网络连接失败 -2 : 写入文件失败</returns>
        public int DownloadFile(string url, string saveFile, int timeout)
        {
            return _dm.DownloadFile(url, saveFile, timeout);
        }

        /// <summary>
        /// 【文件】加密指定的文件
        /// </summary>
        public bool EncodeFile(string file, string pwd)
        {
            return _dm.EncodeFile(file, pwd) == 1;
        }

        /// <summary>
        /// 【文件】根据指定的ini文件以及section,枚举此section中所有的key名
        /// </summary>
        /// <param name="section">小节名. (不可为空)</param>
        /// <param name="file">ini文件名</param>
        /// <returns>每个key用"|"来连接，如果没有key，则返回空字符串. 比如"aaa|bbb|ccc"</returns>
        public string EnumIniKey(string section, string file)
        {
            return _dm.EnumIniKey(section, file);
        }

        /// <summary>
        /// 【文件】根据指定的ini文件以及section,枚举此section中所有的key名.可支持加密文件
        /// </summary>
        /// <param name="section">小节名. (不可为空)</param>
        /// <param name="file">ini文件名</param>
        /// <param name="pwd">密码</param>
        /// <returns>每个key用"|"来连接，如果没有key，则返回空字符串. 比如"aaa|bbb|ccc"</returns>
        public string EnumIniKeyPwd(string section, string file, string pwd)
        {
            return _dm.EnumIniKeyPwd(section, file, pwd);
        }

        /// <summary>
        /// 【文件】根据指定的ini文件,枚举此ini中所有的Section(小节名)
        /// </summary>
        public string EnumIniSection(string file)
        {
            return _dm.EnumIniSection(file);
        }

        /// <summary>
        /// 【文件】根据指定的ini文件,枚举此ini中所有的Section(小节名).可支持加密文件
        /// </summary>
        public string EnumIniSectionPwd(string file, string pwd)
        {
            return _dm.EnumIniSectionPwd(file, pwd);
        }

        /// <summary>
        /// 【文件】获取指定的文件长度
        /// </summary>
        public int GetFileLength(string file)
        {
            return _dm.GetFileLength(file);
        }

        /// <summary>
        /// 【文件】判断指定文件是否存在
        /// </summary>
        public int IsFileExist(string file)
        {
            return _dm.IsFileExist(file);
        }

        /// <summary>
        /// 【文件】移动文件
        /// </summary>
        public bool MoveFile(string srcfile, string dstfile)
        {
            return _dm.MoveFile(srcfile, dstfile) == 1;
        }

        /// <summary>
        /// 【文件】从指定的文件读取内容.
        /// </summary>
        public string ReadFile(string file)
        {
            return _dm.ReadFile(file);
        }

        /// <summary>
        /// 【文件】从Ini中读取指定信息
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名. 如果这个变量为空串，则删除整个section小节</param>
        /// <param name="file">ini文件名</param> 
        /// <returns>字符串形式表达的读取到的内容</returns>
        public string ReadIni(string section, string key, string file)
        {
            return _dm.ReadIni(section, key, file);
        }

        /// <summary>
        /// 【文件】从Ini中读取指定信息.可支持加密文件
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名. 如果这个变量为空串，则删除整个section小节</param>
        /// <param name="file">ini文件名</param>
        /// <param name="pwd">密码</param>
        /// <returns>字符串形式表达的读取到的内容</returns>
        public string ReadIniPwd(string section, string key, string file, string pwd)
        {
            return _dm.ReadIniPwd(section, key, file, pwd);
        }

        /// <summary>
        /// 【文件】弹出选择文件夹对话框，并返回选择的文件夹
        /// </summary>
        /// <returns>选择的文件夹全路径</returns>
        public string SelectDirectory()
        {
            return _dm.SelectDirectory();
        }

        /// <summary>
        /// 【文件】弹出选择文件对话框，并返回选择的文件
        /// </summary>
        /// <returns>选择的文件全路径</returns>
        public string SelectFile()
        {
            return _dm.SelectFile();
        }

        /// <summary>
        /// 【文件】向指定文件追加字符串
        /// </summary>
        public bool WriteFile(string file, string content)
        {
            return _dm.WriteFile(file, content) == 1;
        }

        /// <summary>
        /// 【文件】向指定的Ini写入信息
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名</param>
        /// <param name="value">变量内容</param>
        /// <param name="file">ini文件名</param>
        public bool WriteIni(string section, string key, string value, string file)
        {
            return _dm.WriteIni(section, key, value, file) == 1;
        }

        /// <summary>
        /// 【文件】向指定的Ini写入信息.支持加密文件
        /// </summary>
        /// <param name="section">小节名</param>
        /// <param name="key">变量名</param>
        /// <param name="value">变量内容</param>
        /// <param name="file">ini文件名</param>
        /// <param name="pwd">密码</param>
        public bool WriteIniPwd(string section, string key, string value, string file, string pwd)
        {
            return _dm.WriteIniPwd(section, key, value, file, pwd) == 1;
        }

        #endregion

        #region 答题

        /// <summary>
        /// 【答题】可以把上次FaqPost的发送取消,接着下一次FaqPost
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool FaqCancel()
        {
            return _dm.FaqCancel() == 1;
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
            return _dm.FaqCapture(x1, y1, x2, y2, quality, delay, mis);
        }

        /// <summary>
        /// 【答题】截取指定图片中的图像,并返回此句柄
        /// </summary>
        /// <param name="x1">区域的左上X坐标</param>
        /// <param name="y1">区域的左上Y坐标</param>
        /// <param name="x2">区域的右下X坐标</param>
        /// <param name="y2">区域的右下Y坐标</param>
        /// <param name="file">图片文件名,图像格式基本都支持</param>
        /// <param name="quality">图像或动画品质,或者叫压缩率,此值越大图像质量越好 取值范围（1-100或者250）.当此值为250时,会截取无损bmp图像数据</param>
        /// <returns>图像或者动画句柄</returns>
        public int FaqCaptureFromFile(int x1, int y1, int x2, int y2, string file, int quality)
        {
            return _dm.FaqCaptureFromFile(x1, y1, x2, y2, file, quality);
        }

        /// <summary>
        /// 【答题】从给定的字符串(也可以算是文字类型的问题),获取此句柄. （此接口必须配合答题器v30以后的版本）
        /// </summary>
        /// <param name="str">文字类型的问题. 比如(桃园三结义指的是哪些人?)</param>
        /// <returns>文字句柄</returns>
        public int FaqCaptureString(string str)
        {
            return _dm.FaqCaptureString(str);
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
            return _dm.FaqFetch();
        }

        /// <summary>
        /// 【答题】获取句柄所对应的数据包的大小,单位是字节
        /// </summary>
        /// <param name="handle">由FaqCapture返回的句柄</param>
        /// <returns>数据包大小,一般用于判断数据大小,选择合适的压缩比率.</returns>
        public int FaqGetSize(int handle)
        {
            return _dm.FaqGetSize(handle);
        }

        /// <summary>
        /// 【答题】用于判断当前对象是否有发送过答题(FaqPost)
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool FaqIsPosted()
        {
            return _dm.FaqIsPosted() == 1;
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
            return _dm.FaqPost(server, handle, requestType, timeout);
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
            return _dm.FaqSend(server, handle, requestType, timeout);
        }

        #endregion

        #region Foobar

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
            return _dm.CreateFoobarCustom(hwnd, x, y, picName, transColor, sim);
        }

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
            return _dm.CreateFoobarEllipse(hwnd, x, y, w, h);
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
            return _dm.CreateFoobarRect(hwnd, x, y, w, h);
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
            return _dm.CreateFoobarRoundRect(hwnd, x, y, w, h, rw, rh);
        }

        /// <summary>
        /// 【Foobar】清除指定的Foobar滚动文本区
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarClearText(int hwnd)
        {
            return _dm.FoobarClearText(hwnd) == 1;
        }

        /// <summary>
        /// 【Foobar】关闭一个Foobar,注意,必须调用此函数来关闭窗口,用SetWindowState也可以关闭,但会造成内存泄漏
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarClose(int hwnd)
        {
            return _dm.FoobarClose(hwnd) == 1;
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口内部画线条.
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="x1">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y1">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="x2">右下角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y2">右下角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="color">填充的颜色值</param>
        /// <param name="style">画笔类型. 0为实线. 1为虚线</param>
        /// <param name="width">线条宽度</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarDrawLine(int hwnd, int x1, int y1, int x2, int y2, string color, int style, int width)
        {
            return _dm.FoobarDrawLine(hwnd, x1, y1, x2, y2, color, style, width) == 1;
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口绘制图像 此图片不能是加密的图片
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="picName">图像文件名</param>
        /// <param name="transColor">图像透明色</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarDrawPic(int hwnd, int x, int y, string picName, string transColor)
        {
            return _dm.FoobarDrawPic(hwnd, x, y, picName, transColor) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool FoobarDrawText(int hwnd, int x, int y, int w, int h, string text, string color, int align)
        {
            return _dm.FoobarDrawText(hwnd, x, y, w, h, text, color, align) == 1;
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
        /// <returns>操作是否成功</returns>
        public bool FoobarFillRect(int hwnd, int x1, int y1, int x2, int y2, string color)
        {
            return _dm.FoobarFillRect(hwnd, x1, y1, x2, y2, color) == 1;
        }

        /// <summary>
        /// 【Foobar】锁定指定的Foobar窗口,不能通过鼠标来移动
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarLock(int hwnd)
        {
            return _dm.FoobarLock(hwnd) == 1;
        }

        /// <summary>
        /// 【Foobar】向指定的Foobar窗口区域内输出滚动文字
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="text">文本内容</param>
        /// <param name="color">文本颜色</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarPrintText(int hwnd, string text, string color)
        {
            return _dm.FoobarPrintText(hwnd, text, color) == 1;
        }

        /// <summary>
        /// 【Foobar】设置指定Foobar窗口的字体
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="fontName">系统字体名,注意,必须保证系统中有此字体</param>
        /// <param name="size">字体大小</param>
        /// <param name="flag">0.正常字体，1.粗体，2.斜体，4.下划线
        /// 文字可以是以上的组合 比如粗斜体就是1+2,斜体带下划线就是:2+4等</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarSetFont(int hwnd, string fontName, int size, int flag)
        {
            return _dm.FoobarSetFont(hwnd, fontName, size, flag) == 1;
        }

        /// <summary>
        /// 【Foobar】设置保存指定的Foobar滚动文本区信息到文件
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
        /// <returns>操作是否成功</returns>
        public bool FoobarSetSave(int hwnd, string file, bool enable, string header)
        {
            return _dm.FoobarSetSave(hwnd, file, enable ? 1 : 0, header) == 1;
        }

        /// <summary>
        /// 【Foobar】设置指定Foobar窗口的是否透明
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="isTrans">是否透明.</param>
        /// <param name="color">透明色(RRGGBB</param>
        /// <param name="sim">透明色的相似值 0.1-1.0</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarSetTrans(int hwnd, bool isTrans, string color, double sim)
        {
            return _dm.FoobarSetTrans(hwnd, isTrans ? 1 : 0, color, sim) == 1;
        }

        /// <summary>
        /// 【Foobar】在指定的Foobar窗口绘制gif动画
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="picName">图像文件名</param>
        /// <param name="repeatLimit">表示重复GIF动画的次数，如果是0表示一直循环显示.大于0，则表示循环指定的次数以后就停止显示</param>
        /// <param name="delay">表示每帧GIF动画之间的时间间隔.如果是0，表示使用GIF内置的时间，如果大于0，表示使用自定义的时间间隔</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarStartGif(int hwnd, int x, int y, string picName, int repeatLimit, int delay)
        {
            return _dm.FoobarStartGif(hwnd, x, y, picName, repeatLimit, delay) == 1;
        }

        /// <summary>
        /// 【Foobar】停止在指定foobar里显示的gif动画
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="picName">图像文件名</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarStopGif(int hwnd, int x, int y, string picName)
        {
            return _dm.FoobarStopGif(hwnd, x, y, picName) == 1;
        }

        /// <summary>
        /// 【Foobar】设置滚动文本区的文字行间距,默认是3
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="lineGap">文本行间距</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarTextLineGap(int hwnd, int lineGap = 3)
        {
            return _dm.FoobarTextLineGap(hwnd, lineGap) == 1;
        }

        /// <summary>
        /// 【Foobar】设置滚动文本区的文字输出方向,默认是0
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <param name="dir">文字方向：0.表示向下输出，1.表示向上输出</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarTextPrintDir(int hwnd, int dir = 0)
        {
            return _dm.FoobarTextPrintDir(hwnd, dir) == 1;
        }

        /// <summary>
        /// 【Foobar】设置指定Foobar窗口的滚动文本框范围,默认的文本框范围是窗口区域
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <param name="x">左上角X坐标(相对于hwnd客户区坐标)</param>
        /// <param name="y">左上角Y坐标(相对于hwnd客户区坐标)</param>
        /// <param name="w">区域的宽度</param>
        /// <param name="h">区域的高度</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarTextRect(int hwnd, int x, int y, int w, int h)
        {
            return _dm.FoobarTextRect(hwnd, x, y, w, h) == 1;
        }

        /// <summary>
        /// 【Foobar】解锁指定的Foobar窗口,可以通过鼠标来移动
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口句柄,此句柄必须是通过CreateFoobarxxx创建而来</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarUnlock(int hwnd)
        {
            return _dm.FoobarUnlock(hwnd) == 1;
        }

        /// <summary>
        /// 【Foobar】刷新指定的Foobar窗口
        /// </summary>
        /// <param name="hwnd">指定的Foobar窗口,注意,此句柄必须是通过CreateFoobarxxxx系列函数创建出来的</param>
        /// <returns>操作是否成功</returns>
        public bool FoobarUpdate(int hwnd)
        {
            return _dm.FoobarUpdate(hwnd) == 1;
        }

        #endregion

        #region 防护盾

        /// <summary>
        /// 【防护盾】针对部分检测措施的保护盾.  前面有五角星的表示同时支持32位和64位,否则就仅支持64位. 另外phide仅仅支持32位.
        /// </summary>
        /// <param name="enable">整形数: 0表示关闭保护盾(仅仅对memory memory2 memory3 b2 b3起作用) 1表示打开保护盾</param>
        /// <param name="type">参数具体内容可以是以下任意一个. 所有驱动功能仅支持64位(win7 win7sp1 win8 win8.1 win10)
        ///  ★"np" : 这个是防止NP检测.
        ///  "memory" : 这个保护内存系列接口和汇编接口可以正常运行. (此模式需要加载驱动)
        ///  "memory2" : 这个保护内存系列接口和汇编接口可以正常运行. (此模式需要加载驱动)
        ///  "memory3 pid addr_start addr_end" : 这个保护内存系列接口和汇编接口可以正常运行.pid表示要操作内存的进程ID,指定了以后,所有内存系列接口仅能对此pid进程进行操作,其他进程无效.但此盾速度较快。addr_start表示起始地址(此参数可以忽略),addr_end表示结束地址(此参数可以忽略). 另外，如果你发现有地址读写不到，可以尝试重新调用一次此盾.此盾是对指定的PID，指定的地址范围做快照. (此模式需要加载驱动)
        ///  ★"display2" : 同display,但此模式用在一些极端的场合.比如用任何截图软件也无法截图时，可以考虑这个盾.
        ///  ★"block [pid]" : 保护指定进程不被非法访问.pid为可选参数.如果不指定pid，默认保护当前进程,另种实现方式.（此模式需要加载驱动）
        ///  ★"b2 [pid]" : 保护指定进程不被非法访问.pid为可选参数.如果不指定pid，默认保护当前进程,另种实现方式.(此模式需要加载驱动),另外,b2盾有副作用，会导致任何和音频输出的函数无声音(比如，Play和Beep函数，或者类似此函数实现的方式.解决办法是另外创建一个进程用来播放音乐). 另外要特别注意，个别系统上，会出现保护进程退出时，导致系统蓝屏，解决办法是在进程结束前，关闭b2盾即可.
        ///  "b3 [pid]" : 保护指定进程不被非法访问.pid为可选参数.如果不指定pid，默认保护当前进程,另种实现方式.(此模式需要加载驱动),另外,b3盾有副作用，会导致无法创建线程，无法结束线程,无法操作某些系统API(比如打开文件对话框)，无法绑定目标窗口等等,解决办法是，临时关闭b3，进行你的操作,然后再打开b3。 
        ///  "f1 [pid]" : 把当前进程伪装成pid指定的进程，可以保护进程路径无法被获取到.如果省略pid参数，则伪装成svchost.exe进程. (此模式需要加载驱动),另外，简单游平台专用版本无法使用此盾，原因是和简单游有冲突。   还有，使用此盾后，别人无法获取到你的进程的真实路径，但自己也同样无法获取，所以如果要获取真实路径，请务必在获取到路径后保存，再调用此盾.pid参数如果有效，那必须是一个真实存在的pid,否则会失败.如果被伪装的进程关闭了，那么当前进程也会立刻失去伪装.还有最重要的一点，伪装的进程和目的进程，占用内存要差不多，最好目的进程占用内存要大于被伪装进程，否则可能会导致进程崩溃!!!  有些编译平台编译出的程序,貌似开这个盾会导致异常，可以尝试f2盾.
        ///  ★"d1 [cls][add dll_name exact]" : 阻止指定的dll加载到本进程.这里的dll_name不区分大小写.具体调用方法看下面的例子.
        ///  "f2 &lt;target_process&gt; &lt;protect_process&gt;" :把protect_process伪装成target_process运行.此盾会加载target_process运行,然后用protect_process来替换target_process,从而达到伪装自身的目的.此盾不加载驱动(使用此盾后，别人无法获取到你的进程的真实路径，但自己也同样无法获取，所以如果要获取真实路径，请务必在获取到路径后保存后,通过共享内存等方式传递给保护进程).
        /// "phide [pid]" : 隐藏制定进程,保护指定进程以及进程内的窗口不被非法访问.pid为可选参数.如果不指定pid，默认保护当前进程. (此模式需要加载驱动, 目前仅支持32位系统)
        /// "hm module unlink" : 防止当前进程中的指定模块被非法访问.module为模块名,比如dm.dll 。 unlink取0或者1，1表示是否把模块在进程模块链表中擦除,0表示不擦除.(此模式需要加载驱动) 
        /// "inject mode &lt;param&gt; pid unlink erase" : 注入指定的DLL到指定的进程中.具体参数含义根据mode决定.(此模式需要加载驱动)
        ///    mode取值0 1 2 3，具体含义如下:
        ///     0: 此时param表示需要注入的dll全路径.pid表示需要注入进去的进程ID.unlink(取值0和1)，表示是否从进程模块链表中断链.erase(取值0和1),表示是否擦除PE头.注入方式是通过创建线程注入.
        ///     1: 此时param表示需要注入的dll全路径.pid表示需要注入进去的进程ID.unlink(取值0和1)，表示是否从进程模块链表中断链.erase(取值0和1),表示是否擦除PE头.注入方式是通过APC注入.
        ///     2: 此时param表示需要注入的dll全路径.pid表示需要注入进去的进程ID.unlink(取值0和1)，表示是否从进程模块链表中断链.erase(取值0和1),表示是否擦除PE头.注入方式是内存加载DLL.
        ///     3: 此时param表示需要注入的dll的地址和大小.param表示为addr,size.addr表示DLL的起始地址,10进制表示,size表示DLL的大小，10进制表示.pid表示需要注入进去的进程ID.unlink(取值0和1)，表示是否从进程模块链表中断链.erase(取值0和1),表示是否擦除PE头.注入方式是内存加载DLL.
        /// "del &lt;path&gt;" :强制删除指定的文件.path表示需要删除的文件的全路径.当path为0时,表示为当前dm.dll的路径,当path为1时,表示为当前EXE的全路径.(此模式需要加载驱动)
        /// 其它后续开发.</param>
        public int DmGuard(int enable, string type)
        {
            return _dm.DmGuard(enable, type);
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
        /// <returns>操作是否成功</returns>
        public bool ActiveInputMethod(int hwnd, string inputMethod)
        {
            return _dm.ActiveInputMethod(hwnd, inputMethod) == 1;
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
            return _dm.CheckInputMethod(hwnd, inputMethod) == 1;
        }

        /// <summary>
        /// 检测是否可以进入临界区,如果可以返回1,否则返回0. 此函数如果返回1，则调用对象就会占用此互斥信号量,直到此对象调用LeaveCri,否则不会释放.注意:如果调用对象在释放时，会自动把本对象占用的互斥信号量释放
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool EnterCri()
        {
            return _dm.EnterCri() == 1;
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
            return _dm.FindInputMethod(inputMethod) == 1;
        }

        /// <summary>
        /// 初始化临界区,必须在脚本开头调用一次.这个函数是强制把插件内的互斥信号量归0,无论调用对象是否拥有此信号量
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool InitCri()
        {
            return _dm.InitCri() == 1;
        }

        /// <summary>
        /// 和EnterCri对应,离开临界区。此函数是释放调用对象占用的互斥信号量. 注意，只有调用对象占有了互斥信号量，此函数才会有作用. 否则没有任何作用. 如果调用对象在释放时，会自动把本对象占用的互斥信号量释放
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool LeaveCri()
        {
            return _dm.LeaveCri() == 1;
        }

        #endregion

        #endregion
    }
}