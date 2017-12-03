// -----------------------------------------------------------------------
//  <copyright file="TaskStep.cs" company="柳柳软件">
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
    /// 任务执行步骤
    /// </summary>
    public class TaskStep
    {
        /// <summary>
        /// 获取或设置 步骤名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 步骤序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置 步骤执行方法
        /// </summary>
        public Func<TaskContext, TaskResult> RunFunc { get; set; }
    }
}