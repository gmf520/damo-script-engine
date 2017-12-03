// -----------------------------------------------------------------------
//  <copyright file="TaskRunState.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-27 3:02</last-date>
// -----------------------------------------------------------------------



namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 任务运行状态
    /// </summary>
    public enum TaskRunState
    {
        Initialize,
        Starting,
        Running,
        Pausing,
        Paused,
        Continuing,
        Stopping,
        Stopped
    }
}