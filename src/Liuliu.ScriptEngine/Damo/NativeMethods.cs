using System;
using System.Runtime.InteropServices;

namespace Liuliu.ScriptEngine.Damo
{
    internal static class NativeMethods
    {
        private const string DmcName = "dmc.dll";

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr CreateDM(string dmpath);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FreeDM();

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr Ver(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetPath(IntPtr dm, string path);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr Ocr(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindStr(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim, out object x, out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetResultCount(IntPtr dm, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetResultPos(IntPtr dm, string str, int index, out object x, out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int StrStr(IntPtr dm, string s, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SendCommand(IntPtr dm, string cmd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int UseDict(IntPtr dm, int index);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetBasePath(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDictPwd(IntPtr dm, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr OcrInFile(IntPtr dm, int x1, int y1, int x2, int y2, string picName, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Capture(IntPtr dm, int x1, int y1, int x2, int y2, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyPress(IntPtr dm, int vk);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyDown(IntPtr dm, int vk);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyUp(IntPtr dm, int vk);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LeftClick(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RightClick(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MiddleClick(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LeftDoubleClick(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LeftDown(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LeftUp(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RightDown(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RightUp(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveTo(IntPtr dm, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveR(IntPtr dm, int rx, int ry);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetColor(IntPtr dm, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetColorBGR(IntPtr dm, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr RGB2BGR(IntPtr dm, string rgbColor);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr BGR2RGB(IntPtr dm, string bgrColor);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int UnBindWindow(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CmpColor(IntPtr dm, int x, int y, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ClientToScreen(IntPtr dm, int hwnd, ref object x, ref object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ScreenToClient(IntPtr dm, int hwnd, ref object x, ref object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ShowScrMsg(IntPtr dm, int x1, int y1, int x2, int y2, string msg, string color);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetMinRowGap(IntPtr dm, int rowGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetMinColGap(IntPtr dm, int colGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindColor(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim, int dir, out object x, out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindColorEx(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWordLineHeight(IntPtr dm, int lineHeight);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWordGap(IntPtr dm, int wordGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetRowGapNoDict(IntPtr dm, int rowGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetColGapNoDict(IntPtr dm, int colGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWordLineHeightNoDict(IntPtr dm, int lineHeight);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWordGapNoDict(IntPtr dm, int wordGap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWordResultCount(IntPtr dm, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWordResultPos(IntPtr dm, string str, int index, out object x, out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWordResultStr(IntPtr dm, string str, int index);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWords(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWordsNoDict(IntPtr dm, int x1, int y1, int x2, int y2, string color);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetShowErrorMsg(IntPtr dm, int show);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetClientSize(IntPtr dm, int hwnd, out object width, out object height);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveWindow(IntPtr dm, int hwnd, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetColorHSV(IntPtr dm, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetAveRGB(IntPtr dm, int x1, int y1, int x2, int y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetAveHSV(IntPtr dm, int x1, int y1, int x2, int y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetForegroundWindow(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetForegroundFocus(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetMousePointWindow(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetPointWindow(IntPtr dm, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr EnumWindow(IntPtr dm, int parent, string title, string className, int filter);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWindowState(IntPtr dm, int hwnd, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWindow(IntPtr dm, int hwnd, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetSpecialWindow(IntPtr dm, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWindowText(IntPtr dm, int hwnd, string text);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWindowSize(IntPtr dm, int hwnd, int width, int height);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWindowRect(IntPtr dm, int hwnd, out object x1, out object y1, out object x2, out object y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWindowTitle(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWindowClass(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWindowState(IntPtr dm, int hwnd, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CreateFoobarRect(IntPtr dm, int hwnd, int x, int y, int w, int h);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CreateFoobarRoundRect(IntPtr dm, int hwnd, int x, int y, int w, int h, int rw, int rh);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CreateFoobarEllipse(IntPtr dm, int hwnd, int x, int y, int w, int h);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CreateFoobarCustom(IntPtr dm, int hwnd, int x, int y, string pic, string transColor, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarFillRect(IntPtr dm, int hwnd, int x1, int y1, int x2, int y2, string color);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarDrawText(IntPtr dm, int hwnd, int x, int y, int w, int h, string text, string color, int align);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarDrawPic(IntPtr dm, int hwnd, int x, int y, string pic, string transColor);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarUpdate(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarLock(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarUnlock(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarSetFont(IntPtr dm, int hwnd, string fontName, int size, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarTextRect(IntPtr dm, int hwnd, int x, int y, int w, int h);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarPrintText(IntPtr dm, int hwnd, string text, string color);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarClearText(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarTextLineGap(IntPtr dm, int hwnd, int gap);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Play(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqCapture(IntPtr dm, int x1, int y1, int x2, int y2, int quality, int delay, int time);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqRelease(IntPtr dm, int handle);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FaqSend(IntPtr dm, string server, int handle, int requestType, int timeOut);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Beep(IntPtr dm, int fre, int delay);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarClose(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveDD(IntPtr dm, int dx, int dy);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqGetSize(IntPtr dm, int handle);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LoadPic(IntPtr dm, string picName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FreePic(IntPtr dm, string picName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetScreenData(IntPtr dm, int x1, int y1, int x2, int y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FreeScreenData(IntPtr dm, int handle);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WheelUp(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WheelDown(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetMouseDelay(IntPtr dm, string type, int delay);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetKeypadDelay(IntPtr dm, string type, int delay);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetEnv(IntPtr dm, int index, string name);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetEnv(IntPtr dm, int index, string name, string value);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SendString(IntPtr dm, int hwnd, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DelEnv(IntPtr dm, int index, string name);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetPath(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDict(IntPtr dm, int index, string dictName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindPic(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string picName,
            string deltaColor,
            double sim,
            int dir,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicEx(IntPtr dm, int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetClientSize(IntPtr dm, int hwnd, int width, int height);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ReadInt(IntPtr dm, int hwnd, string addr, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ReadFloat(IntPtr dm, int hwnd, string addr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ReadDouble(IntPtr dm, int hwnd, string addr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindInt(IntPtr dm, int hwnd, string addrRange, int intValueMin, int intValueMax, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindFloat(IntPtr dm, int hwnd, string addrRange, Single floatValueMin, Single floatValueMax);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindDouble(IntPtr dm, int hwnd, string addrRange, double doubleValueMin, double doubleValueMax);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindString(IntPtr dm, int hwnd, string addrRange, string stringValue, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetModuleBaseAddr(IntPtr dm, int hwnd, string moduleName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr MoveToEx(IntPtr dm, int x, int y, int w, int h);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr MatchPicName(IntPtr dm, string picName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int AddDict(IntPtr dm, int index, string dictInfo);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnterCri(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LeaveCri(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteInt(IntPtr dm, int hwnd, string addr, int type, int v);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteFloat(IntPtr dm, int hwnd, string addr, Single v);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteDouble(IntPtr dm, int hwnd, string addr, double v);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteString(IntPtr dm, int hwnd, string addr, int type, string v);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int AsmAdd(IntPtr dm, string asmIns);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int AsmClear(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int AsmCall(IntPtr dm, int hwnd, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindMultiColor(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string firstColor,
            string offsetColor,
            double sim,
            int dir,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindMultiColorEx(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string firstColor,
            string offsetColor,
            double sim,
            int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr AsmCode(IntPtr dm, int baseAddr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr Assemble(IntPtr dm, string asmCode, int baseAddr, int isUpper);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetWindowTransparent(IntPtr dm, int hwnd, int v);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ReadData(IntPtr dm, int hwnd, string addr, int len);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteData(IntPtr dm, int hwnd, string addr, string data);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindData(IntPtr dm, int hwnd, string addrRange, string data);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetPicPwd(IntPtr dm, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Log(IntPtr dm, string info);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrE(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindColorE(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicE(IntPtr dm, int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindMultiColorE(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string firstColor,
            string offsetColor,
            double sim,
            int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetExactOcr(IntPtr dm, int exactOcr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ReadString(IntPtr dm, int hwnd, string addr, int type, int len);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarTextPrintDir(IntPtr dm, int hwnd, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr OcrEx(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDisplayInput(IntPtr dm, string mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern long GetTime(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetScreenWidth(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetScreenHeight(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int BindWindowEx(IntPtr dm, int hwnd, string display, string mouse, string keypad, string publicDesc, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetDiskSerial(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr Md5(IntPtr dm, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetMac(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ActiveInputMethod(IntPtr dm, int hwnd, string id);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CheckInputMethod(IntPtr dm, int hwnd, string id);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindInputMethod(IntPtr dm, string id);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetCursorPos(IntPtr dm, out object x, out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int BindWindow(IntPtr dm, int hwnd, string display, string mouse, string keypad, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindWindow(IntPtr dm, string className, string titleName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetScreenDepth(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetScreen(IntPtr dm, int width, int height, int depth);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ExitOs(IntPtr dm, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetDir(IntPtr dm, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetOsType(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindWindowEx(IntPtr dm, int parent, string className, string titleName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetExportDict(IntPtr dm, int index, string dictName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCursorShape(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DownCpu(IntPtr dm, int rate);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCursorSpot(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SendString2(IntPtr dm, int hwnd, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqPost(IntPtr dm, string server, int handle, int requestType, int timeOut);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FaqFetch(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FetchWord(IntPtr dm, int x1, int y1, int x2, int y2, string color, string word);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CaptureJpg(IntPtr dm, int x1, int y1, int x2, int y2, string file, int quality);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindStrWithFont(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            string fontName,
            int fontSize,
            int flag,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrWithFontE(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            string fontName,
            int fontSize,
            int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrWithFontEx(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            string fontName,
            int fontSize,
            int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetDictInfo(IntPtr dm, string str, string fontName, int fontSize, int flag);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SaveDict(IntPtr dm, int index, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWindowProcessId(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetWindowProcessPath(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LockInput(IntPtr dm, int lock1);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetPicSize(IntPtr dm, string picName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetID(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CapturePng(IntPtr dm, int x1, int y1, int x2, int y2, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CaptureGif(IntPtr dm, int x1, int y1, int x2, int y2, string file, int delay, int time);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ImageToBmp(IntPtr dm, string picName, string bmpName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindStrFast(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrFastEx(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrFastE(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableDisplayDebug(IntPtr dm, int enableDebug);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CapturePre(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RegEx(IntPtr dm, string code, string ver, string ip);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetMachineCode(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetClipboard(IntPtr dm, string data);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetClipboard(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetNowDict(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Is64Bit(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetColorNum(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr EnumWindowByProcess(IntPtr dm, string processName, string title, string className, int filter);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetDictCount(IntPtr dm, int index);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetLastError(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetNetTime(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableGetColorByCapture(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CheckUAC(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetUAC(IntPtr dm, int uac);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DisableFontSmooth(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CheckFontSmooth(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDisplayAcceler(IntPtr dm, int level);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindWindowByProcess(IntPtr dm, string processName, string className, string titleName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindWindowByProcessId(IntPtr dm, int processId, string className, string titleName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ReadIni(IntPtr dm, string section, string key, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteIni(IntPtr dm, string section, string key, string v, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RunApp(IntPtr dm, string path, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int delay(IntPtr dm, int mis);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindWindowSuper(IntPtr dm, string spec1, int flag1, int type1, string spec2, int flag2, int type2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ExcludePos(IntPtr dm, string allPos, int type, int x1, int y1, int x2, int y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindNearestPos(IntPtr dm, string allPos, int type, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr SortPosDistance(IntPtr dm, string allPos, int type, int x, int y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindPicMem(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string picInfo,
            string deltaColor,
            double sim,
            int dir,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicMemEx(IntPtr dm, int x1, int y1, int x2, int y2, string picInfo, string deltaColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicMemE(IntPtr dm, int x1, int y1, int x2, int y2, string picInfo, string deltaColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr AppendPicAddr(IntPtr dm, string picInfo, int addr, int size);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteFile(IntPtr dm, string file, string content);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Stop(IntPtr dm, int id);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDictMem(IntPtr dm, int index, int addr, int size);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetNetTimeSafe(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ForceUnBindWindow(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ReadIniPwd(IntPtr dm, string section, string key, string file, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WriteIniPwd(IntPtr dm, string section, string key, string v, string file, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DecodeFile(IntPtr dm, string file, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyDownChar(IntPtr dm, string keyStr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyUpChar(IntPtr dm, string keyStr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyPressChar(IntPtr dm, string keyStr);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int KeyPressStr(IntPtr dm, string keyStr, int delay);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableKeypadPatch(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableKeypadSync(IntPtr dm, int en, int timeOut);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableMouseSync(IntPtr dm, int en, int timeOut);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DmGuard(IntPtr dm, int en, string type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqCaptureFromFile(IntPtr dm, int x1, int y1, int x2, int y2, string file, int quality);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindIntEx(IntPtr dm,
            int hwnd,
            string addrRange,
            int intValueMin,
            int intValueMax,
            int type,
            int step,
            int multiThread,
            int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindFloatEx(IntPtr dm,
            int hwnd,
            string addrRange,
            Single floatValueMin,
            Single floatValueMax,
            int step,
            int multiThread,
            int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindDoubleEx(IntPtr dm,
            int hwnd,
            string addrRange,
            double doubleValueMin,
            double doubleValueMax,
            int step,
            int multiThread,
            int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStringEx(IntPtr dm,
            int hwnd,
            string addrRange,
            string stringValue,
            int type,
            int step,
            int multiThread,
            int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindDataEx(IntPtr dm, int hwnd, string addrRange, string data, int step, int multiThread, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableRealMouse(IntPtr dm, int en, int mousedelay, int mousestep);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableRealKeypad(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SendStringIme(IntPtr dm, string str);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarDrawLine(IntPtr dm, int hwnd, int x1, int y1, int x2, int y2, string color, int style, int width);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrEx(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int IsBind(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDisplayDelay(IntPtr dm, int t);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetDmCount(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DisableScreenSave(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DisablePowerSave(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetMemoryHwndAsProcessId(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindShape(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string offsetColor,
            double sim,
            int dir,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindShapeE(IntPtr dm, int x1, int y1, int x2, int y2, string offsetColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindShapeEx(IntPtr dm, int x1, int y1, int x2, int y2, string offsetColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrS(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrExS(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrFastS(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string str,
            string color,
            double sim,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindStrFastExS(IntPtr dm, int x1, int y1, int x2, int y2, string str, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicS(IntPtr dm,
            int x1,
            int y1,
            int x2,
            int y2,
            string picName,
            string deltaColor,
            double sim,
            int dir,
            out object x,
            out object y);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FindPicExS(IntPtr dm, int x1, int y1, int x2, int y2, string picName, string deltaColor, double sim, int dir);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int ClearDict(IntPtr dm, int index);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetMachineCodeNoMac(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetClientRect(IntPtr dm, int hwnd, out object x1, out object y1, out object x2, out object y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableFakeActive(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetScreenDataBmp(IntPtr dm, int x1, int y1, int x2, int y2, out object data, out object size);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EncodeFile(IntPtr dm, string file, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCursorShapeEx(IntPtr dm, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FaqCancel(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr IntToData(IntPtr dm, int intValue, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr FloatToData(IntPtr dm, Single floatValue);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr DoubleToData(IntPtr dm, double doubleValue);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr StringToData(IntPtr dm, string stringValue, int type);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetMemoryFindResultToFile(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableBind(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetSimMode(IntPtr dm, int mode);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LockMouseRect(IntPtr dm, int x1, int y1, int x2, int y2);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SendPaste(IntPtr dm, int hwnd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int IsDisplayDead(IntPtr dm, int x1, int y1, int x2, int y2, int t);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetKeyState(IntPtr dm, int vk);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CopyFile(IntPtr dm, string srcFile, string dstFile, int over);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int IsFileExist(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DeleteFile(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveFile(IntPtr dm, string srcFile, string dstFile);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int CreateFolder(IntPtr dm, string folderName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DeleteFolder(IntPtr dm, string folderName);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetFileLength(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr ReadFile(IntPtr dm, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int WaitKey(IntPtr dm, int keyCode, int timeOut);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DeleteIni(IntPtr dm, string section, string key, string file);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DeleteIniPwd(IntPtr dm, string section, string key, string file, string pwd);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableSpeedDx(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableIme(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int Reg(IntPtr dm, string code, string ver);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr SelectFile(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr SelectDirectory(IntPtr dm);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int LockDisplay(IntPtr dm, int lock1);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FoobarSetSave(IntPtr dm, int hwnd, string file, int en, string header);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr EnumWindowSuper(IntPtr dm, string spec1, int flag1, int type1, string spec2, int flag2, int type2, int sort);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DownloadFile(IntPtr dm, string url, string saveFile, int timeout);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableKeypadMsg(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int EnableMouseMsg(IntPtr dm, int en);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RegNoMac(IntPtr dm, string code, string ver);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int RegExNoMac(IntPtr dm, string code, string ver, string ip);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetEnumWindowDelay(IntPtr dm, int delay);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern int FindMulColor(IntPtr dm, int x1, int y1, int x2, int y2, string color, double sim);

        [DllImport(DmcName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetDict(IntPtr dm, int index, int fontIndex);
    }

}
