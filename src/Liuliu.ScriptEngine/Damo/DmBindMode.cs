// -----------------------------------------------------------------------
//  <copyright file="DmBindMode.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:26</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 表示大漠插件窗口绑定模式的枚举
    /// </summary>
    public enum DmBindMode
    {
        /// <summary>
        /// 推荐模式,此模式比较通用，而且后台效果是最好的
        /// </summary>
        _0 = 0,

        /// <summary>
        /// 和模式0效果一样，如果模式0会失败时，可以尝试此模式,此模式仅支持按键和简单游平台,小精灵等一律不支持
        /// </summary>
        _1 = 1,

        /// <summary>
        /// 同模式0,此模式为老的模式0,尽量不要用此模式，除非有兼容性问题
        /// </summary>
        _2 = 2,

        /// <summary>
        /// 同模式1,此模式为老的模式1,尽量不要用此模式，除非有兼容性问题
        /// </summary>
        _3 = 3,

        /// <summary>
        /// 同模式0,如果模式0有崩溃问题，可以尝试此模式
        /// </summary>
        _4 = 4,

        /// <summary>
        /// 同模式1, 如果模式0有崩溃问题，可以尝试此模式
        /// </summary>
        _5 = 5,

        /// <summary>
        /// 同模式0，如果模式0有崩溃问题，可以尝试此模式
        /// </summary>
        _6 = 6,

        /// <summary>
        /// 同模式1，如果模式1有崩溃问题，可以尝试此模式
        /// </summary>
        _7 = 7,

        /// <summary>
        /// 超级绑定模式. 可隐藏目标进程中的dm.dll.避免被恶意检测.效果要比dx.public.hide.dll好. 推荐使用
        /// </summary>
        _101 = 101,

        /// <summary>
        /// 同模式101，如果模式101有崩溃问题，可以尝试此模式.
        /// </summary>
        _103 = 103
    }
}