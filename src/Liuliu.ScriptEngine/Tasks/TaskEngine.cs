// -----------------------------------------------------------------------
//  <copyright file="TaskEngine.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 21:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Liuliu.ScriptEngine.Damo;

using OSharp.Utility;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;


namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 任务执行引擎，任务执行的入口，<see cref="TaskBase"/>的执行者
    /// </summary>
    public class TaskEngine
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(TaskEngine));
        private TaskBase _task;
        private TaskEventArg _taskEventArg;
        private Thread _workThread;

        public TaskEngine()
        {
            TaskList = new List<TaskBase>();
        }

        #region 属性

        public DmWindow Window { get; set; }

        public TaskBase CurrentTask
        {
            get { return _task; }
        }

        public List<TaskBase> TaskList { get; set; }

        public TaskRunState TaskRunState { get; private set; }

        public bool IsWorking
        {
            get
            {
                return TaskRunState != TaskRunState.Initialize
                    && TaskRunState != TaskRunState.Stopping
                    && TaskRunState != TaskRunState.Stopped;
            }
        }

        public Action<string> OutMessage { get; set; }

        #endregion

        #region 事件

        public event TaskEventHandler Starting;

        public event TaskEventHandler Started;

        public event TaskEventHandler Pausing;

        public event TaskEventHandler Paused;

        public event TaskEventHandler Continuing;

        public event TaskEventHandler Continued;

        public event TaskEventHandler Stopping;

        public event TaskEventHandler Stopped;

        public event TaskEventHandler StateChanged;

        protected virtual void OnStarting(TaskEventArg e)
        {
            TaskEventHandler handler = Starting;
            DoEventHandler(handler, e);
        }

        protected virtual void OnStarted(TaskEventArg e)
        {
            TaskEventHandler handler = Started;
            DoEventHandler(handler, e);
        }

        protected virtual void OnPausing(TaskEventArg e)
        {
            TaskEventHandler handler = Pausing;
            DoEventHandler(handler, e);
        }

        protected virtual void OnPaused(TaskEventArg e)
        {
            TaskEventHandler handler = Paused;
            DoEventHandler(handler, e);
        }

        protected virtual void OnContinuing(TaskEventArg e)
        {
            TaskEventHandler handler = Continuing;
            DoEventHandler(handler, e);
        }

        protected virtual void OnContinued(TaskEventArg e)
        {
            TaskEventHandler handler = Continued;
            DoEventHandler(handler, e);
        }

        protected virtual void OnStopping(TaskEventArg e)
        {
            TaskEventHandler handler = Stopping;
            DoEventHandler(handler, e);
        }

        protected virtual void OnStopped(TaskEventArg e)
        {
            TaskEventHandler handler = Stopped;
            DoEventHandler(handler, e);
        }

        protected virtual void OnStateChanged(TaskEventArg e)
        {
            TaskEventHandler handler = StateChanged;
            DoEventHandler(handler, e);
        }

        private void DoEventHandler(TaskEventHandler hander, TaskEventArg e)
        {
            if (hander != null)
            {
                hander.Invoke(this, e);
            }
        }

        #endregion

        #region 公共方法

        public void Start(params TaskBase[] tasks)
        {
            tasks.CheckNotNull("tasks");
            if (tasks.Length == 0)
            {
                return;
            }
            TaskList = tasks.ToList();
            if (_workThread == null)
            {
                _workThread = GetWorkThread(tasks);
            }
            if (!CheckTaskRunState(TaskRunState.Starting))
            {
                return;
            }
            _task = tasks[0];
            _taskEventArg = new TaskEventArg { Context = _task.TaskContext };
            OutMessage("任务“{0}”正在启动。".FormatWith(_task.Name));
            TaskRunState = TaskRunState.Starting;
            OnStateChanged(_taskEventArg);
            OnStarting(_taskEventArg);
            _workThread.Start();
        }

        public void Pause()
        {
            if (_task == null)
            {
                OutMessage("当前任务不处于任务状态，不能暂停。");
                return;
            }
            if (!CheckTaskRunState(TaskRunState.Pausing))
            {
                return;
            }
            OutMessage("任务“{0}”正在暂停。".FormatWith(_task.Name));
            TaskRunState = TaskRunState.Pausing;
            OnStateChanged(_taskEventArg);
            OnPausing(_taskEventArg);
            _workThread.Suspend();
            //如果窗口锁定外部输入，则允许外部输入，但不改变锁定类型，以便继续的时候还原锁定
            if (Window.InputLockType != InputLockType.None)
            {
                Window.SetInputLock(InputLockType.None, false);
            }
            TaskRunState = TaskRunState.Paused;
            OnStateChanged(_taskEventArg);
            OnPaused(_taskEventArg);
            OutMessage("任务“{0}”已暂停。".FormatWith(_task.Name));
        }

        public void Continue()
        {
            if (_task == null)
            {
                OutMessage("当前任务不处于任务状态，不能继续。");
                return;
            }
            if (!CheckTaskRunState(TaskRunState.Continuing))
            {
                return;
            }
            OutMessage("任务“{0}”正在继续。".FormatWith(_task.Name));
            TaskRunState = TaskRunState.Continuing;
            OnStateChanged(_taskEventArg);
            OnContinuing(_taskEventArg);
            //如果窗口锁定外部输入，则还原锁定
            if (Window.InputLockType != InputLockType.None)
            {
                Window.SetInputLock(Window.InputLockType, false);
            }
            try
            {
                _workThread.Resume();
            }
            catch (ThreadStateException)
            {
                TaskStop();
                return;
            }
            TaskRunState = TaskRunState.Running;
            OnStateChanged(_taskEventArg);
            OnContinued(_taskEventArg);
            OutMessage("任务“{0}”已继续执行。".FormatWith(_task.Name));
        }

        public void Stop()
        {
            if (_task == null)
            {
                OutMessage("当前角色不处于任务状态，不能停止。");
                return;
            }
            if (!CheckTaskRunState(TaskRunState.Stopping))
            {
                return;
            }
            OutMessage("任务“{0}”正在停止".FormatWith(_task.Name));
            if (_workThread != null)
            {
                try
                {
                    _workThread.Abort();
                    _workThread.Join();
                }
                catch (ThreadStateException)
                {
                    _workThread.Resume();
                }
            }
            WaitForUnBind();
            _workThread = null;
        }

        #endregion

        #region 私有方法

        private Thread GetWorkThread(IEnumerable<TaskBase> tasks)
        {
            return new Thread(() =>
            {
                foreach (TaskBase task in tasks)
                {
                    _task = task;
                    _taskEventArg = new TaskEventArg { Context = _task.TaskContext };
                    try
                    {
                        //窗口绑定
                        DmPlugin dm = Window.Dm;
                        bool flag;
                        flag = Delegater.WaitTrue(() => Window.BindHalfBackground(), () => dm.Delay(1000), 10);
                        //flag = Delegater.WaitTrue(() => Window.BindNormal(), () => dm.Delay(1000), 10);
                        if (!flag)
                        {
                            throw new Exception("角色绑定失败，请添加杀软信任，右键以管理员身份运行，Win7系统请确保电脑账户为“Administrator”");
                        }
                        TaskRunState = TaskRunState.Running;
                        OnStateChanged(_taskEventArg);
                        OnStarted(_taskEventArg);
                        OutMessage("任务“{0}”启动成功。".FormatWith(_task.Name));

                        TaskStart();
                        TaskStop();
                        Window.FlashWindow();
                    }
                    catch (ThreadAbortException)
                    {
                        TaskStop(true);
                        WaitForUnBind();
                        break;
                    }
                    catch (Exception ex)
                    {
                        TaskStop();
                        Window.FlashWindow();
                        Logger.Error("任务执行失败，{0}", ex.FormatMessage());
                        OutMessage("任务执行失败，{0}".FormatWith(ex.Message));
                    }
                    Window.Dm.Delay(1000);
                    TaskList.Remove(task);
                }
                WaitForUnBind();
                _workThread = null;
            }) { IsBackground = true };
        }

        private void TaskStart()
        {
            try
            {
                bool restart = true;
                while (restart)
                {
                    try
                    {
                        TaskResult result = _task.Run();
                        if (result == null)
                        {
                            return;
                        }
                        if (result.ResultType == TaskResultType.Success)
                        {
                            OutMessage("任务“{0}”执行完毕。".FormatWith(_task.Name));
                            return;
                        }
                        if (result.ResultType == TaskResultType.Finished)
                        {
                            OutMessage("任务“{0}”执行结束，{1}".FormatWith(_task.Name, result.Message));
                            return;
                        }
                        OutMessage("任务“{0}”执行中止：{1}".FormatWith(_task.Name, result.Message));
                    }
                    catch (TaskRestartException ex)
                    {
                        OutMessage("任务执行错误{0}，正在重新启动".FormatWith(ex.Message == null ? "" : "：" + ex.Message));
                        _task.Stop();
                        continue;
                    }
                    restart = false;
                }
            }
            catch (TaskInterruptException ex)
            {
                TaskStop();
                OutMessage("任务执行中断，{0}".FormatWith(ex.Message));
            }
        }

        private void TaskStop(bool showStop = false)
        {
            if (TaskRunState == TaskRunState.Stopped)
            {
                return;
            }

            TaskRunState = TaskRunState.Stopping;
            OnStateChanged(_taskEventArg);
            OnStopping(_taskEventArg);

            _task.Stop();

            Window.FlashWindow();
            TaskRunState = TaskRunState.Stopped;
            OnStateChanged(_taskEventArg);
            OnStopped(_taskEventArg);
            if (showStop)
            {
                OutMessage("任务“{0}”已停止".FormatWith(_task.Name)); 
            }
        }

        /// <summary>
        /// 检验任务状态，是否可进行开始/暂停/继续/停止操作
        /// </summary>
        /// <param name="state">要检查的状态，仅限Starting/Pausing/Continuing/Stopping</param>
        /// <returns></returns>
        private bool CheckTaskRunState(TaskRunState state)
        {
            switch (state)
            {
                case TaskRunState.Initialize:
                case TaskRunState.Stopped:
                    if (state != TaskRunState.Starting)
                    {
                        OutMessage("任务尚未启动，不支持暂停/继续/停止操作。");
                        return false;
                    }
                    return true;
                case TaskRunState.Running:
                    if (state == TaskRunState.Starting || state == TaskRunState.Continuing)
                    {
                        OutMessage("任务正在运行，不支持开始/继续操作。");
                        return false;
                    }
                    return true;
                case TaskRunState.Paused:
                    if (state == TaskRunState.Starting)
                    {
                        OutMessage("任务已暂停，不支持开始操作。");
                        return false;
                    }
                    return true;
            }
            return true;
        }

        private void WaitForUnBind()
        {
            bool flag = Delegater.WaitTrue(() => Window.UnBind(), () => Window.Dm.Delay(1000), 10);
            if (!flag)
            {
                throw new Exception("窗口“{0}”解除绑定失败。".FormatWith(Window.Title));
            }
        }

        #endregion
    }


    public delegate void TaskEventHandler(TaskEngine sender, TaskEventArg e);


    public class TaskEventArg : EventArgs
    {
        public TaskContext Context { get; set; }
    }
}