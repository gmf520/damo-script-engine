// -----------------------------------------------------------------------
//  <copyright file="DmBindKeypad.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:26</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 表示大漠插件窗口绑定的键盘仿真模式的枚举
    /// </summary>
    public enum DmBindKeypad
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 正常模式,平常我们用的前台键盘模式
        /// </summary>
        normal,

        /// <summary>
        /// Windows模式,采取模拟windows消息方式 同按键的后台插件
        /// </summary>
        windows,

        /// <summary>
        /// dx模式,采用模拟dx后台键盘模式。有些窗口在此模式下绑定时，需要先激活窗口再绑定(或者绑定以后激活)，否则可能会出现绑定后键盘无效的情况. 此模式等同于BindWindowEx中的keypad为以下组合
        /// "dx.public.active.api|dx.public.active.message| dx.keypad.state.api|dx.keypad.api|dx.keypad.input.lock.api".
        /// 注意此模式需要管理员权限.
        /// </summary>
        dx
    }
}