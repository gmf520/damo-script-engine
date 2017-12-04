using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.CompilerServices;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace Qf
{

    /// <summary>
    /// C#清风免注册表调用DM插件 For 6.1720
    /// QQ：274838061
    /// 版本:1.0
    /// 创建时间:2017-09-04 21:21:18
    /// 直接调用，不需要注册到系统
    /// 调用方法： 
    /// QfDmSoft dm = new QfDmSoft();
    /// MessageBox.Show(dm.Ver());
    /// string path = dm.GetBasePath();
    /// </summary>
    public class QfDmSoft : IDisposable
    {
        bool _disposed = false;
        URCOMLoader _urCom;
        IDmsoft _dm;
        string _dmPath;
        delegate int DllGETCLASSOBJECTInvoker([MarshalAs(UnmanagedType.LPStruct)]Guid clsid, [MarshalAs(UnmanagedType.LPStruct)]Guid iid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        
        IntPtr _lib = IntPtr.Zero;

        public object CreateObjectFromPath(string dllPath, Guid clsid)
        {
            object createdObject = null;
            //IntPtr lib = IntPtr.Zero;
            string fullDllPath = Path.Combine("", dllPath);

            if (File.Exists(fullDllPath))
            {                
                _lib = NativeMethods.LoadLibrary(fullDllPath);              
                if (_lib != IntPtr.Zero)
                {                    
                    IntPtr fnP = NativeMethods.GetProcAddress(_lib, "DllGetClassObject");
                    if (fnP != IntPtr.Zero)
                    {
                        DllGETCLASSOBJECTInvoker fn = Marshal.GetDelegateForFunctionPointer(fnP, typeof(DllGETCLASSOBJECTInvoker)) as DllGETCLASSOBJECTInvoker;

                        object pUnk = null;
                        int hr = fn(clsid, IID_IUnknown, out pUnk);
                        if (hr >= 0)
                        {
                            IClassFactory pCF = pUnk as IClassFactory;
                            if (pCF != null)
                            {
                                hr = pCF.CreateInstance(null, IID_IUnknown, out createdObject);


                            }
                        }
                    }
                    else
                    {
                        throw new Win32Exception();
                    }
                }               
                else
                {
                    throw new Win32Exception();
                }
            }
            else 
            {
                Type type = Type.GetTypeFromCLSID(clsid);
                return Activator.CreateInstance(type);
            }

            return createdObject;
        }
       
        /// <summary>
        /// 创建免注册表大漠实例
        /// </summary>
        /// <param name="dmpath"></param>
        public QfDmSoft(string dmpath = "dm.dll") 
        {
            _dmPath = dmpath;            
            _urCom = new URCOMLoader();
            _dm = CreateObjectFromPath(dmpath, Guid.Parse("26037A0E-7CBD-4FFF-9C63-56F2D0770214")) as IDmsoft;
           

        }

        public IDmsoft DM
        {
            get { return _dm; }
        }

        /// <summary>
        /// 大漠插件地址
        /// </summary>
        public string DmPath
        {
            get { return _dmPath; }
        }

        #region 清风大漠免注册方法封装
        public int FoobarClearText(int hwnd)
        {
            return _dm.FoobarClearText(hwnd);

        }

        public int FindWindow(string class_name,string title_name)
        {
            return _dm.FindWindow(class_name,title_name);

        }

        public int FindMultiColor(int x1,int y1,int x2,int y2,string first_color,string offset_color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindMultiColor(x1,y1,x2,y2,first_color,offset_color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string FindPicEx(int x1,int y1,int x2,int y2,string pic_name,string delta_color,double sim,int dir)
        {
            return _dm.FindPicEx(x1,y1,x2,y2,pic_name,delta_color,sim,dir);

        }

        public int GetWindow(int hwnd,int flag)
        {
            return _dm.GetWindow(hwnd,flag);

        }

        public string GetDir(int tpe)
        {
            return _dm.GetDir(tpe);

        }

        public string GetWindowTitle(int hwnd)
        {
            return _dm.GetWindowTitle(hwnd);

        }

        public int ClearDict(int index)
        {
            return _dm.ClearDict(index);

        }

        public int UseDict(int index)
        {
            return _dm.UseDict(index);

        }

        public int SetMouseSpeed(int speed)
        {
            return _dm.SetMouseSpeed(speed);

        }

        public int GetCursorPos(out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.GetCursorPos(out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string SelectDirectory()
        {
            return _dm.SelectDirectory();

        }

        public int SetWindowText(int hwnd,string text)
        {
            return _dm.SetWindowText(hwnd,text);

        }

        public int WriteIniPwd(string section,string key,string v,string file_name,string pwd)
        {
            return _dm.WriteIniPwd(section,key,v,file_name,pwd);

        }

        public int CaptureJpg(int x1,int y1,int x2,int y2,string file_name,int quality)
        {
            return _dm.CaptureJpg(x1,y1,x2,y2,file_name,quality);

        }

        public string GetPicSize(string pic_name)
        {
            return _dm.GetPicSize(pic_name);

        }

        public string EnumWindow(int parent,string title,string class_name,int filter)
        {
            return _dm.EnumWindow(parent,title,class_name,filter);

        }

        public int DisableScreenSave()
        {
            return _dm.DisableScreenSave();

        }

        public int MoveFile(string src_file,string dst_file)
        {
            return _dm.MoveFile(src_file,dst_file);

        }

        public int EnableSpeedDx(int en)
        {
            return _dm.EnableSpeedDx(en);

        }

        public int GetNowDict()
        {
            return _dm.GetNowDict();

        }

        public int ExitOs(int tpe)
        {
            return _dm.ExitOs(tpe);

        }

        public int FoobarTextRect(int hwnd,int x,int y,int w,int h)
        {
            return _dm.FoobarTextRect(hwnd,x,y,w,h);

        }

        public int SetMouseDelay(string tpe,int delay)
        {
            return _dm.SetMouseDelay(tpe,delay);

        }

        public int AsmClear()
        {
            return _dm.AsmClear();

        }

        public int FaqGetSize(int handle)
        {
            return _dm.FaqGetSize(handle);

        }

        public string GetEnv(int index,string name)
        {
            return _dm.GetEnv(index,name);

        }

        public int GetPointWindow(int x,int y)
        {
            return _dm.GetPointWindow(x,y);

        }

        public string ReadFileData(string file_name,int start_pos,int end_pos)
        {
            return _dm.ReadFileData(file_name,start_pos,end_pos);

        }

        public int CapturePng(int x1,int y1,int x2,int y2,string file_name)
        {
            return _dm.CapturePng(x1,y1,x2,y2,file_name);

        }

        public int Reg(string code,string Ver)
        {
            return _dm.Reg(code,Ver);

        }

        public int MoveR(int rx,int ry)
        {
            return _dm.MoveR(rx,ry);

        }

        public int RightClick()
        {
            return _dm.RightClick();

        }

        public int EncodeFile(string file_name,string pwd)
        {
            return _dm.EncodeFile(file_name,pwd);

        }

        public string FindData(int hwnd,string addr_range,string data)
        {
            return _dm.FindData(hwnd,addr_range,data);

        }

        public int TerminateProcess(int pid)
        {
            return _dm.TerminateProcess(pid);

        }

        public string EnumIniKeyPwd(string section,string file_name,string pwd)
        {
            return _dm.EnumIniKeyPwd(section,file_name,pwd);

        }

        public int SetWordGapNoDict(int word_gap)
        {
            return _dm.SetWordGapNoDict(word_gap);

        }

        public int CreateFolder(string folder_name)
        {
            return _dm.CreateFolder(folder_name);

        }

        public int GetID()
        {
            return _dm.GetID();

        }

        public int SetMinColGap(int col_gap)
        {
            return _dm.SetMinColGap(col_gap);

        }

        public int Capture(int x1,int y1,int x2,int y2,string file_name)
        {
            return _dm.Capture(x1,y1,x2,y2,file_name);

        }

        public string EnumIniSectionPwd(string file_name,string pwd)
        {
            return _dm.EnumIniSectionPwd(file_name,pwd);

        }

        public int GetWindowRect(int hwnd,out int x1,out int y1,out int x2,out int y2)
        {
            object ox1;
            object oy1;
            object ox2;
            object oy2;
            int result = _dm.GetWindowRect(hwnd,out ox1,out oy1,out ox2,out oy2);
            x1 = (int)ox1;
            y1 = (int)oy1;
            x2 = (int)ox2;
            y2 = (int)oy2;
            return result;

        }

        public int GetOsType()
        {
            return _dm.GetOsType();

        }

        public string FindInt(int hwnd,string addr_range,int int_value_min,int int_value_max,int tpe)
        {
            return _dm.FindInt(hwnd,addr_range,int_value_min,int_value_max,tpe);

        }

        public string GetCursorShape()
        {
            return _dm.GetCursorShape();

        }

        public int FindMulColor(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.FindMulColor(x1,y1,x2,y2,color,sim);

        }

        public string FindPicMemE(int x1,int y1,int x2,int y2,string pic_info,string delta_color,double sim,int dir)
        {
            return _dm.FindPicMemE(x1,y1,x2,y2,pic_info,delta_color,sim,dir);

        }

        public int FindWindowEx(int parent,string class_name,string title_name)
        {
            return _dm.FindWindowEx(parent,class_name,title_name);

        }

        public int FreeProcessMemory(int hwnd)
        {
            return _dm.FreeProcessMemory(hwnd);

        }

        public int Delays(int min_s,int max_s)
        {
            return _dm.Delays(min_s,max_s);

        }

        public int LockInput(int locks)
        {
            return _dm.LockInput(locks);

        }

        public int MoveTo(int x,int y)
        {
            return _dm.MoveTo(x,y);

        }

        public int EnableMouseMsg(int en)
        {
            return _dm.EnableMouseMsg(en);

        }

        public int WriteString(int hwnd,string addr,int tpe,string v)
        {
            return _dm.WriteString(hwnd,addr,tpe,v);

        }

        public int GetColorNum(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.GetColorNum(x1,y1,x2,y2,color,sim);

        }

        public string OcrEx(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.OcrEx(x1,y1,x2,y2,color,sim);

        }

        public int CopyFile(string src_file,string dst_file,int over)
        {
            return _dm.CopyFile(src_file,dst_file,over);

        }

        public int SetMinRowGap(int row_gap)
        {
            return _dm.SetMinRowGap(row_gap);

        }

        public string GetWordsNoDict(int x1,int y1,int x2,int y2,string color)
        {
            return _dm.GetWordsNoDict(x1,y1,x2,y2,color);

        }

        public int CheckInputMethod(int hwnd,string id)
        {
            return _dm.CheckInputMethod(hwnd,id);

        }

        public int LockDisplay(int locks)
        {
            return _dm.LockDisplay(locks);

        }

        public string GetDict(int index,int font_index)
        {
            return _dm.GetDict(index,font_index);

        }

        public string FindDoubleEx(int hwnd,string addr_range,double double_value_min,double double_value_max,int steps,int multi_thread,int mode)
        {
            return _dm.FindDoubleEx(hwnd,addr_range,double_value_min,double_value_max,steps,multi_thread,mode);

        }

        public int RunApp(string path,int mode)
        {
            return _dm.RunApp(path,mode);

        }

        public int WriteDoubleAddr(int hwnd,int addr,double v)
        {
            return _dm.WriteDoubleAddr(hwnd,addr,v);

        }

        public int KeyPressStr(string key_str,int delay)
        {
            return _dm.KeyPressStr(key_str,delay);

        }

        public int KeyUpChar(string key_str)
        {
            return _dm.KeyUpChar(key_str);

        }

        public int VirtualFreeEx(int hwnd,int addr)
        {
            return _dm.VirtualFreeEx(hwnd,addr);

        }

        public int GetResultCount(string str)
        {
            return _dm.GetResultCount(str);

        }

        public int GetLastError()
        {
            return _dm.GetLastError();

        }

        public int RightUp()
        {
            return _dm.RightUp();

        }

        public int ActiveInputMethod(int hwnd,string id)
        {
            return _dm.ActiveInputMethod(hwnd,id);

        }

        public int FoobarUpdate(int hwnd)
        {
            return _dm.FoobarUpdate(hwnd);

        }

        public int AsmAdd(string asm_ins)
        {
            return _dm.AsmAdd(asm_ins);

        }

        public int KeyDown(int vk)
        {
            return _dm.KeyDown(vk);

        }

        public string Md5(string str)
        {
            return _dm.Md5(str);

        }

        public int GetTime()
        {
            return _dm.GetTime();

        }

        public string GetCommandLine(int hwnd)
        {
            return _dm.GetCommandLine(hwnd);

        }

        public int FindWindowByProcess(string process_name,string class_name,string title_name)
        {
            return _dm.FindWindowByProcess(process_name,class_name,title_name);

        }

        public int SetPicPwd(string pwd)
        {
            return _dm.SetPicPwd(pwd);

        }

        public string AsmCode(int base_addr)
        {
            return _dm.AsmCode(base_addr);

        }

        public int SetScreen(int width,int height,int depth)
        {
            return _dm.SetScreen(width,height,depth);

        }

        public int CreateFoobarRect(int hwnd,int x,int y,int w,int h)
        {
            return _dm.CreateFoobarRect(hwnd,x,y,w,h);

        }

        public string FindStrFastE(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrFastE(x1,y1,x2,y2,str,color,sim);

        }

        public int SendString2(int hwnd,string str)
        {
            return _dm.SendString2(hwnd,str);

        }

        public int GetClientRect(int hwnd,out int x1,out int y1,out int x2,out int y2)
        {
            object ox1;
            object oy1;
            object ox2;
            object oy2;
            int result = _dm.GetClientRect(hwnd,out ox1,out oy1,out ox2,out oy2);
            x1 = (int)ox1;
            y1 = (int)oy1;
            x2 = (int)ox2;
            y2 = (int)oy2;
            return result;

        }

        public string GetNetTimeByIp(string ip)
        {
            return _dm.GetNetTimeByIp(ip);

        }

        public int Beep(int fre,int delay)
        {
            return _dm.Beep(fre,delay);

        }

        public int GetMousePointWindow()
        {
            return _dm.GetMousePointWindow();

        }

        public int RegEx(string code,string Ver,string ip)
        {
            return _dm.RegEx(code,Ver,ip);

        }

        public string ExcludePos(string all_pos,int tpe,int x1,int y1,int x2,int y2)
        {
            return _dm.ExcludePos(all_pos,tpe,x1,y1,x2,y2);

        }

        public int CheckUAC()
        {
            return _dm.CheckUAC();

        }

        public string GetCursorShapeEx(int tpe)
        {
            return _dm.GetCursorShapeEx(tpe);

        }

        public string EnumWindowByProcess(string process_name,string title,string class_name,int filter)
        {
            return _dm.EnumWindowByProcess(process_name,title,class_name,filter);

        }

        public string ReadFile(string file_name)
        {
            return _dm.ReadFile(file_name);

        }

        public int RightDown()
        {
            return _dm.RightDown();

        }

        public int Stop(int id)
        {
            return _dm.Stop(id);

        }

        public int FoobarClose(int hwnd)
        {
            return _dm.FoobarClose(hwnd);

        }

        public int RegNoMac(string code,string Ver)
        {
            return _dm.RegNoMac(code,Ver);

        }

        public int LoadPic(string pic_name)
        {
            return _dm.LoadPic(pic_name);

        }

        public string AppendPicAddr(string pic_info,int addr,int size)
        {
            return _dm.AppendPicAddr(pic_info,addr,size);

        }

        public int WriteFloatAddr(int hwnd,int addr,float v)
        {
            return _dm.WriteFloatAddr(hwnd,addr,v);

        }

        public string FindStrEx(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrEx(x1,y1,x2,y2,str,color,sim);

        }

        public string Ver()
        {
            return _dm.Ver();

        }

        public string MatchPicName(string pic_name)
        {
            return _dm.MatchPicName(pic_name);

        }

        public int OpenProcess(int pid)
        {
            return _dm.OpenProcess(pid);

        }

        public int SwitchBindWindow(int hwnd)
        {
            return _dm.SwitchBindWindow(hwnd);

        }

        public int SetWindowTransparent(int hwnd,int v)
        {
            return _dm.SetWindowTransparent(hwnd,v);

        }

        public int AsmCall(int hwnd,int mode)
        {
            return _dm.AsmCall(hwnd,mode);

        }

        public int SendString(int hwnd,string str)
        {
            return _dm.SendString(hwnd,str);

        }

        public int FaqRelease(int handle)
        {
            return _dm.FaqRelease(handle);

        }

        public int SetClientSize(int hwnd,int width,int height)
        {
            return _dm.SetClientSize(hwnd,width,height);

        }

        public int SetWindowState(int hwnd,int flag)
        {
            return _dm.SetWindowState(hwnd,flag);

        }

        public int SetDictMem(int index,int addr,int size)
        {
            return _dm.SetDictMem(index,addr,size);

        }

        public string FindPicExS(int x1,int y1,int x2,int y2,string pic_name,string delta_color,double sim,int dir)
        {
            return _dm.FindPicExS(x1,y1,x2,y2,pic_name,delta_color,sim,dir);

        }

        public int FoobarDrawLine(int hwnd,int x1,int y1,int x2,int y2,string color,int style,int width)
        {
            return _dm.FoobarDrawLine(hwnd,x1,y1,x2,y2,color,style,width);

        }

        public int GetClientSize(int hwnd,out int width,out int height)
        {
            object owidth;
            object oheight;
            int result = _dm.GetClientSize(hwnd,out owidth,out oheight);
            width = (int)owidth;
            height = (int)oheight;
            return result;

        }

        public string OcrInFile(int x1,int y1,int x2,int y2,string pic_name,string color,double sim)
        {
            return _dm.OcrInFile(x1,y1,x2,y2,pic_name,color,sim);

        }

        public string GetDictInfo(string str,string font_name,int font_size,int flag)
        {
            return _dm.GetDictInfo(str,font_name,font_size,flag);

        }

        public int GetSpecialWindow(int flag)
        {
            return _dm.GetSpecialWindow(flag);

        }

        public string BGR2RGB(string bgr_color)
        {
            return _dm.BGR2RGB(bgr_color);

        }

        public int DeleteFolder(string folder_name)
        {
            return _dm.DeleteFolder(folder_name);

        }

        public int WriteDouble(int hwnd,string addr,double v)
        {
            return _dm.WriteDouble(hwnd,addr,v);

        }

        public int DmGuard(int en,string tpe)
        {
            return _dm.DmGuard(en,tpe);

        }

        public int FoobarPrintText(int hwnd,string text,string color)
        {
            return _dm.FoobarPrintText(hwnd,text,color);

        }

        public string GetColor(int x,int y)
        {
            return _dm.GetColor(x,y);

        }

        public int SetWordLineHeight(int line_height)
        {
            return _dm.SetWordLineHeight(line_height);

        }

        public string FindMultiColorE(int x1,int y1,int x2,int y2,string first_color,string offset_color,double sim,int dir)
        {
            return _dm.FindMultiColorE(x1,y1,x2,y2,first_color,offset_color,sim,dir);

        }

        public string GetColorHSV(int x,int y)
        {
            return _dm.GetColorHSV(x,y);

        }

        public string GetDiskSerial()
        {
            return _dm.GetDiskSerial();

        }

        public int FaqPost(string server,int handle,int request_type,int time_out)
        {
            return _dm.FaqPost(server,handle,request_type,time_out);

        }

        public int DownCpu(int rate)
        {
            return _dm.DownCpu(rate);

        }

        public int SendPaste(int hwnd)
        {
            return _dm.SendPaste(hwnd);

        }

        public int SetUAC(int uac)
        {
            return _dm.SetUAC(uac);

        }

        public float ReadFloat(int hwnd,string addr)
        {
            return _dm.ReadFloat(hwnd,addr);

        }

        public string ReadString(int hwnd,string addr,int tpe,int length)
        {
            return _dm.ReadString(hwnd,addr,tpe,length);

        }

        public int FreeScreenData(int handle)
        {
            return _dm.FreeScreenData(handle);

        }

        public int SetWindowSize(int hwnd,int width,int height)
        {
            return _dm.SetWindowSize(hwnd,width,height);

        }

        public int KeyPressChar(string key_str)
        {
            return _dm.KeyPressChar(key_str);

        }

        public int GetScreenWidth()
        {
            return _dm.GetScreenWidth();

        }

        public string RGB2BGR(string rgb_color)
        {
            return _dm.RGB2BGR(rgb_color);

        }

        public int MoveWindow(int hwnd,int x,int y)
        {
            return _dm.MoveWindow(hwnd,x,y);

        }

        public int FindShape(int x1,int y1,int x2,int y2,string offset_color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindShape(x1,y1,x2,y2,offset_color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int DecodeFile(string file_name,string pwd)
        {
            return _dm.DecodeFile(file_name,pwd);

        }

        public int MoveDD(int dx,int dy)
        {
            return _dm.MoveDD(dx,dy);

        }

        public string FindStrWithFontEx(int x1,int y1,int x2,int y2,string str,string color,double sim,string font_name,int font_size,int flag)
        {
            return _dm.FindStrWithFontEx(x1,y1,x2,y2,str,color,sim,font_name,font_size,flag);

        }

        public string GetMachineCode()
        {
            return _dm.GetMachineCode();

        }

        public int LoadPicByte(int addr,int size,string name)
        {
            return _dm.LoadPicByte(addr,size,name);

        }

        public int GetWindowState(int hwnd,int flag)
        {
            return _dm.GetWindowState(hwnd,flag);

        }

        public int ForceUnBindWindow(int hwnd)
        {
            return _dm.ForceUnBindWindow(hwnd);

        }

        public int WaitKey(int key_code,int time_out)
        {
            return _dm.WaitKey(key_code,time_out);

        }

        public int WheelUp()
        {
            return _dm.WheelUp();

        }

        public int FreePic(string pic_name)
        {
            return _dm.FreePic(pic_name);

        }

        public int LeftUp()
        {
            return _dm.LeftUp();

        }

        public int FaqCancel()
        {
            return _dm.FaqCancel();

        }

        public string FindStrFastS(int x1,int y1,int x2,int y2,string str,string color,double sim,out int x,out int y)
        {
            object ox;
            object oy;
            string result = _dm.FindStrFastS(x1,y1,x2,y2,str,color,sim,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string DoubleToData(double double_value)
        {
            return _dm.DoubleToData(double_value);

        }

        public int StrStr(string s,string str)
        {
            return _dm.StrStr(s,str);

        }

        public int EnableRealKeypad(int en)
        {
            return _dm.EnableRealKeypad(en);

        }

        public int InitCri()
        {
            return _dm.InitCri();

        }

        public int LeftDown()
        {
            return _dm.LeftDown();

        }

        public int EnableMouseAccuracy(int en)
        {
            return _dm.EnableMouseAccuracy(en);

        }

        public int GetBindWindow()
        {
            return _dm.GetBindWindow();

        }

        public int IsBind(int hwnd)
        {
            return _dm.IsBind(hwnd);

        }

        public int KeyPress(int vk)
        {
            return _dm.KeyPress(vk);

        }

        public int GetForegroundFocus()
        {
            return _dm.GetForegroundFocus();

        }

        public double ReadDouble(int hwnd,string addr)
        {
            return _dm.ReadDouble(hwnd,addr);

        }

        public int EnterCri()
        {
            return _dm.EnterCri();

        }

        public string ReadStringAddr(int hwnd,int addr,int tpe,int length)
        {
            return _dm.ReadStringAddr(hwnd,addr,tpe,length);

        }

        public string GetNetTime()
        {
            return _dm.GetNetTime();

        }

        public string GetWordResultStr(string str,int index)
        {
            return _dm.GetWordResultStr(str,index);

        }

        public int EnableFontSmooth()
        {
            return _dm.EnableFontSmooth();

        }

        public string FindColorBlockEx(int x1,int y1,int x2,int y2,string color,double sim,int count,int width,int height)
        {
            return _dm.FindColorBlockEx(x1,y1,x2,y2,color,sim,count,width,height);

        }

        public int EnableDisplayDebug(int enable_debug)
        {
            return _dm.EnableDisplayDebug(enable_debug);

        }

        public string GetDisplayInfo()
        {
            return _dm.GetDisplayInfo();

        }

        public int CmpColor(int x,int y,string color,double sim)
        {
            return _dm.CmpColor(x,y,color,sim);

        }

        public int WheelDown()
        {
            return _dm.WheelDown();

        }

        public int FoobarLock(int hwnd)
        {
            return _dm.FoobarLock(hwnd);

        }

        public int WriteInt(int hwnd,string addr,int tpe,int v)
        {
            return _dm.WriteInt(hwnd,addr,tpe,v);

        }

        public int LeaveCri()
        {
            return _dm.LeaveCri();

        }

        public string GetColorBGR(int x,int y)
        {
            return _dm.GetColorBGR(x,y);

        }

        public int IsDisplayDead(int x1,int y1,int x2,int y2,int t)
        {
            return _dm.IsDisplayDead(x1,y1,x2,y2,t);

        }

        public string GetWindowProcessPath(int hwnd)
        {
            return _dm.GetWindowProcessPath(hwnd);

        }

        public int CaptureGif(int x1,int y1,int x2,int y2,string file_name,int delay,int time)
        {
            return _dm.CaptureGif(x1,y1,x2,y2,file_name,delay,time);

        }

        public int FoobarStartGif(int hwnd,int x,int y,string pic_name,int repeat_limit,int delay)
        {
            return _dm.FoobarStartGif(hwnd,x,y,pic_name,repeat_limit,delay);

        }

        public string FindFloat(int hwnd,string addr_range,float float_value_min,float float_value_max)
        {
            return _dm.FindFloat(hwnd,addr_range,float_value_min,float_value_max);

        }

        public int FoobarSetFont(int hwnd,string font_name,int size,int flag)
        {
            return _dm.FoobarSetFont(hwnd,font_name,size,flag);

        }

        public int AddDict(int index,string dict_info)
        {
            return _dm.AddDict(index,dict_info);

        }

        public int ShowScrMsg(int x1,int y1,int x2,int y2,string msg,string color)
        {
            return _dm.ShowScrMsg(x1,y1,x2,y2,msg,color);

        }

        public string ReadIni(string section,string key,string file_name)
        {
            return _dm.ReadIni(section,key,file_name);

        }

        public int FaqCaptureFromFile(int x1,int y1,int x2,int y2,string file_name,int quality)
        {
            return _dm.FaqCaptureFromFile(x1,y1,x2,y2,file_name,quality);

        }

        public string FindDataEx(int hwnd,string addr_range,string data,int steps,int multi_thread,int mode)
        {
            return _dm.FindDataEx(hwnd,addr_range,data,steps,multi_thread,mode);

        }

        public int WriteStringAddr(int hwnd,int addr,int tpe,string v)
        {
            return _dm.WriteStringAddr(hwnd,addr,tpe,v);

        }

        public int BindWindowEx(int hwnd,string display,string mouse,string keypad,string public_desc,int mode)
        {
            return _dm.BindWindowEx(hwnd,display,mouse,keypad,public_desc,mode);

        }

        public int ReadInt(int hwnd,string addr,int tpe)
        {
            return _dm.ReadInt(hwnd,addr,tpe);

        }

        public string ReadData(int hwnd,string addr,int length)
        {
            return _dm.ReadData(hwnd,addr,length);

        }

        public int FindStrWithFont(int x1,int y1,int x2,int y2,string str,string color,double sim,string font_name,int font_size,int flag,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindStrWithFont(x1,y1,x2,y2,str,color,sim,font_name,font_size,flag,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int SetEnumWindowDelay(int delay)
        {
            return _dm.SetEnumWindowDelay(delay);

        }

        public string GetInfo(string cmd,string param)
        {
            return _dm.GetInfo(cmd,param);

        }

        public string GetProcessInfo(int pid)
        {
            return _dm.GetProcessInfo(pid);

        }

        public int RegExNoMac(string code,string Ver,string ip)
        {
            return _dm.RegExNoMac(code,Ver,ip);

        }

        public string GetBasePath()
        {
            return _dm.GetBasePath();

        }

        public int FindStrFast(int x1,int y1,int x2,int y2,string str,string color,double sim,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindStrFast(x1,y1,x2,y2,str,color,sim,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int IsFileExist(string file_name)
        {
            return _dm.IsFileExist(file_name);

        }

        public string GetCursorSpot()
        {
            return _dm.GetCursorSpot();

        }

        public int CreateFoobarEllipse(int hwnd,int x,int y,int w,int h)
        {
            return _dm.CreateFoobarEllipse(hwnd,x,y,w,h);

        }

        public int GetWordResultPos(string str,int index,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.GetWordResultPos(str,index,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string GetMachineCodeNoMac()
        {
            return _dm.GetMachineCodeNoMac();

        }

        public string FindShapeEx(int x1,int y1,int x2,int y2,string offset_color,double sim,int dir)
        {
            return _dm.FindShapeEx(x1,y1,x2,y2,offset_color,sim,dir);

        }

        public int DeleteIni(string section,string key,string file_name)
        {
            return _dm.DeleteIni(section,key,file_name);

        }

        public string GetWords(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.GetWords(x1,y1,x2,y2,color,sim);

        }

        public int GetWordResultCount(string str)
        {
            return _dm.GetWordResultCount(str);

        }

        public int SetShowErrorMsg(int show)
        {
            return _dm.SetShowErrorMsg(show);

        }

        public int SetSimMode(int mode)
        {
            return _dm.SetSimMode(mode);

        }

        public int DisablePowerSave()
        {
            return _dm.DisablePowerSave();

        }

        public int GetDmCount()
        {
            return _dm.GetDmCount();

        }

        public int SetWordGap(int word_gap)
        {
            return _dm.SetWordGap(word_gap);

        }

        public int SaveDict(int index,string file_name)
        {
            return _dm.SaveDict(index,file_name);

        }

        public double ReadDoubleAddr(int hwnd,int addr)
        {
            return _dm.ReadDoubleAddr(hwnd,addr);

        }

        public string GetPath()
        {
            return _dm.GetPath();

        }

        public int FoobarTextLineGap(int hwnd,int gap)
        {
            return _dm.FoobarTextLineGap(hwnd,gap);

        }

        public string FloatToData(float float_value)
        {
            return _dm.FloatToData(float_value);

        }

        public int GetScreenHeight()
        {
            return _dm.GetScreenHeight();

        }

        public int UnBindWindow()
        {
            return _dm.UnBindWindow();

        }

        public int EnableIme(int en)
        {
            return _dm.EnableIme(en);

        }

        public int SetMemoryFindResultToFile(string file_name)
        {
            return _dm.SetMemoryFindResultToFile(file_name);

        }

        public string FindStrE(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrE(x1,y1,x2,y2,str,color,sim);

        }

        public int VirtualAllocEx(int hwnd,int addr,int size,int tpe)
        {
            return _dm.VirtualAllocEx(hwnd,addr,size,tpe);

        }

        public string EnumIniKey(string section,string file_name)
        {
            return _dm.EnumIniKey(section,file_name);

        }

        public string FindDouble(int hwnd,string addr_range,double double_value_min,double double_value_max)
        {
            return _dm.FindDouble(hwnd,addr_range,double_value_min,double_value_max);

        }

        public int MiddleClick()
        {
            return _dm.MiddleClick();

        }

        public int CheckFontSmooth()
        {
            return _dm.CheckFontSmooth();

        }

        public int SetAero(int en)
        {
            return _dm.SetAero(en);

        }

        public int WriteFile(string file_name,string content)
        {
            return _dm.WriteFile(file_name,content);

        }

        public string EnumIniSection(string file_name)
        {
            return _dm.EnumIniSection(file_name);

        }

        public int FoobarDrawPic(int hwnd,int x,int y,string pic,string trans_color)
        {
            return _dm.FoobarDrawPic(hwnd,x,y,pic,trans_color);

        }

        public int GetScreenDataBmp(int x1,int y1,int x2,int y2,out int data,out int size)
        {
            object odata;
            object osize;
            int result = _dm.GetScreenDataBmp(x1,y1,x2,y2,out odata,out osize);
            data = (int)odata;
            size = (int)osize;
            return result;

        }

        public int GetModuleBaseAddr(int hwnd,string module_name)
        {
            return _dm.GetModuleBaseAddr(hwnd,module_name);

        }

        public int EnableMouseSync(int en,int time_out)
        {
            return _dm.EnableMouseSync(en,time_out);

        }

        public string ReadDataAddr(int hwnd,int addr,int length)
        {
            return _dm.ReadDataAddr(hwnd,addr,length);

        }

        public string FindStrFastExS(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrFastExS(x1,y1,x2,y2,str,color,sim);

        }

        public int KeyDownChar(string key_str)
        {
            return _dm.KeyDownChar(key_str);

        }

        public int MiddleDown()
        {
            return _dm.MiddleDown();

        }

        public int WriteFloat(int hwnd,string addr,float v)
        {
            return _dm.WriteFloat(hwnd,addr,v);

        }

        public string FindIntEx(int hwnd,string addr_range,int int_value_min,int int_value_max,int tpe,int steps,int multi_thread,int mode)
        {
            return _dm.FindIntEx(hwnd,addr_range,int_value_min,int_value_max,tpe,steps,multi_thread,mode);

        }

        public string FindPicE(int x1,int y1,int x2,int y2,string pic_name,string delta_color,double sim,int dir)
        {
            return _dm.FindPicE(x1,y1,x2,y2,pic_name,delta_color,sim,dir);

        }

        public string FetchWord(int x1,int y1,int x2,int y2,string color,string word)
        {
            return _dm.FetchWord(x1,y1,x2,y2,color,word);

        }

        public string FindFloatEx(int hwnd,string addr_range,float float_value_min,float float_value_max,int steps,int multi_thread,int mode)
        {
            return _dm.FindFloatEx(hwnd,addr_range,float_value_min,float_value_max,steps,multi_thread,mode);

        }

        public string FindStrS(int x1,int y1,int x2,int y2,string str,string color,double sim,out int x,out int y)
        {
            object ox;
            object oy;
            string result = _dm.FindStrS(x1,y1,x2,y2,str,color,sim,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int GetMouseSpeed()
        {
            return _dm.GetMouseSpeed();

        }

        public int WriteIni(string section,string key,string v,string file_name)
        {
            return _dm.WriteIni(section,key,v,file_name);

        }

        public string OcrExOne(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.OcrExOne(x1,y1,x2,y2,color,sim);

        }

        public int LockMouseRect(int x1,int y1,int x2,int y2)
        {
            return _dm.LockMouseRect(x1,y1,x2,y2);

        }

        public int GetScreenData(int x1,int y1,int x2,int y2)
        {
            return _dm.GetScreenData(x1,y1,x2,y2);

        }

        public int EnableKeypadPatch(int en)
        {
            return _dm.EnableKeypadPatch(en);

        }

        public string ReadIniPwd(string section,string key,string file_name,string pwd)
        {
            return _dm.ReadIniPwd(section,key,file_name,pwd);

        }

        public string GetClipboard()
        {
            return _dm.GetClipboard();

        }

        public int DelEnv(int index,string name)
        {
            return _dm.DelEnv(index,name);

        }

        public int FindStr(int x1,int y1,int x2,int y2,string str,string color,double sim,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindStr(x1,y1,x2,y2,str,color,sim,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string StringToData(string string_value,int tpe)
        {
            return _dm.StringToData(string_value,tpe);

        }

        public int SetRowGapNoDict(int row_gap)
        {
            return _dm.SetRowGapNoDict(row_gap);

        }

        public string GetWindowClass(int hwnd)
        {
            return _dm.GetWindowClass(hwnd);

        }

        public int FoobarFillRect(int hwnd,int x1,int y1,int x2,int y2,string color)
        {
            return _dm.FoobarFillRect(hwnd,x1,y1,x2,y2,color);

        }

        public string GetAveHSV(int x1,int y1,int x2,int y2)
        {
            return _dm.GetAveHSV(x1,y1,x2,y2);

        }

        public int SetPath(string path)
        {
            return _dm.SetPath(path);

        }

        public int LeftDoubleClick()
        {
            return _dm.LeftDoubleClick();

        }

        public int CreateFoobarCustom(int hwnd,int x,int y,string pic,string trans_color,double sim)
        {
            return _dm.CreateFoobarCustom(hwnd,x,y,pic,trans_color,sim);

        }

        public int SetDictPwd(string pwd)
        {
            return _dm.SetDictPwd(pwd);

        }

        public int SetDisplayAcceler(int level)
        {
            return _dm.SetDisplayAcceler(level);

        }

        public int MiddleUp()
        {
            return _dm.MiddleUp();

        }

        public int EnableKeypadSync(int en,int time_out)
        {
            return _dm.EnableKeypadSync(en,time_out);

        }

        public int EnableFakeActive(int en)
        {
            return _dm.EnableFakeActive(en);

        }

        public float ReadFloatAddr(int hwnd,int addr)
        {
            return _dm.ReadFloatAddr(hwnd,addr);

        }

        public string GetMac()
        {
            return _dm.GetMac();

        }

        public string FindStrWithFontE(int x1,int y1,int x2,int y2,string str,string color,double sim,string font_name,int font_size,int flag)
        {
            return _dm.FindStrWithFontE(x1,y1,x2,y2,str,color,sim,font_name,font_size,flag);

        }

        public int SetMemoryHwndAsProcessId(int en)
        {
            return _dm.SetMemoryHwndAsProcessId(en);

        }

        public int DownloadFile(string url,string save_file,int timeout)
        {
            return _dm.DownloadFile(url,save_file,timeout);

        }

        public string FindStrFastEx(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrFastEx(x1,y1,x2,y2,str,color,sim);

        }

        public int SetEnv(int index,string name,string value)
        {
            return _dm.SetEnv(index,name,value);

        }

        public int SetClipboard(string data)
        {
            return _dm.SetClipboard(data);

        }

        public string EnumWindowByProcessId(int pid,string title,string class_name,int filter)
        {
            return _dm.EnumWindowByProcessId(pid,title,class_name,filter);

        }

        public string SortPosDistance(string all_pos,int tpe,int x,int y)
        {
            return _dm.SortPosDistance(all_pos,tpe,x,y);

        }

        public int SetDisplayInput(string mode)
        {
            return _dm.SetDisplayInput(mode);

        }

        public string FaqFetch()
        {
            return _dm.FaqFetch();

        }

        public int SetKeypadDelay(string tpe,int delay)
        {
            return _dm.SetKeypadDelay(tpe,delay);

        }

        public int FindPicMem(int x1,int y1,int x2,int y2,string pic_info,string delta_color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindPicMem(x1,y1,x2,y2,pic_info,delta_color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int GetKeyState(int vk)
        {
            return _dm.GetKeyState(vk);

        }

        public int GetDictCount(int index)
        {
            return _dm.GetDictCount(index);

        }

        public string GetNetTimeSafe()
        {
            return _dm.GetNetTimeSafe();

        }

        public int SetExactOcr(int exact_ocr)
        {
            return _dm.SetExactOcr(exact_ocr);

        }

        public int GetScreenDepth()
        {
            return _dm.GetScreenDepth();

        }

        public string EnumProcess(string name)
        {
            return _dm.EnumProcess(name);

        }

        public int WriteIntAddr(int hwnd,int addr,int tpe,int v)
        {
            return _dm.WriteIntAddr(hwnd,addr,tpe,v);

        }

        public int FoobarSetTrans(int hwnd,int trans,string color,double sim)
        {
            return _dm.FoobarSetTrans(hwnd,trans,color,sim);

        }

        public int EnableRealMouse(int en,int mousedelay,int mousestep)
        {
            return _dm.EnableRealMouse(en,mousedelay,mousestep);

        }

        public int SetWordLineHeightNoDict(int line_height)
        {
            return _dm.SetWordLineHeightNoDict(line_height);

        }

        public int SendStringIme(string str)
        {
            return _dm.SendStringIme(str);

        }

        public int FoobarDrawText(int hwnd,int x,int y,int w,int h,string text,string color,int align)
        {
            return _dm.FoobarDrawText(hwnd,x,y,w,h,text,color,align);

        }

        public string FindColorE(int x1,int y1,int x2,int y2,string color,double sim,int dir)
        {
            return _dm.FindColorE(x1,y1,x2,y2,color,sim,dir);

        }

        public int ReadIntAddr(int hwnd,int addr,int tpe)
        {
            return _dm.ReadIntAddr(hwnd,addr,tpe);

        }

        public int FindPic(int x1,int y1,int x2,int y2,string pic_name,string delta_color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindPic(x1,y1,x2,y2,pic_name,delta_color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string EnumWindowSuper(string spec1,int flag1,int type1,string spec2,int flag2,int type2,int sort)
        {
            return _dm.EnumWindowSuper(spec1,flag1,type1,spec2,flag2,type2,sort);

        }

        public int GetResultPos(string str,int index,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.GetResultPos(str,index,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public string FindNearestPos(string all_pos,int tpe,int x,int y)
        {
            return _dm.FindNearestPos(all_pos,tpe,x,y);

        }

        public int FoobarTextPrintDir(int hwnd,int dir)
        {
            return _dm.FoobarTextPrintDir(hwnd,dir);

        }

        public int Play(string file_name)
        {
            return _dm.Play(file_name);

        }

        public string SelectFile()
        {
            return _dm.SelectFile();

        }

        public int SetDisplayDelay(int t)
        {
            return _dm.SetDisplayDelay(t);

        }

        public int LeftClick()
        {
            return _dm.LeftClick();

        }

        public string FindShapeE(int x1,int y1,int x2,int y2,string offset_color,double sim,int dir)
        {
            return _dm.FindShapeE(x1,y1,x2,y2,offset_color,sim,dir);

        }

        public string Assemble(string asm_code,int base_addr,int is_upper)
        {
            return _dm.Assemble(asm_code,base_addr,is_upper);

        }

        public string FindString(int hwnd,string addr_range,string string_value,int tpe)
        {
            return _dm.FindString(hwnd,addr_range,string_value,tpe);

        }

        public int DisableFontSmooth()
        {
            return _dm.DisableFontSmooth();

        }

        public int SendCommand(string cmd)
        {
            return _dm.SendCommand(cmd);

        }

        public int FaqCaptureString(string str)
        {
            return _dm.FaqCaptureString(str);

        }

        public int DeleteIniPwd(string section,string key,string file_name,string pwd)
        {
            return _dm.DeleteIniPwd(section,key,file_name,pwd);

        }

        public int WriteDataAddr(int hwnd,int addr,string data)
        {
            return _dm.WriteDataAddr(hwnd,addr,data);

        }

        public int SetDict(int index,string dict_name)
        {
            return _dm.SetDict(index,dict_name);

        }

        public int FindWindowSuper(string spec1,int flag1,int type1,string spec2,int flag2,int type2)
        {
            return _dm.FindWindowSuper(spec1,flag1,type1,spec2,flag2,type2);

        }

        public int FoobarSetSave(int hwnd,string file_name,int en,string header)
        {
            return _dm.FoobarSetSave(hwnd,file_name,en,header);

        }

        public int DeleteFile(string file_name)
        {
            return _dm.DeleteFile(file_name);

        }

        public int Log(string info)
        {
            return _dm.Log(info);

        }

        public string FindPicS(int x1,int y1,int x2,int y2,string pic_name,string delta_color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            string result = _dm.FindPicS(x1,y1,x2,y2,pic_name,delta_color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int EnableGetColorByCapture(int en)
        {
            return _dm.EnableGetColorByCapture(en);

        }

        public int CapturePre(string file_name)
        {
            return _dm.CapturePre(file_name);

        }

        public string Ocr(int x1,int y1,int x2,int y2,string color,double sim)
        {
            return _dm.Ocr(x1,y1,x2,y2,color,sim);

        }

        public int EnableKeypadMsg(int en)
        {
            return _dm.EnableKeypadMsg(en);

        }

        public string FindPicMemEx(int x1,int y1,int x2,int y2,string pic_info,string delta_color,double sim,int dir)
        {
            return _dm.FindPicMemEx(x1,y1,x2,y2,pic_info,delta_color,sim,dir);

        }

        public int EnableBind(int en)
        {
            return _dm.EnableBind(en);

        }

        public int SetColGapNoDict(int col_gap)
        {
            return _dm.SetColGapNoDict(col_gap);

        }

        public int CreateFoobarRoundRect(int hwnd,int x,int y,int w,int h,int rw,int rh)
        {
            return _dm.CreateFoobarRoundRect(hwnd,x,y,w,h,rw,rh);

        }

        public string IntToData(int int_value,int tpe)
        {
            return _dm.IntToData(int_value,tpe);

        }

        public string MoveToEx(int x,int y,int w,int h)
        {
            return _dm.MoveToEx(x,y,w,h);

        }

        public int FaqIsPosted()
        {
            return _dm.FaqIsPosted();

        }

        public int FaqCapture(int x1,int y1,int x2,int y2,int quality,int delay,int time)
        {
            return _dm.FaqCapture(x1,y1,x2,y2,quality,delay,time);

        }

        public int FindColorBlock(int x1,int y1,int x2,int y2,string color,double sim,int count,int width,int height,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindColorBlock(x1,y1,x2,y2,color,sim,count,width,height,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int EnablePicCache(int en)
        {
            return _dm.EnablePicCache(en);

        }

        public int SendStringIme2(int hwnd,string str,int mode)
        {
            return _dm.SendStringIme2(hwnd,str,mode);

        }

        public int WriteData(int hwnd,string addr,string data)
        {
            return _dm.WriteData(hwnd,addr,data);

        }

        public int ImageToBmp(string pic_name,string bmp_name)
        {
            return _dm.ImageToBmp(pic_name,bmp_name);

        }

        public string FindColorEx(int x1,int y1,int x2,int y2,string color,double sim,int dir)
        {
            return _dm.FindColorEx(x1,y1,x2,y2,color,sim,dir);

        }

        public int FoobarUnlock(int hwnd)
        {
            return _dm.FoobarUnlock(hwnd);

        }

        public int FindColor(int x1,int y1,int x2,int y2,string color,double sim,int dir,out int x,out int y)
        {
            object ox;
            object oy;
            int result = _dm.FindColor(x1,y1,x2,y2,color,sim,dir,out ox,out oy);
            x = (int)ox;
            y = (int)oy;
            return result;

        }

        public int FindWindowByProcessId(int process_id,string class_name,string title_name)
        {
            return _dm.FindWindowByProcessId(process_id,class_name,title_name);

        }

        public int GetForegroundWindow()
        {
            return _dm.GetForegroundWindow();

        }

        public int FoobarStopGif(int hwnd,int x,int y,string pic_name)
        {
            return _dm.FoobarStopGif(hwnd,x,y,pic_name);

        }

        public int Is64Bit()
        {
            return _dm.Is64Bit();

        }

        public int SetExportDict(int index,string dict_name)
        {
            return _dm.SetExportDict(index,dict_name);

        }

        public int VirtualProtectEx(int hwnd,int addr,int size,int tpe,int old_protect)
        {
            return _dm.VirtualProtectEx(hwnd,addr,size,tpe,old_protect);

        }

        public string FaqSend(string server,int handle,int request_type,int time_out)
        {
            return _dm.FaqSend(server,handle,request_type,time_out);

        }

        public int GetFileLength(string file_name)
        {
            return _dm.GetFileLength(file_name);

        }

        public int KeyUp(int vk)
        {
            return _dm.KeyUp(vk);

        }

        public string GetAveRGB(int x1,int y1,int x2,int y2)
        {
            return _dm.GetAveRGB(x1,y1,x2,y2);

        }

        public int BindWindow(int hwnd,string display,string mouse,string keypad,int mode)
        {
            return _dm.BindWindow(hwnd,display,mouse,keypad,mode);

        }

        public int GetWindowProcessId(int hwnd)
        {
            return _dm.GetWindowProcessId(hwnd);

        }

        public string FindMultiColorEx(int x1,int y1,int x2,int y2,string first_color,string offset_color,double sim,int dir)
        {
            return _dm.FindMultiColorEx(x1,y1,x2,y2,first_color,offset_color,sim,dir);

        }

        public int delay(int mis)
        {
            return _dm.delay(mis);

        }

        public string FindStringEx(int hwnd,string addr_range,string string_value,int tpe,int steps,int multi_thread,int mode)
        {
            return _dm.FindStringEx(hwnd,addr_range,string_value,tpe,steps,multi_thread,mode);

        }

        public string FindStrExS(int x1,int y1,int x2,int y2,string str,string color,double sim)
        {
            return _dm.FindStrExS(x1,y1,x2,y2,str,color,sim);

        }

        public int FindInputMethod(string id)
        {
            return _dm.FindInputMethod(id);

        }


 #endregion 
        
        
        #region IDisposable Members

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }

                _urCom.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    #region 清风免注册调用COM类

    [ComVisible(true), ComImport(),
    Guid("00000001-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IClassFactory
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int CreateInstance(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object obj);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int LockServer(
            [MarshalAs(UnmanagedType.Bool), In] bool fLock);
    }


    internal sealed class NativeMethods
    {
        private NativeMethods() { }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern Int32 GetLastError();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }

    /// <summary>
    /// 清风免注册调用COM类
    /// </summary>
    public class URCOMLoader : IDisposable
    {

        delegate int DllGETCLASSOBJECTInvoker([MarshalAs(UnmanagedType.LPStruct)]Guid clsid, [MarshalAs(UnmanagedType.LPStruct)]Guid iid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        bool _disposed = false;
        IntPtr _lib = IntPtr.Zero;
        bool _preferURObjects = true;



        public string SearchPath
        {
            get;
            private set;
        }

        public URCOMLoader()
        {
            //this should be called on app load, but this will make sure it gets done.
            SearchPath = "";
            _preferURObjects = true;
        }

        public object CreateObjectFromPath(string dllPath, Guid clsid, bool comFallback)
        {
            return CreateObjectFromPath(dllPath, clsid, false, comFallback);
        }

        public object CreateObjectFromPath(string dllPath, Guid clsid, bool setSearchPath, bool comFallback)
        {
            object createdObject = null;
            //IntPtr lib = IntPtr.Zero;
            string fullDllPath = Path.Combine(SearchPath, dllPath);

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

                    IntPtr fnP = NativeMethods.GetProcAddress(_lib, "DllGetClassObject");
                    if (fnP != IntPtr.Zero)
                    {
                        DllGETCLASSOBJECTInvoker fn = Marshal.GetDelegateForFunctionPointer(fnP, typeof(DllGETCLASSOBJECTInvoker)) as DllGETCLASSOBJECTInvoker;

                        object pUnk = null;
                        int hr = fn(clsid, IID_IUnknown, out pUnk);
                        if (hr >= 0)
                        {
                            IClassFactory pCF = pUnk as IClassFactory;
                            if (pCF != null)
                            {
                                hr = pCF.CreateInstance(null, IID_IUnknown, out createdObject);

                                
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

            return createdObject;
        }


        #region IDisposable Members

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }

                NativeMethods.FreeLibrary(_lib);

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
    #endregion


    #region 大漠插件接口
    [Guid("F3F54BC2-D6D1-4A85-B943-16287ECEA64C"), TypeLibType(4160)]
	[ComImport]
	public interface IDmsoft
	{
		[DispId(1)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string Ver();

		[DispId(2)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetPath([MarshalAs(UnmanagedType.BStr)] [In] string path);

		[DispId(3)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string Ocr([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(4)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindStr([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(5)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetResultCount([MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(6)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetResultPos([MarshalAs(UnmanagedType.BStr)] [In] string str, [In] int index, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(7)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int StrStr([MarshalAs(UnmanagedType.BStr)] [In] string s, [MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(8)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendCommand([MarshalAs(UnmanagedType.BStr)] [In] string cmd);

		[DispId(9)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int UseDict([In] int index);

		[DispId(10)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBasePath();

		[DispId(11)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDictPwd([MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(12)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string OcrInFile([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(13)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Capture([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(14)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyPress([In] int vk);

		[DispId(15)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyDown([In] int vk);

		[DispId(16)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyUp([In] int vk);

		[DispId(17)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LeftClick();

		[DispId(18)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RightClick();

		[DispId(19)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MiddleClick();

		[DispId(20)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LeftDoubleClick();

		[DispId(21)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LeftDown();

		[DispId(22)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LeftUp();

		[DispId(23)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RightDown();

		[DispId(24)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RightUp();

		[DispId(25)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MoveTo([In] int x, [In] int y);

		[DispId(26)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MoveR([In] int rx, [In] int ry);

		[DispId(27)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetColor([In] int x, [In] int y);

		[DispId(28)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetColorBGR([In] int x, [In] int y);

		[DispId(29)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string RGB2BGR([MarshalAs(UnmanagedType.BStr)] [In] string rgb_color);

		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string BGR2RGB([MarshalAs(UnmanagedType.BStr)] [In] string bgr_color);

		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int UnBindWindow();

		[DispId(32)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CmpColor([In] int x, [In] int y, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ClientToScreen([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] [In] [Out] ref object x, [MarshalAs(UnmanagedType.Struct)] [In] [Out] ref object y);

		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ScreenToClient([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] [In] [Out] ref object x, [MarshalAs(UnmanagedType.Struct)] [In] [Out] ref object y);

		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ShowScrMsg([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string msg, [MarshalAs(UnmanagedType.BStr)] [In] string color);

		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMinRowGap([In] int row_gap);

		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMinColGap([In] int col_gap);

		[DispId(38)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(39)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindColorEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [In] int dir);

		[DispId(40)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWordLineHeight([In] int line_height);

		[DispId(41)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWordGap([In] int word_gap);

		[DispId(42)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetRowGapNoDict([In] int row_gap);

		[DispId(43)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetColGapNoDict([In] int col_gap);

		[DispId(44)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWordLineHeightNoDict([In] int line_height);

		[DispId(45)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWordGapNoDict([In] int word_gap);

		[DispId(46)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWordResultCount([MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(47)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWordResultPos([MarshalAs(UnmanagedType.BStr)] [In] string str, [In] int index, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(48)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWordResultStr([MarshalAs(UnmanagedType.BStr)] [In] string str, [In] int index);

		[DispId(49)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWords([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(50)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWordsNoDict([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color);

		[DispId(51)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetShowErrorMsg([In] int show);

		[DispId(52)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetClientSize([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object width, [MarshalAs(UnmanagedType.Struct)] out object height);

		[DispId(53)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MoveWindow([In] int hwnd, [In] int x, [In] int y);

		[DispId(54)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetColorHSV([In] int x, [In] int y);

		[DispId(55)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetAveRGB([In] int x1, [In] int y1, [In] int x2, [In] int y2);

		[DispId(56)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetAveHSV([In] int x1, [In] int y1, [In] int x2, [In] int y2);

		[DispId(57)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetForegroundWindow();

		[DispId(58)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetForegroundFocus();

		[DispId(59)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetMousePointWindow();

		[DispId(60)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetPointWindow([In] int x, [In] int y);

		[DispId(61)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumWindow([In] int parent, [MarshalAs(UnmanagedType.BStr)] [In] string title, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [In] int filter);

		[DispId(62)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWindowState([In] int hwnd, [In] int flag);

		[DispId(63)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWindow([In] int hwnd, [In] int flag);

		[DispId(64)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetSpecialWindow([In] int flag);

		[DispId(65)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWindowText([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string text);

		[DispId(66)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWindowSize([In] int hwnd, [In] int width, [In] int height);

		[DispId(67)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWindowRect([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object x1, [MarshalAs(UnmanagedType.Struct)] out object y1, [MarshalAs(UnmanagedType.Struct)] out object x2, [MarshalAs(UnmanagedType.Struct)] out object y2);

		[DispId(68)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWindowTitle([In] int hwnd);

		[DispId(69)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWindowClass([In] int hwnd);

		[DispId(70)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWindowState([In] int hwnd, [In] int flag);

		[DispId(71)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CreateFoobarRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

		[DispId(72)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CreateFoobarRoundRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h, [In] int rw, [In] int rh);

		[DispId(73)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CreateFoobarEllipse([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

		[DispId(74)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CreateFoobarCustom([In] int hwnd, [In] int x, [In] int y, [MarshalAs(UnmanagedType.BStr)] [In] string pic, [MarshalAs(UnmanagedType.BStr)] [In] string trans_color, [In] double sim);

		[DispId(75)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarFillRect([In] int hwnd, [In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color);

		[DispId(76)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarDrawText([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h, [MarshalAs(UnmanagedType.BStr)] [In] string text, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] int align);

		[DispId(77)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarDrawPic([In] int hwnd, [In] int x, [In] int y, [MarshalAs(UnmanagedType.BStr)] [In] string pic, [MarshalAs(UnmanagedType.BStr)] [In] string trans_color);

		[DispId(78)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarUpdate([In] int hwnd);

		[DispId(79)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarLock([In] int hwnd);

		[DispId(80)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarUnlock([In] int hwnd);

		[DispId(81)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarSetFont([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string font_name, [In] int size, [In] int flag);

		[DispId(82)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarTextRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

		[DispId(83)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarPrintText([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string text, [MarshalAs(UnmanagedType.BStr)] [In] string color);

		[DispId(84)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarClearText([In] int hwnd);

		[DispId(85)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarTextLineGap([In] int hwnd, [In] int gap);

		[DispId(86)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Play([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(87)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqCapture([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] int quality, [In] int delay, [In] int time);

		[DispId(88)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqRelease([In] int handle);

		[DispId(89)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FaqSend([MarshalAs(UnmanagedType.BStr)] [In] string server, [In] int handle, [In] int request_type, [In] int time_out);

		[DispId(90)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Beep([In] int fre, [In] int delay);

		[DispId(91)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarClose([In] int hwnd);

		[DispId(92)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MoveDD([In] int dx, [In] int dy);

		[DispId(93)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqGetSize([In] int handle);

		[DispId(94)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LoadPic([MarshalAs(UnmanagedType.BStr)] [In] string pic_name);

		[DispId(95)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FreePic([MarshalAs(UnmanagedType.BStr)] [In] string pic_name);

		[DispId(96)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetScreenData([In] int x1, [In] int y1, [In] int x2, [In] int y2);

		[DispId(97)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FreeScreenData([In] int handle);

		[DispId(98)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WheelUp();

		[DispId(99)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WheelDown();

		[DispId(100)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMouseDelay([MarshalAs(UnmanagedType.BStr)] [In] string type, [In] int delay);

		[DispId(101)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetKeypadDelay([MarshalAs(UnmanagedType.BStr)] [In] string type, [In] int delay);

		[DispId(102)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetEnv([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string name);

		[DispId(103)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetEnv([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string name, [MarshalAs(UnmanagedType.BStr)] [In] string value);

		[DispId(104)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendString([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(105)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DelEnv([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string name);

		[DispId(106)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPath();

		[DispId(107)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDict([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string dict_name);

		[DispId(108)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindPic([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(109)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir);

		[DispId(110)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetClientSize([In] int hwnd, [In] int width, [In] int height);

		[DispId(111)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ReadInt([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] int type);

		[DispId(112)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		float ReadFloat([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr);

		[DispId(113)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		double ReadDouble([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr);

		[DispId(114)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindInt([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] int int_value_min, [In] int int_value_max, [In] int type);

		[DispId(115)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindFloat([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] float float_value_min, [In] float float_value_max);

		[DispId(116)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindDouble([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] double double_value_min, [In] double double_value_max);

		[DispId(117)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindString([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [MarshalAs(UnmanagedType.BStr)] [In] string string_value, [In] int type);

		[DispId(118)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetModuleBaseAddr([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string module_name);

		[DispId(119)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string MoveToEx([In] int x, [In] int y, [In] int w, [In] int h);

		[DispId(120)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string MatchPicName([MarshalAs(UnmanagedType.BStr)] [In] string pic_name);

		[DispId(121)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int AddDict([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string dict_info);

		[DispId(122)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnterCri();

		[DispId(123)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LeaveCri();

		[DispId(124)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteInt([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] int type, [In] int v);

		[DispId(125)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteFloat([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] float v);

		[DispId(126)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteDouble([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] double v);

		[DispId(127)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteString([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] int type, [MarshalAs(UnmanagedType.BStr)] [In] string v);

		[DispId(128)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int AsmAdd([MarshalAs(UnmanagedType.BStr)] [In] string asm_ins);

		[DispId(129)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int AsmClear();

		[DispId(130)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int AsmCall([In] int hwnd, [In] int mode);

		[DispId(131)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindMultiColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string first_color, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(132)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindMultiColorEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string first_color, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir);

		[DispId(133)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string AsmCode([In] int base_addr);

		[DispId(134)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string Assemble([MarshalAs(UnmanagedType.BStr)] [In] string asm_code, [In] int base_addr, [In] int is_upper);

		[DispId(135)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetWindowTransparent([In] int hwnd, [In] int v);

		[DispId(136)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadData([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] int len);

		[DispId(137)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteData([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [MarshalAs(UnmanagedType.BStr)] [In] string data);

		[DispId(138)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindData([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [MarshalAs(UnmanagedType.BStr)] [In] string data);

		[DispId(139)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetPicPwd([MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(140)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Log([MarshalAs(UnmanagedType.BStr)] [In] string info);

		[DispId(141)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(142)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindColorE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [In] int dir);

		[DispId(143)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir);

		[DispId(144)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindMultiColorE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string first_color, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir);

		[DispId(145)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetExactOcr([In] int exact_ocr);

		[DispId(146)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadString([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr, [In] int type, [In] int len);

		[DispId(147)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarTextPrintDir([In] int hwnd, [In] int dir);

		[DispId(148)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string OcrEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(149)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDisplayInput([MarshalAs(UnmanagedType.BStr)] [In] string mode);

		[DispId(150)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetTime();

		[DispId(151)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetScreenWidth();

		[DispId(152)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetScreenHeight();

		[DispId(153)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int BindWindowEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string display, [MarshalAs(UnmanagedType.BStr)] [In] string mouse, [MarshalAs(UnmanagedType.BStr)] [In] string keypad, [MarshalAs(UnmanagedType.BStr)] [In] string public_desc, [In] int mode);

		[DispId(154)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDiskSerial();

		[DispId(155)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string Md5([MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(156)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMac();

		[DispId(157)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ActiveInputMethod([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string id);

		[DispId(158)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CheckInputMethod([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string id);

		[DispId(159)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindInputMethod([MarshalAs(UnmanagedType.BStr)] [In] string id);

		[DispId(160)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetCursorPos([MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(161)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int BindWindow([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string display, [MarshalAs(UnmanagedType.BStr)] [In] string mouse, [MarshalAs(UnmanagedType.BStr)] [In] string keypad, [In] int mode);

		[DispId(162)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindWindow([MarshalAs(UnmanagedType.BStr)] [In] string class_name, [MarshalAs(UnmanagedType.BStr)] [In] string title_name);

		[DispId(163)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetScreenDepth();

		[DispId(164)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetScreen([In] int width, [In] int height, [In] int depth);

		[DispId(165)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ExitOs([In] int type);

		[DispId(166)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDir([In] int type);

		[DispId(167)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetOsType();

		[DispId(168)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindWindowEx([In] int parent, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [MarshalAs(UnmanagedType.BStr)] [In] string title_name);

		[DispId(169)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetExportDict([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string dict_name);

		[DispId(170)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursorShape();

		[DispId(171)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DownCpu([In] int rate);

		[DispId(172)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursorSpot();

		[DispId(173)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendString2([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(174)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqPost([MarshalAs(UnmanagedType.BStr)] [In] string server, [In] int handle, [In] int request_type, [In] int time_out);

		[DispId(175)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FaqFetch();

		[DispId(176)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FetchWord([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [MarshalAs(UnmanagedType.BStr)] [In] string word);

		[DispId(177)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CaptureJpg([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string file, [In] int quality);

		[DispId(178)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindStrWithFont([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.BStr)] [In] string font_name, [In] int font_size, [In] int flag, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(179)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrWithFontE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.BStr)] [In] string font_name, [In] int font_size, [In] int flag);

		[DispId(180)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrWithFontEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.BStr)] [In] string font_name, [In] int font_size, [In] int flag);

		[DispId(181)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDictInfo([MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string font_name, [In] int font_size, [In] int flag);

		[DispId(182)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SaveDict([In] int index, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(183)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetWindowProcessId([In] int hwnd);

		[DispId(184)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWindowProcessPath([In] int hwnd);

		[DispId(185)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LockInput([In] int @lock);

		[DispId(186)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPicSize([MarshalAs(UnmanagedType.BStr)] [In] string pic_name);

		[DispId(187)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetID();

		[DispId(188)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CapturePng([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(189)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CaptureGif([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string file, [In] int delay, [In] int time);

		[DispId(190)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ImageToBmp([MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string bmp_name);

		[DispId(191)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindStrFast([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(192)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrFastEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(193)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrFastE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(194)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableDisplayDebug([In] int enable_debug);

		[DispId(195)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CapturePre([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(196)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RegEx([MarshalAs(UnmanagedType.BStr)] [In] string code, [MarshalAs(UnmanagedType.BStr)] [In] string Ver, [MarshalAs(UnmanagedType.BStr)] [In] string ip);

		[DispId(197)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMachineCode();

		[DispId(198)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetClipboard([MarshalAs(UnmanagedType.BStr)] [In] string data);

		[DispId(199)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClipboard();

		[DispId(200)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetNowDict();

		[DispId(201)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Is64Bit();

		[DispId(202)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetColorNum([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(203)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumWindowByProcess([MarshalAs(UnmanagedType.BStr)] [In] string process_name, [MarshalAs(UnmanagedType.BStr)] [In] string title, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [In] int filter);

		[DispId(204)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetDictCount([In] int index);

		[DispId(205)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetLastError();

		[DispId(206)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetNetTime();

		[DispId(207)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableGetColorByCapture([In] int en);

		[DispId(208)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CheckUAC();

		[DispId(209)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetUAC([In] int uac);

		[DispId(210)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DisableFontSmooth();

		[DispId(211)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CheckFontSmooth();

		[DispId(212)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDisplayAcceler([In] int level);

		[DispId(213)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindWindowByProcess([MarshalAs(UnmanagedType.BStr)] [In] string process_name, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [MarshalAs(UnmanagedType.BStr)] [In] string title_name);

		[DispId(214)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindWindowByProcessId([In] int process_id, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [MarshalAs(UnmanagedType.BStr)] [In] string title_name);

		[DispId(215)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadIni([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(216)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteIni([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string v, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(217)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RunApp([MarshalAs(UnmanagedType.BStr)] [In] string path, [In] int mode);

		[DispId(218)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int delay([In] int mis);

		[DispId(219)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindWindowSuper([MarshalAs(UnmanagedType.BStr)] [In] string spec1, [In] int flag1, [In] int type1, [MarshalAs(UnmanagedType.BStr)] [In] string spec2, [In] int flag2, [In] int type2);

		[DispId(220)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ExcludePos([MarshalAs(UnmanagedType.BStr)] [In] string all_pos, [In] int type, [In] int x1, [In] int y1, [In] int x2, [In] int y2);

		[DispId(221)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindNearestPos([MarshalAs(UnmanagedType.BStr)] [In] string all_pos, [In] int type, [In] int x, [In] int y);

		[DispId(222)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string SortPosDistance([MarshalAs(UnmanagedType.BStr)] [In] string all_pos, [In] int type, [In] int x, [In] int y);

		[DispId(223)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindPicMem([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_info, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(224)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicMemEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_info, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir);

		[DispId(225)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicMemE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_info, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir);

		[DispId(226)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string AppendPicAddr([MarshalAs(UnmanagedType.BStr)] [In] string pic_info, [In] int addr, [In] int size);

		[DispId(227)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteFile([MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string content);

		[DispId(228)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Stop([In] int id);

		[DispId(229)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDictMem([In] int index, [In] int addr, [In] int size);

		[DispId(230)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetNetTimeSafe();

		[DispId(231)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ForceUnBindWindow([In] int hwnd);

		[DispId(232)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadIniPwd([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(233)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteIniPwd([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string v, [MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(234)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DecodeFile([MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(235)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyDownChar([MarshalAs(UnmanagedType.BStr)] [In] string key_str);

		[DispId(236)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyUpChar([MarshalAs(UnmanagedType.BStr)] [In] string key_str);

		[DispId(237)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyPressChar([MarshalAs(UnmanagedType.BStr)] [In] string key_str);

		[DispId(238)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int KeyPressStr([MarshalAs(UnmanagedType.BStr)] [In] string key_str, [In] int delay);

		[DispId(239)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableKeypadPatch([In] int en);

		[DispId(240)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableKeypadSync([In] int en, [In] int time_out);

		[DispId(241)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableMouseSync([In] int en, [In] int time_out);

		[DispId(242)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DmGuard([In] int en, [MarshalAs(UnmanagedType.BStr)] [In] string type);

		[DispId(243)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqCaptureFromFile([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string file, [In] int quality);

		[DispId(244)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindIntEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] int int_value_min, [In] int int_value_max, [In] int type, [In] int step, [In] int multi_thread, [In] int mode);

		[DispId(245)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindFloatEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] float float_value_min, [In] float float_value_max, [In] int step, [In] int multi_thread, [In] int mode);

		[DispId(246)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindDoubleEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [In] double double_value_min, [In] double double_value_max, [In] int step, [In] int multi_thread, [In] int mode);

		[DispId(247)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStringEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [MarshalAs(UnmanagedType.BStr)] [In] string string_value, [In] int type, [In] int step, [In] int multi_thread, [In] int mode);

		[DispId(248)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindDataEx([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string addr_range, [MarshalAs(UnmanagedType.BStr)] [In] string data, [In] int step, [In] int multi_thread, [In] int mode);

		[DispId(249)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableRealMouse([In] int en, [In] int mousedelay, [In] int mousestep);

		[DispId(250)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableRealKeypad([In] int en);

		[DispId(251)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendStringIme([MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(252)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarDrawLine([In] int hwnd, [In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] int style, [In] int width);

		[DispId(253)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(254)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int IsBind([In] int hwnd);

		[DispId(255)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetDisplayDelay([In] int t);

		[DispId(256)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetDmCount();

		[DispId(257)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DisableScreenSave();

		[DispId(258)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DisablePowerSave();

		[DispId(259)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMemoryHwndAsProcessId([In] int en);

		[DispId(260)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindShape([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(261)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindShapeE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir);

		[DispId(262)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindShapeEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string offset_color, [In] double sim, [In] int dir);

		[DispId(263)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(264)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(265)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrFastS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(266)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindStrFastExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string str, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(267)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(268)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindPicExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [MarshalAs(UnmanagedType.BStr)] [In] string delta_color, [In] double sim, [In] int dir);

		[DispId(269)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ClearDict([In] int index);

		[DispId(270)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMachineCodeNoMac();

		[DispId(271)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetClientRect([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object x1, [MarshalAs(UnmanagedType.Struct)] out object y1, [MarshalAs(UnmanagedType.Struct)] out object x2, [MarshalAs(UnmanagedType.Struct)] out object y2);

		[DispId(272)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableFakeActive([In] int en);

		[DispId(273)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetScreenDataBmp([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.Struct)] out object data, [MarshalAs(UnmanagedType.Struct)] out object size);

		[DispId(274)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EncodeFile([MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(275)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursorShapeEx([In] int type);

		[DispId(276)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqCancel();

		[DispId(277)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string IntToData([In] int int_value, [In] int type);

		[DispId(278)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FloatToData([In] float float_value);

		[DispId(279)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string DoubleToData([In] double double_value);

		[DispId(280)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string StringToData([MarshalAs(UnmanagedType.BStr)] [In] string string_value, [In] int type);

		[DispId(281)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMemoryFindResultToFile([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(282)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableBind([In] int en);

		[DispId(283)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetSimMode([In] int mode);

		[DispId(284)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LockMouseRect([In] int x1, [In] int y1, [In] int x2, [In] int y2);

		[DispId(285)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendPaste([In] int hwnd);

		[DispId(286)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int IsDisplayDead([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] int t);

		[DispId(287)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetKeyState([In] int vk);

		[DispId(288)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CopyFile([MarshalAs(UnmanagedType.BStr)] [In] string src_file, [MarshalAs(UnmanagedType.BStr)] [In] string dst_file, [In] int over);

		[DispId(289)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int IsFileExist([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(290)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DeleteFile([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(291)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MoveFile([MarshalAs(UnmanagedType.BStr)] [In] string src_file, [MarshalAs(UnmanagedType.BStr)] [In] string dst_file);

		[DispId(292)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int CreateFolder([MarshalAs(UnmanagedType.BStr)] [In] string folder_name);

		[DispId(293)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DeleteFolder([MarshalAs(UnmanagedType.BStr)] [In] string folder_name);

		[DispId(294)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetFileLength([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(295)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadFile([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(296)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WaitKey([In] int key_code, [In] int time_out);

		[DispId(297)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DeleteIni([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(298)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DeleteIniPwd([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string key, [MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(299)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableSpeedDx([In] int en);

		[DispId(300)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableIme([In] int en);

		[DispId(301)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Reg([MarshalAs(UnmanagedType.BStr)] [In] string code, [MarshalAs(UnmanagedType.BStr)] [In] string Ver);

		[DispId(302)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string SelectFile();

		[DispId(303)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string SelectDirectory();

		[DispId(304)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LockDisplay([In] int @lock);

		[DispId(305)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarSetSave([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string file, [In] int en, [MarshalAs(UnmanagedType.BStr)] [In] string header);

		[DispId(306)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumWindowSuper([MarshalAs(UnmanagedType.BStr)] [In] string spec1, [In] int flag1, [In] int type1, [MarshalAs(UnmanagedType.BStr)] [In] string spec2, [In] int flag2, [In] int type2, [In] int sort);

		[DispId(307)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int DownloadFile([MarshalAs(UnmanagedType.BStr)] [In] string url, [MarshalAs(UnmanagedType.BStr)] [In] string save_file, [In] int timeout);

		[DispId(308)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableKeypadMsg([In] int en);

		[DispId(309)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableMouseMsg([In] int en);

		[DispId(310)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RegNoMac([MarshalAs(UnmanagedType.BStr)] [In] string code, [MarshalAs(UnmanagedType.BStr)] [In] string Ver);

		[DispId(311)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int RegExNoMac([MarshalAs(UnmanagedType.BStr)] [In] string code, [MarshalAs(UnmanagedType.BStr)] [In] string Ver, [MarshalAs(UnmanagedType.BStr)] [In] string ip);

		[DispId(312)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetEnumWindowDelay([In] int delay);

		[DispId(313)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindMulColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(314)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDict([In] int index, [In] int font_index);

		[DispId(315)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetBindWindow();

		[DispId(316)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarStartGif([In] int hwnd, [In] int x, [In] int y, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name, [In] int repeat_limit, [In] int delay);

		[DispId(317)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarStopGif([In] int hwnd, [In] int x, [In] int y, [MarshalAs(UnmanagedType.BStr)] [In] string pic_name);

		[DispId(318)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FreeProcessMemory([In] int hwnd);

		[DispId(319)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadFileData([MarshalAs(UnmanagedType.BStr)] [In] string file, [In] int start_pos, [In] int end_pos);

		[DispId(320)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int VirtualAllocEx([In] int hwnd, [In] int addr, [In] int size, [In] int type);

		[DispId(321)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int VirtualFreeEx([In] int hwnd, [In] int addr);

		[DispId(322)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCommandLine([In] int hwnd);

		[DispId(323)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int TerminateProcess([In] int pid);

		[DispId(324)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetNetTimeByIp([MarshalAs(UnmanagedType.BStr)] [In] string ip);

		[DispId(325)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumProcess([MarshalAs(UnmanagedType.BStr)] [In] string name);

		[DispId(326)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetProcessInfo([In] int pid);

		[DispId(327)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int ReadIntAddr([In] int hwnd, [In] int addr, [In] int type);

		[DispId(328)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadDataAddr([In] int hwnd, [In] int addr, [In] int len);

		[DispId(329)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		double ReadDoubleAddr([In] int hwnd, [In] int addr);

		[DispId(330)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		float ReadFloatAddr([In] int hwnd, [In] int addr);

		[DispId(331)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadStringAddr([In] int hwnd, [In] int addr, [In] int type, [In] int len);

		[DispId(332)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteDataAddr([In] int hwnd, [In] int addr, [MarshalAs(UnmanagedType.BStr)] [In] string data);

		[DispId(333)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteDoubleAddr([In] int hwnd, [In] int addr, [In] double v);

		[DispId(334)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteFloatAddr([In] int hwnd, [In] int addr, [In] float v);

		[DispId(335)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteIntAddr([In] int hwnd, [In] int addr, [In] int type, [In] int v);

		[DispId(336)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int WriteStringAddr([In] int hwnd, [In] int addr, [In] int type, [MarshalAs(UnmanagedType.BStr)] [In] string v);

		[DispId(337)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int Delays([In] int min_s, [In] int max_s);

		[DispId(338)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FindColorBlock([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [In] int count, [In] int width, [In] int height, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

		[DispId(339)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string FindColorBlockEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim, [In] int count, [In] int width, [In] int height);

		[DispId(340)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int OpenProcess([In] int pid);

		[DispId(341)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumIniSection([MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(342)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumIniSectionPwd([MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(343)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumIniKey([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string file);

		[DispId(344)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumIniKeyPwd([MarshalAs(UnmanagedType.BStr)] [In] string section, [MarshalAs(UnmanagedType.BStr)] [In] string file, [MarshalAs(UnmanagedType.BStr)] [In] string pwd);

		[DispId(345)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SwitchBindWindow([In] int hwnd);

		[DispId(346)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int InitCri();

		[DispId(347)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SendStringIme2([In] int hwnd, [MarshalAs(UnmanagedType.BStr)] [In] string str, [In] int mode);

		[DispId(348)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string EnumWindowByProcessId([In] int pid, [MarshalAs(UnmanagedType.BStr)] [In] string title, [MarshalAs(UnmanagedType.BStr)] [In] string class_name, [In] int filter);

		[DispId(349)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDisplayInfo();

		[DispId(350)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableFontSmooth();

		[DispId(351)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string OcrExOne([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(352)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetAero([In] int en);

		[DispId(353)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FoobarSetTrans([In] int hwnd, [In] int trans, [MarshalAs(UnmanagedType.BStr)] [In] string color, [In] double sim);

		[DispId(354)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnablePicCache([In] int en);

		[DispId(355)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetInfo([MarshalAs(UnmanagedType.BStr)] [In] string cmd, [MarshalAs(UnmanagedType.BStr)] [In] string param);

		[DispId(356)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqIsPosted();

		[DispId(357)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int LoadPicByte([In] int addr, [In] int size, [MarshalAs(UnmanagedType.BStr)] [In] string name);

		[DispId(358)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MiddleDown();

		[DispId(359)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int MiddleUp();

		[DispId(360)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int FaqCaptureString([MarshalAs(UnmanagedType.BStr)] [In] string str);

		[DispId(361)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int VirtualProtectEx([In] int hwnd, [In] int addr, [In] int size, [In] int type, [In] int old_protect);

		[DispId(362)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int SetMouseSpeed([In] int speed);

		[DispId(363)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int GetMouseSpeed();

		[DispId(364)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		int EnableMouseAccuracy([In] int en);
	}
    #endregion
}