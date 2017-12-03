// -----------------------------------------------------------------------
//  <copyright file="DmBindDisplay.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:25</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 表示大漠插件窗口绑定屏幕颜色获取方式的枚举
    /// </summary>
    public enum DmBindDisplay
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 正常模式,平常我们用的前台截屏模式
        /// </summary>
        normal,

        /// <summary>
        /// gdi模式,用于窗口采用GDI方式刷新时. 此模式占用CPU较大
        /// </summary>
        gdi,

        /// <summary>
        /// gdi2模式,此模式兼容性较强,但是速度比gdi模式要慢许多,如果gdi模式发现后台不刷新时,可以考虑用gdi2模式
        /// </summary>
        gdi2,

        /// <summary>
        /// dx模式,等同于BindWindowEx中，display设置的"dx.graphic.2d|dx.graphic.3d",具体参考BindWindowEx。
        /// 注意此模式需要管理员权限
        /// </summary>
        dx,

        /// <summary>
        /// dx2模式,用于窗口采用dx模式刷新,如果dx方式会出现窗口所在进程崩溃的状况,可以考虑采用这种.采用这种方式要保证窗口有一部分在屏幕外.win7或者vista不需要移动也可后台.此模式占用CPU较大.
        /// </summary>
        dx2,

        /// <summary>
        /// dx3模式,同dx2模式,但是如果发现有些窗口后台不刷新时,可以考虑用dx3模式,此模式比dx2模式慢许多. 此模式占用CPU较大
        /// </summary>
        dx3
        // ReSharper restore InconsistentNaming
    }
}