// -----------------------------------------------------------------------
//  <copyright file="TaskContext.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 17:44</last-date>
// -----------------------------------------------------------------------

using System.Dynamic;

using Liuliu.ScriptEngine.Models;


namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 任务执行上下文，任务执行所需的必要数据
    /// </summary>
    public class TaskContext
    {
        public TaskContext(IRole role, Function function)
        {
            Role = role;
            Function = function;
            Settings = new ExpandoObject();
            StepIndex = 0;
            TaskSteps = new TaskStep[0];
            IsAutoFuhuo = true;
        }

        /// <summary>
        /// 获取 角色信息
        /// </summary>
        public IRole Role { get; private set; }

        /// <summary>
        /// 获取 功能信息
        /// </summary>
        public Function Function { get; private set; }

        /// <summary>
        /// 获取或设置 是否自动复活
        /// </summary>
        public bool IsAutoFuhuo { get; set; }

        /// <summary>
        /// 获取 任务设置组信息
        /// </summary>
        public ExpandoObject Settings { get; private set; }

        /// <summary>
        /// 即将或正在执行的任务步骤序号
        /// </summary>
        public int StepIndex { get; set; }

        /// <summary>
        /// 任务步骤集合
        /// </summary>
        public TaskStep[] TaskSteps { get; set; }
    }
}