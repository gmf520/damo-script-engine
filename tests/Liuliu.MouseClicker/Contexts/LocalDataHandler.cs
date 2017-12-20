// -----------------------------------------------------------------------
//  <copyright file="LocalDataHandler.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-20 12:38</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using OSharp.Utility.Extensions;
using OSharp.Utility.Reflection;


namespace Liuliu.MouseClicker.Contexts
{
    /// <summary>
    /// 本地数据处理类
    /// </summary>
    public static class LocalDataHandler
    {
        /// <summary>
        /// 获取指定文件指定键的数据
        /// </summary>
        public static T GetData<T>(string fileName, string key)
        {
            IDictionary<string, string> dict = GetDataDict(fileName);
            if (dict == null || !dict.ContainsKey(key))
            {
                return default(T);
            }
            string json = dict[key];
            return json.FromJsonString<T>();
        }

        /// <summary>
        /// 设置指定文件指定键的数据
        /// </summary>
        public static void SetData(string fileName, string key, object data)
        {
            IDictionary<string, string> dict = GetDataDict(fileName);
            if (dict == null)
            {
                dict = new Dictionary<string, string>();
            }
            dict[key] = data.ToJsonString();
            SetDataDict(fileName, dict);
        }

        private static IDictionary<string, string> GetDataDict(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            return SyncLocker.MutexLock(fileName,
                () =>
                {
                    string json = File.ReadAllText(fileName);
                    if (json.IsMissing() || !json.StartsWith("{"))
                    {
                        return null;
                    }
                    IDictionary<string, string> dict = json.FromJsonString<IDictionary<string, string>>();
                    return dict;
                });
        }

        private static void SetDataDict(string fileName, IDictionary<string, string> dict)
        {
            string json = dict.ToJsonString();
            SyncLocker.MutexLock(fileName,
                () =>
                {
                    File.WriteAllText(fileName, json);
                });
        }
    }
}