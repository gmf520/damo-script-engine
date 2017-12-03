// -----------------------------------------------------------------------
//  <copyright file="TaskInterruptException.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 17:45</last-date>
// -----------------------------------------------------------------------

using System;


namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 表示任务中断的异常，发生此异常时任务中断，但不作为系统异常记录
    /// </summary>
    public class TaskInterruptException : Exception
    {
        /// <summary>
        /// 初始化一个<see cref="TaskInterruptException"/>类型的新实例
        /// </summary>
        public TaskInterruptException()
        { }

        /// <summary>
        /// 初始化一个<see cref="TaskInterruptException"/>类型的新实例
        /// </summary>
        public TaskInterruptException(string message)
            : base(message)
        { }

        /// <summary>
        /// 初始化一个<see cref="TaskInterruptException"/>类型的新实例
        /// </summary>
        public TaskInterruptException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}