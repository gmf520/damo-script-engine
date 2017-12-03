// -----------------------------------------------------------------------
//  <copyright file="DmBindMouse.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:26</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 表示大漠插件窗口绑定鼠标仿真模式的枚举
    /// </summary>
    public enum DmBindMouse
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 正常模式,平常我们用的前台鼠标模式
        /// </summary>
        normal,

        /// <summary>
        /// Windows模式,采取模拟windows消息方式 同按键自带后台插件
        /// </summary>
        windows,

        /// <summary>
        /// Windows2 模式,采取模拟windows消息方式(锁定鼠标位置) 此模式等同于BindWindowEx中的mouse为以下组合
        /// "dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.state.message"。
        /// 注意此模式需要管理员权限
        /// </summary>
        windows2,

        /// <summary>
        /// Windows3模式，采取模拟windows消息方式,可以支持有多个子窗口的窗口后台
        /// </summary>
        windows3,

        /// <summary>
        /// dx模式,采用模拟dx后台鼠标模式,这种方式会锁定鼠标输入.
        /// 有些窗口在此模式下绑定时，需要先激活窗口再绑定(或者绑定以后激活)，否则可能会出现绑定后鼠标无效的情况.
        /// 此模式等同于BindWindowEx中的mouse为以下组合："dx.public.active.api|dx.public.active.message|dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.state.api|dx.mouse.state.message|dx.mouse.api|dx.mouse.focus.input.api|dx.mouse.focus.input.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api|dx.mouse.cursor"
        /// 注意此模式需要管理员权限
        /// </summary>
        dx,

        /// <summary>
        /// dx2模式,这种方式类似于dx模式,但是不会锁定外部鼠标输入.
        /// 有些窗口在此模式下绑定时，需要先激活窗口再绑定(或者绑定以后手动激活)，否则可能会出现绑定后鼠标无效的情况. 
        /// 此模式等同于BindWindowEx中的mouse为以下组合 "dx.public.active.api|dx.public.active.message|dx.mouse.position.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.focus.input.api|dx.mouse.focus.input.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api| dx.mouse.cursor"
        /// 注意此模式需要管理员权限
        /// </summary>
        dx2
    }
}