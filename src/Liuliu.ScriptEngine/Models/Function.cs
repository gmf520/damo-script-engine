// -----------------------------------------------------------------------
//  <copyright file="Function.cs" company="柳柳软件">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-26 23:46</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace Liuliu.ScriptEngine.Models
{
    /// <summary>
    /// 功能信息
    /// </summary>
    public class Function
    {
        public Function()
        {
            DisableRoleIds = new HashSet<int>();
            AllowRoleIds = new HashSet<int>();
            DisableAreaNames = new HashSet<string>();
            AllowAreaNames = new HashSet<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int BeginLevel { get; set; }

        public int? EndLevel { get; set; }

        public bool IsSub { get; set; }

        public bool IsCircle { get; set; }

        public bool IsSky { get; set; }

        public string ExtendData { get; set; }

        public string Remark { get; set; }

        public FunctionType FunctionType { get; set; }

        public int SoftwareId { get; set; }

        public string SoftwareName { get; set; }

        public ICollection<int> AllowRoleIds { get; set; }

        public ICollection<int> DisableRoleIds { get; set; }

        public ICollection<string> AllowAreaNames { get; set; }

        public ICollection<string> DisableAreaNames { get; set; }
    }


    public enum FunctionType
    {
        /// <summary>
        /// 免费功能
        /// </summary>
        Free = 0,

        /// <summary>
        /// 收费功能
        /// </summary>
        Charging = 10,

        /// <summary>
        /// 管理功能
        /// </summary>
        Admin = 20
    }
}