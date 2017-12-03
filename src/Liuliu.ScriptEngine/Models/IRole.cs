// -----------------------------------------------------------------------
//  <copyright file="IRole.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 17:44</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.ScriptEngine.Damo;
using Liuliu.ScriptEngine.Tasks;


namespace Liuliu.ScriptEngine.Models
{
    /// <summary>
    ///     定义公共的角色信息
    /// </summary>
    public interface IRole
    {
        /// <summary>
        ///     获取 任务消息输出，主要用于<see cref="TaskBase" />执行过程中的消息输出
        /// </summary>
        Action<string> OutMessage { get; }

        /// <summary>
        ///     获取 子功能消息输出，主要用于<see cref="IRole" />的扩展方法的细节消息输出
        /// </summary>
        Action<string> OutSubMessage { get; }

        /// <summary>
        ///     获取 角色所在窗口
        /// </summary>
        DmWindow Window { get; }

        /// <summary>
        ///     获取 角色名
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     获取 职业
        /// </summary>
        string Vocation { get; }

        /// <summary>
        ///     获取 等级
        /// </summary>
        int Level { get; }

        /// <summary>
        ///     获取 生命值
        /// </summary>
        int HealthPoint { get; }

        /// <summary>
        ///     获取 生命最大值
        /// </summary>
        int HealthPointMax { get; }

        /// <summary>
        ///     获取 魔法值
        /// </summary>
        int MagicPoint { get; }

        /// <summary>
        ///     获取 魔法最大值
        /// </summary>
        int MagicPointMax { get; }

        /// <summary>
        ///     获取 经验值
        /// </summary>
        int Empirical { get; }

        /// <summary>
        ///     获取 升级经验值
        /// </summary>
        int EmpiricalMax { get; }

        /// <summary>
        ///     获取 所在区服
        /// </summary>
        string Area { get; }

        /// <summary>
        ///     获取 所在公会
        /// </summary>
        string Sociaty { get; }

        /// <summary>
        ///     获取 公会职务
        /// </summary>
        string SociatyPosition { get; }

        /// <summary>
        ///     获取 背包金钱
        /// </summary>
        decimal MoneyInBag { get; }

        /// <summary>
        ///     获取 仓库金钱
        /// </summary>
        decimal MoneyInRepertory { get; }

        /// <summary>
        ///     获取 充值元宝
        /// </summary>
        decimal Yuanbao { get; }

        /// <summary>
        ///     获取 当前地图
        /// </summary>
        string CurrentMap { get; }

        /// <summary>
        ///     获取 当前Ｘ坐标
        /// </summary>
        double PointX { get; }

        /// <summary>
        ///     获取 当前Ｙ坐标
        /// </summary>
        double PointY { get; }

        /// <summary>
        ///     获取 是否在城中
        /// </summary>
        bool InCity { get; }

        /// <summary>
        ///     获取 是否在安全区
        /// </summary>
        bool InSafePlace { get; }

        /// <summary>
        ///     获取 是否活着
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        ///     获取 登录账号
        /// </summary>
        string Ac { get; }

        /// <summary>
        ///     获取 登录密码
        /// </summary>
        string P { get; }

        /// <summary>
        ///     角色是否在指定坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alw">坐标容差</param>
        /// <returns></returns>
        bool IsInPos(double x, double y, double alw = 0.2);

        /// <summary>
        ///     使用间隔取点判断角色是否正在移动
        /// </summary>
        /// <param name="mis">移动检查时差</param>
        /// <returns></returns>
        bool IsMoving(int mis = 1000);
    }
}