// -----------------------------------------------------------------------
//  <copyright file="URComLoader.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 23:12</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;


namespace Liuliu.ScriptEngine.Damo
{
    /// <summary>
    /// 清风免注册调用COM类
    /// </summary>
    public class URComLoader : IDisposable
    {
        delegate int DllGETCLASSOBJECTInvoker([MarshalAs(UnmanagedType.LPStruct)]Guid clsid, [MarshalAs(UnmanagedType.LPStruct)]Guid iid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
        
        private IntPtr _lib = IntPtr.Zero;
        private bool _preferURObjects = true;
        
        /// <summary>
        /// 从dll文件创建对象
        /// </summary>
        /// <param name="dllPath">dll文件路径</param>
        /// <param name="clsid">clsid</param>
        /// <param name="comFallback">comFallback</param>
        /// <returns></returns>
        public object CreateObjectFromPath(string dllPath, Guid clsid, bool comFallback)
        {
            return CreateObjectFromPath(dllPath, clsid, false, comFallback);
        }

        /// <summary>
        /// 从dll文件创建对象
        /// </summary>
        /// <param name="dllPath">dll文件路径</param>
        /// <param name="clsid">clsid</param>
        /// <param name="setSearchPath">设置搜索路径</param>
        /// <param name="comFallback">comFallback</param>
        /// <returns></returns>
        public object CreateObjectFromPath(string dllPath, Guid clsid, bool setSearchPath, bool comFallback)
        {
            string fullDllPath = Path.Combine(dllPath);
            if (File.Exists(fullDllPath) && (_preferURObjects || !comFallback))
            {
                if (setSearchPath)
                {
                    NativeMethods.SetDllDirectory(Path.GetDirectoryName(fullDllPath));
                }
                _lib = NativeMethods.LoadLibrary(fullDllPath);
                if (setSearchPath)
                {
                    NativeMethods.SetDllDirectory(null);
                }
                if (_lib != IntPtr.Zero)
                {
                    //we need to cache the handle so the COM object will work and we can clean up later
                    IntPtr ptr = NativeMethods.GetProcAddress(_lib, "DllGetClassObject");
                    if (ptr != IntPtr.Zero)
                    {
                        DllGETCLASSOBJECTInvoker invoker =
                            Marshal.GetDelegateForFunctionPointer(ptr, typeof(DllGETCLASSOBJECTInvoker)) as DllGETCLASSOBJECTInvoker;
                        if (invoker != null)
                        {
                            object unknown;
                            int hr = invoker(clsid, IID_IUnknown, out unknown);
                            if (hr >= 0)
                            {
                                IClassFactory factory = unknown as IClassFactory;
                                if (factory != null)
                                {
                                    object createdObject;
                                    factory.CreateInstance(null, IID_IUnknown, out createdObject);
                                    return createdObject;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Win32Exception();
                    }
                }
                else if (comFallback)
                {
                    Type type = Type.GetTypeFromCLSID(clsid);
                    return Activator.CreateInstance(type);
                }
                else
                {
                    throw new Win32Exception();
                }
            }
            else if (comFallback)
            {
                Type type = Type.GetTypeFromCLSID(clsid);
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public void Dispose()
        {
            NativeMethods.FreeLibrary(_lib);
            GC.SuppressFinalize(this);
        }
    }
}