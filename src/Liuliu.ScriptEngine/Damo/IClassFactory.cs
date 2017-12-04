// -----------------------------------------------------------------------
//  <copyright file="IClassFactory.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 23:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;


namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 清风免注册调用COM类
    /// </summary>
    [ComVisible(true), ComImport(),
     Guid("00000001-0000-0000-C000-000000000046"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IClassFactory
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int CreateInstance([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object obj);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int LockServer([MarshalAs(UnmanagedType.Bool), In] bool fLock);
    }
}