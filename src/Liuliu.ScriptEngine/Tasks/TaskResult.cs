// -----------------------------------------------------------------------
//  <copyright file="TaskResult.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:44</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 任务执行结果
    /// </summary>
    public class TaskResult
    {
        public TaskResult(TaskResultType type, string message = null)
        {
            ResultType = type;
            Message = message;
        }

        public TaskResultType ResultType { get; private set; }

        public string Message { get; private set; }

        public static TaskResult Success { get; } = new TaskResult(TaskResultType.Success);

        public static TaskResult Jump { get; } = new TaskResult(TaskResultType.Jump);

        /// <summary>
        /// 获取 任务是否即将停止
        /// </summary>
        public bool Stopping
        {
            get { return ResultType == TaskResultType.Fail || ResultType == TaskResultType.Finished; }
        }
    }


    /// <summary>
    /// 任务执行结果的类型
    /// </summary>
    public enum TaskResultType
    {
        /// <summary>
        /// 任务执行失败
        /// </summary>
        Fail,

        /// <summary>
        /// 任务执行成功，并可继续执行
        /// </summary>
        Success,

        /// <summary>
        /// 任务执行结束
        /// </summary>
        Finished,

        /// <summary>
        /// 跳转到别的步骤
        /// </summary>
        Jump
    }
}