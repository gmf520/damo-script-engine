using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liuliu.ScriptEngine.Tasks
{
    /// <summary>
    /// 委托辅助操作类
    /// </summary>
    public static class Delegater
    {
        /// <summary>
        /// 重复执行指定代码直到成功
        /// </summary>
        /// <param name="trueFunc">要执行的返回成功的操作</param>
        /// <param name="failAction">每次执行失败时，执行的动作</param>
        /// <param name="maxCount">重试的最大次数，0为一直重试</param>
        /// <returns>最终是否成功</returns>
        public static bool WaitTrue(Func<bool> trueFunc, Action failAction = null, int maxCount = 0)
        {
            failAction = failAction ?? (() => { });
            if (maxCount == 0)
            {
                while (!trueFunc())
                {
                    failAction();
                }
                return true;
            }
            int count = 0;
            while (!trueFunc() && count < maxCount)
            {
                failAction();
                count++;
            }
            return count < maxCount;
        }

        /// <summary>
        /// 重复执行指定代码直到成功，满足指定条件时立即失败
        /// </summary>
        /// <param name="trueFunc">要执行的返回成功的操作</param>
        /// <param name="failAction">每次执行失败时，执行的动作</param>
        /// <param name="breakFunc">当此代码执行成功时，立即中断并返回失败</param>
        /// <param name="maxCount">重试的最大次数，0为一直重试</param>
        /// <returns>最终是否成功</returns>
        public static bool WaitTrue(Func<bool> trueFunc, Action failAction, Func<bool> breakFunc, int maxCount = 0)
        {
            failAction = failAction ?? (() => { });
            if (maxCount == 0)
            {
                while (!trueFunc())
                {
                    if (breakFunc())
                    {
                        return false;
                    }
                    failAction();
                }
                return true;
            }
            int count = 0;
            while (!trueFunc() && count < maxCount)
            {
                if (breakFunc())
                {
                    return trueFunc();
                }
                failAction();
                count++;
            }
            return count < maxCount;
        }
    }
}
