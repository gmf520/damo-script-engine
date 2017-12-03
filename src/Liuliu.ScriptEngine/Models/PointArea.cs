// -----------------------------------------------------------------------
//  <copyright file="PointArea.cs" company="柳柳软件">
//      Copyright (c) 2015 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-05-15 1:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace Liuliu.ScriptEngine.Models
{
    /// <summary>
    /// 坐标区域信息
    /// </summary>
    public class PointArea
    {
        /// <summary>
        /// 初始化一个<see cref="PointArea"/>类型的新实例
        /// </summary>
        public PointArea()
            : this(null)
        { }

        /// <summary>
        /// 初始化一个<see cref="PointArea"/>类型的新实例
        /// </summary>
        public PointArea(string name)
        {
            Name = name;
            PointRanges = new List<Tuple<Point, Point>>();
            EntryPoints = new Dictionary<string, Point>();
        }

        /// <summary>
        /// 获取或设置 区域名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 区域范围集合
        /// </summary>
        public ICollection<Tuple<Point, Point>> PointRanges { get; set; }

        /// <summary>
        /// 获取或设置 区域入口坐标
        /// </summary>
        public IDictionary<string, Point> EntryPoints { get; set; }
    }
}