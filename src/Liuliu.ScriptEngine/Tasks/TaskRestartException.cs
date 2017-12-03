// -----------------------------------------------------------------------
//  <copyright file="TaskRestartException.cs" company="柳柳软件">
//      Copyright (c) 2014 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-09-10 1:51</last-date>
// -----------------------------------------------------------------------

using System;


namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 当<see cref="TaskEngine"/>截获此异常后，将停止并重新启动任务
    /// </summary>
    public class TaskRestartException : Exception
    {
        /// <summary>
        /// 初始化一个<see cref="TaskRestartException"/>类型的新实例
        /// </summary>
        public TaskRestartException()
            : base()
        { }

        /// <summary>
        /// 初始化一个<see cref="TaskRestartException"/>类型的新实例
        /// </summary>
        public TaskRestartException(string message)
            : base(message)
        { }

        /// <summary>
        /// 初始化一个<see cref="TaskRestartException"/>类型的新实例
        /// </summary>
        public TaskRestartException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}