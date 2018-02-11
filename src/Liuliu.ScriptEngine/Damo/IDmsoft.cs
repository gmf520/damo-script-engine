// -----------------------------------------------------------------------
//  <copyright file="IDmsoft.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-03 22:33</last-date>
// -----------------------------------------------------------------------


#pragma warning disable 1591
namespace Liuliu.ScriptEngine.Damo
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 大漠插件接口（版本7.1807）
    /// 代码生成方式：>tlbimp D:\dm.dll /out:D:\dmnet.dll，再反编译dmnet.dll获得代码
    /// </summary>
    [ComImport]
    [Guid("F3F54BC2-D6D1-4A85-B943-16287ECEA64C")]
    [TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
    public interface IDmsoft
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(1)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string Ver();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(2)]
        int SetPath([In] [MarshalAs(UnmanagedType.BStr)] string path);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(3)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string Ocr([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(4)]
        int FindStr([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(5)]
        int GetResultCount([In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(6)]
        int GetResultPos([In] [MarshalAs(UnmanagedType.BStr)] string str, [In] int index, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(7)]
        int StrStr([In] [MarshalAs(UnmanagedType.BStr)] string s, [In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(8)]
        int SendCommand([In] [MarshalAs(UnmanagedType.BStr)] string cmd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(9)]
        int UseDict([In] int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(10)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetBasePath();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(11)]
        int SetDictPwd([In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(12)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string OcrInFile([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(13)]
        int Capture([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(14)]
        int KeyPress([In] int vk);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(15)]
        int KeyDown([In] int vk);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(16)]
        int KeyUp([In] int vk);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(17)]
        int LeftClick();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(18)]
        int RightClick();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(19)]
        int MiddleClick();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(20)]
        int LeftDoubleClick();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(21)]
        int LeftDown();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(22)]
        int LeftUp();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(23)]
        int RightDown();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(24)]
        int RightUp();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(25)]
        int MoveTo([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(26)]
        int MoveR([In] int rx, [In] int ry);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(27)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetColor([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(28)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetColorBGR([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(29)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string RGB2BGR([In] [MarshalAs(UnmanagedType.BStr)] string rgb_color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(30)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string BGR2RGB([In] [MarshalAs(UnmanagedType.BStr)] string bgr_color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(31)]
        int UnBindWindow();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(32)]
        int CmpColor([In] int x, [In] int y, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(33)]
        int ClientToScreen([In] int hwnd, [In] [Out] [MarshalAs(UnmanagedType.Struct)] ref object x, [In] [Out] [MarshalAs(UnmanagedType.Struct)] ref object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(34)]
        int ScreenToClient([In] int hwnd, [In] [Out] [MarshalAs(UnmanagedType.Struct)] ref object x, [In] [Out] [MarshalAs(UnmanagedType.Struct)] ref object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(35)]
        int ShowScrMsg([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string msg, [In] [MarshalAs(UnmanagedType.BStr)] string color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(36)]
        int SetMinRowGap([In] int row_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(37)]
        int SetMinColGap([In] int col_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(38)]
        int FindColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(39)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindColorEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(40)]
        int SetWordLineHeight([In] int line_height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(41)]
        int SetWordGap([In] int word_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(42)]
        int SetRowGapNoDict([In] int row_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(43)]
        int SetColGapNoDict([In] int col_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(44)]
        int SetWordLineHeightNoDict([In] int line_height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(45)]
        int SetWordGapNoDict([In] int word_gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(46)]
        int GetWordResultCount([In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(47)]
        int GetWordResultPos([In] [MarshalAs(UnmanagedType.BStr)] string str, [In] int index, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(48)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWordResultStr([In] [MarshalAs(UnmanagedType.BStr)] string str, [In] int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(49)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWords([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(50)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWordsNoDict([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(51)]
        int SetShowErrorMsg([In] int show);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(52)]
        int GetClientSize([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object width, [MarshalAs(UnmanagedType.Struct)] out object height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(53)]
        int MoveWindow([In] int hwnd, [In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(54)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetColorHSV([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(55)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetAveRGB([In] int x1, [In] int y1, [In] int x2, [In] int y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(56)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetAveHSV([In] int x1, [In] int y1, [In] int x2, [In] int y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(57)]
        int GetForegroundWindow();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(58)]
        int GetForegroundFocus();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(59)]
        int GetMousePointWindow();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(60)]
        int GetPointWindow([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(61)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumWindow([In] int parent, [In] [MarshalAs(UnmanagedType.BStr)] string title, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] int filter);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(62)]
        int GetWindowState([In] int hwnd, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(63)]
        int GetWindow([In] int hwnd, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(64)]
        int GetSpecialWindow([In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(65)]
        int SetWindowText([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(66)]
        int SetWindowSize([In] int hwnd, [In] int width, [In] int height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(67)]
        int GetWindowRect([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object x1, [MarshalAs(UnmanagedType.Struct)] out object y1, [MarshalAs(UnmanagedType.Struct)] out object x2, [MarshalAs(UnmanagedType.Struct)] out object y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(68)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWindowTitle([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(69)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWindowClass([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(70)]
        int SetWindowState([In] int hwnd, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(71)]
        int CreateFoobarRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(72)]
        int CreateFoobarRoundRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h, [In] int rw, [In] int rh);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(73)]
        int CreateFoobarEllipse([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(74)]
        int CreateFoobarCustom([In] int hwnd, [In] int x, [In] int y, [In] [MarshalAs(UnmanagedType.BStr)] string pic, [In] [MarshalAs(UnmanagedType.BStr)] string trans_color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(75)]
        int FoobarFillRect([In] int hwnd, [In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(76)]
        int FoobarDrawText([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h, [In] [MarshalAs(UnmanagedType.BStr)] string text, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] int align);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(77)]
        int FoobarDrawPic([In] int hwnd, [In] int x, [In] int y, [In] [MarshalAs(UnmanagedType.BStr)] string pic, [In] [MarshalAs(UnmanagedType.BStr)] string trans_color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(78)]
        int FoobarUpdate([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(79)]
        int FoobarLock([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(80)]
        int FoobarUnlock([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(81)]
        int FoobarSetFont([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string font_name, [In] int size, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(82)]
        int FoobarTextRect([In] int hwnd, [In] int x, [In] int y, [In] int w, [In] int h);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(83)]
        int FoobarPrintText([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string text, [In] [MarshalAs(UnmanagedType.BStr)] string color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(84)]
        int FoobarClearText([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(85)]
        int FoobarTextLineGap([In] int hwnd, [In] int gap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(86)]
        int Play([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(87)]
        int FaqCapture([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] int quality, [In] int delay, [In] int time);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(88)]
        int FaqRelease([In] int handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(89)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FaqSend([In] [MarshalAs(UnmanagedType.BStr)] string server, [In] int handle, [In] int request_type, [In] int time_out);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(90)]
        int Beep([In] int fre, [In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(91)]
        int FoobarClose([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(92)]
        int MoveDD([In] int dx, [In] int dy);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(93)]
        int FaqGetSize([In] int handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(94)]
        int LoadPic([In] [MarshalAs(UnmanagedType.BStr)] string pic_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(95)]
        int FreePic([In] [MarshalAs(UnmanagedType.BStr)] string pic_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(96)]
        int GetScreenData([In] int x1, [In] int y1, [In] int x2, [In] int y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(97)]
        int FreeScreenData([In] int handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(98)]
        int WheelUp();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(99)]
        int WheelDown();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(100)]
        int SetMouseDelay([In] [MarshalAs(UnmanagedType.BStr)] string type, [In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(101)]
        int SetKeypadDelay([In] [MarshalAs(UnmanagedType.BStr)] string type, [In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(102)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetEnv([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(103)]
        int SetEnv([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string name, [In] [MarshalAs(UnmanagedType.BStr)] string value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(104)]
        int SendString([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(105)]
        int DelEnv([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(106)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetPath();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(107)]
        int SetDict([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string dict_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(108)]
        int FindPic([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(109)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(110)]
        int SetClientSize([In] int hwnd, [In] int width, [In] int height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(111)]
        long ReadInt([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(112)]
        float ReadFloat([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(113)]
        double ReadDouble([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(114)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindInt([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] long int_value_min, [In] long int_value_max, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(115)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindFloat([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] float float_value_min, [In] float float_value_max);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(116)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindDouble([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] double double_value_min, [In] double double_value_max);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(117)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindString([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] [MarshalAs(UnmanagedType.BStr)] string string_value, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(118)]
        long GetModuleBaseAddr([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string module_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(119)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string MoveToEx([In] int x, [In] int y, [In] int w, [In] int h);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(120)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string MatchPicName([In] [MarshalAs(UnmanagedType.BStr)] string pic_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(121)]
        int AddDict([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string dict_info);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(122)]
        int EnterCri();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(123)]
        int LeaveCri();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(124)]
        int WriteInt([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int type, [In] long v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(125)]
        int WriteFloat([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] float v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(126)]
        int WriteDouble([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] double v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(127)]
        int WriteString([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int type, [In] [MarshalAs(UnmanagedType.BStr)] string v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(128)]
        int AsmAdd([In] [MarshalAs(UnmanagedType.BStr)] string asm_ins);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(129)]
        int AsmClear();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(130)]
        int AsmCall([In] int hwnd, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(131)]
        int FindMultiColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string first_color, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(132)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindMultiColorEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string first_color, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(133)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string AsmCode([In] int base_addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(134)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string Assemble([In] [MarshalAs(UnmanagedType.BStr)] string asm_code, [In] int base_addr, [In] int is_upper);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(135)]
        int SetWindowTransparent([In] int hwnd, [In] int v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(136)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadData([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(137)]
        int WriteData([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] [MarshalAs(UnmanagedType.BStr)] string data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(138)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindData([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] [MarshalAs(UnmanagedType.BStr)] string data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(139)]
        int SetPicPwd([In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(140)]
        int Log([In] [MarshalAs(UnmanagedType.BStr)] string info);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(141)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(142)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindColorE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(143)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(144)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindMultiColorE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string first_color, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(145)]
        int SetExactOcr([In] int exact_ocr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(146)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadString([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int type, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(147)]
        int FoobarTextPrintDir([In] int hwnd, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(148)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string OcrEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(149)]
        int SetDisplayInput([In] [MarshalAs(UnmanagedType.BStr)] string mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(150)]
        int GetTime();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(151)]
        int GetScreenWidth();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(152)]
        int GetScreenHeight();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(153)]
        int BindWindowEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string display, [In] [MarshalAs(UnmanagedType.BStr)] string mouse, [In] [MarshalAs(UnmanagedType.BStr)] string keypad, [In] [MarshalAs(UnmanagedType.BStr)] string public_desc, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(154)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDiskSerial();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(155)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string Md5([In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(156)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetMac();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(157)]
        int ActiveInputMethod([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(158)]
        int CheckInputMethod([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(159)]
        int FindInputMethod([In] [MarshalAs(UnmanagedType.BStr)] string id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(160)]
        int GetCursorPos([MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(161)]
        int BindWindow([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string display, [In] [MarshalAs(UnmanagedType.BStr)] string mouse, [In] [MarshalAs(UnmanagedType.BStr)] string keypad, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(162)]
        int FindWindow([In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] [MarshalAs(UnmanagedType.BStr)] string title_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(163)]
        int GetScreenDepth();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(164)]
        int SetScreen([In] int width, [In] int height, [In] int depth);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(165)]
        int ExitOs([In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(166)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDir([In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(167)]
        int GetOsType();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(168)]
        int FindWindowEx([In] int parent, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] [MarshalAs(UnmanagedType.BStr)] string title_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(169)]
        int SetExportDict([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string dict_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(170)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetCursorShape();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(171)]
        int DownCpu([In] int rate);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(172)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetCursorSpot();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(173)]
        int SendString2([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(174)]
        int FaqPost([In] [MarshalAs(UnmanagedType.BStr)] string server, [In] int handle, [In] int request_type, [In] int time_out);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(175)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FaqFetch();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(176)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FetchWord([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] [MarshalAs(UnmanagedType.BStr)] string word);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(177)]
        int CaptureJpg([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(178)]
        int FindStrWithFont([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] [MarshalAs(UnmanagedType.BStr)] string font_name, [In] int font_size, [In] int flag, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(179)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrWithFontE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] [MarshalAs(UnmanagedType.BStr)] string font_name, [In] int font_size, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(180)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrWithFontEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] [MarshalAs(UnmanagedType.BStr)] string font_name, [In] int font_size, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(181)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDictInfo([In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string font_name, [In] int font_size, [In] int flag);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(182)]
        int SaveDict([In] int index, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(183)]
        int GetWindowProcessId([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(184)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetWindowProcessPath([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(185)]
        int LockInput([In] int @lock);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(186)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetPicSize([In] [MarshalAs(UnmanagedType.BStr)] string pic_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(187)]
        int GetID();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(188)]
        int CapturePng([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(189)]
        int CaptureGif([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] int delay, [In] int time);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(190)]
        int ImageToBmp([In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string bmp_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(191)]
        int FindStrFast([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(192)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrFastEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(193)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrFastE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(194)]
        int EnableDisplayDebug([In] int enable_debug);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(195)]
        int CapturePre([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(196)]
        int RegEx([In] [MarshalAs(UnmanagedType.BStr)] string code, [In] [MarshalAs(UnmanagedType.BStr)] string Ver, [In] [MarshalAs(UnmanagedType.BStr)] string ip);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(197)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetMachineCode();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(198)]
        int SetClipboard([In] [MarshalAs(UnmanagedType.BStr)] string data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(199)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetClipboard();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(200)]
        int GetNowDict();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(201)]
        int Is64Bit();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(202)]
        int GetColorNum([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(203)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumWindowByProcess([In] [MarshalAs(UnmanagedType.BStr)] string process_name, [In] [MarshalAs(UnmanagedType.BStr)] string title, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] int filter);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(204)]
        int GetDictCount([In] int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(205)]
        int GetLastError();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(206)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetNetTime();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(207)]
        int EnableGetColorByCapture([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(208)]
        int CheckUAC();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(209)]
        int SetUAC([In] int uac);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(210)]
        int DisableFontSmooth();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(211)]
        int CheckFontSmooth();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(212)]
        int SetDisplayAcceler([In] int level);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(213)]
        int FindWindowByProcess([In] [MarshalAs(UnmanagedType.BStr)] string process_name, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] [MarshalAs(UnmanagedType.BStr)] string title_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(214)]
        int FindWindowByProcessId([In] int process_id, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] [MarshalAs(UnmanagedType.BStr)] string title_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(215)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadIni([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(216)]
        int WriteIni([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string v, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(217)]
        int RunApp([In] [MarshalAs(UnmanagedType.BStr)] string path, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(218)]
        int delay([In] int mis);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(219)]
        int FindWindowSuper([In] [MarshalAs(UnmanagedType.BStr)] string spec1, [In] int flag1, [In] int type1, [In] [MarshalAs(UnmanagedType.BStr)] string spec2, [In] int flag2, [In] int type2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(220)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ExcludePos([In] [MarshalAs(UnmanagedType.BStr)] string all_pos, [In] int type, [In] int x1, [In] int y1, [In] int x2, [In] int y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(221)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindNearestPos([In] [MarshalAs(UnmanagedType.BStr)] string all_pos, [In] int type, [In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(222)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string SortPosDistance([In] [MarshalAs(UnmanagedType.BStr)] string all_pos, [In] int type, [In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(223)]
        int FindPicMem([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_info, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(224)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicMemEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_info, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(225)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicMemE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_info, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(226)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string AppendPicAddr([In] [MarshalAs(UnmanagedType.BStr)] string pic_info, [In] int addr, [In] int size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(227)]
        int WriteFile([In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string content);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(228)]
        int Stop([In] int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(229)]
        int SetDictMem([In] int index, [In] int addr, [In] int size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(230)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetNetTimeSafe();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(231)]
        int ForceUnBindWindow([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(232)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadIniPwd([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(233)]
        int WriteIniPwd([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string v, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(234)]
        int DecodeFile([In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(235)]
        int KeyDownChar([In] [MarshalAs(UnmanagedType.BStr)] string key_str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(236)]
        int KeyUpChar([In] [MarshalAs(UnmanagedType.BStr)] string key_str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(237)]
        int KeyPressChar([In] [MarshalAs(UnmanagedType.BStr)] string key_str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(238)]
        int KeyPressStr([In] [MarshalAs(UnmanagedType.BStr)] string key_str, [In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(239)]
        int EnableKeypadPatch([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(240)]
        int EnableKeypadSync([In] int en, [In] int time_out);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(241)]
        int EnableMouseSync([In] int en, [In] int time_out);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(242)]
        int DmGuard([In] int en, [In] [MarshalAs(UnmanagedType.BStr)] string type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(243)]
        int FaqCaptureFromFile([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] int quality);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(244)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindIntEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] long int_value_min, [In] long int_value_max, [In] int type, [In] int step, [In] int multi_thread, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(245)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindFloatEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] float float_value_min, [In] float float_value_max, [In] int step, [In] int multi_thread, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(246)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindDoubleEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] double double_value_min, [In] double double_value_max, [In] int step, [In] int multi_thread, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(247)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStringEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] [MarshalAs(UnmanagedType.BStr)] string string_value, [In] int type, [In] int step, [In] int multi_thread, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(248)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindDataEx([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr_range, [In] [MarshalAs(UnmanagedType.BStr)] string data, [In] int step, [In] int multi_thread, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(249)]
        int EnableRealMouse([In] int en, [In] int mousedelay, [In] int mousestep);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(250)]
        int EnableRealKeypad([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(251)]
        int SendStringIme([In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(252)]
        int FoobarDrawLine([In] int hwnd, [In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] int style, [In] int width);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(253)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(254)]
        int IsBind([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(255)]
        int SetDisplayDelay([In] int t);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(256)]
        int GetDmCount();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(257)]
        int DisableScreenSave();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(258)]
        int DisablePowerSave();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(259)]
        int SetMemoryHwndAsProcessId([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(260)]
        int FindShape([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(261)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindShapeE([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(262)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindShapeEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string offset_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(263)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(264)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(265)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrFastS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(266)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindStrFastExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(267)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(268)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindPicExS([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] [MarshalAs(UnmanagedType.BStr)] string delta_color, [In] double sim, [In] int dir);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(269)]
        int ClearDict([In] int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(270)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetMachineCodeNoMac();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(271)]
        int GetClientRect([In] int hwnd, [MarshalAs(UnmanagedType.Struct)] out object x1, [MarshalAs(UnmanagedType.Struct)] out object y1, [MarshalAs(UnmanagedType.Struct)] out object x2, [MarshalAs(UnmanagedType.Struct)] out object y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(272)]
        int EnableFakeActive([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(273)]
        int GetScreenDataBmp([In] int x1, [In] int y1, [In] int x2, [In] int y2, [MarshalAs(UnmanagedType.Struct)] out object data, [MarshalAs(UnmanagedType.Struct)] out object size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(274)]
        int EncodeFile([In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(275)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetCursorShapeEx([In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(276)]
        int FaqCancel();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(277)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string IntToData([In] long int_value, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(278)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FloatToData([In] float float_value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(279)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string DoubleToData([In] double double_value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(280)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string StringToData([In] [MarshalAs(UnmanagedType.BStr)] string string_value, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(281)]
        int SetMemoryFindResultToFile([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(282)]
        int EnableBind([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(283)]
        int SetSimMode([In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(284)]
        int LockMouseRect([In] int x1, [In] int y1, [In] int x2, [In] int y2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(285)]
        int SendPaste([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(286)]
        int IsDisplayDead([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] int t);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(287)]
        int GetKeyState([In] int vk);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(288)]
        int CopyFile([In] [MarshalAs(UnmanagedType.BStr)] string src_file, [In] [MarshalAs(UnmanagedType.BStr)] string dst_file, [In] int over);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(289)]
        int IsFileExist([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(290)]
        int DeleteFile([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(291)]
        int MoveFile([In] [MarshalAs(UnmanagedType.BStr)] string src_file, [In] [MarshalAs(UnmanagedType.BStr)] string dst_file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(292)]
        int CreateFolder([In] [MarshalAs(UnmanagedType.BStr)] string folder_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(293)]
        int DeleteFolder([In] [MarshalAs(UnmanagedType.BStr)] string folder_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(294)]
        int GetFileLength([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(295)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadFile([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(296)]
        int WaitKey([In] int key_code, [In] int time_out);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(297)]
        int DeleteIni([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(298)]
        int DeleteIniPwd([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string key, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(299)]
        int EnableSpeedDx([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(300)]
        int EnableIme([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(301)]
        int Reg([In] [MarshalAs(UnmanagedType.BStr)] string code, [In] [MarshalAs(UnmanagedType.BStr)] string Ver);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(302)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string SelectFile();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(303)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string SelectDirectory();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(304)]
        int LockDisplay([In] int @lock);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(305)]
        int FoobarSetSave([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] int en, [In] [MarshalAs(UnmanagedType.BStr)] string header);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(306)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumWindowSuper([In] [MarshalAs(UnmanagedType.BStr)] string spec1, [In] int flag1, [In] int type1, [In] [MarshalAs(UnmanagedType.BStr)] string spec2, [In] int flag2, [In] int type2, [In] int sort);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(307)]
        int DownloadFile([In] [MarshalAs(UnmanagedType.BStr)] string url, [In] [MarshalAs(UnmanagedType.BStr)] string save_file, [In] int timeout);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(308)]
        int EnableKeypadMsg([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(309)]
        int EnableMouseMsg([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(310)]
        int RegNoMac([In] [MarshalAs(UnmanagedType.BStr)] string code, [In] [MarshalAs(UnmanagedType.BStr)] string Ver);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(311)]
        int RegExNoMac([In] [MarshalAs(UnmanagedType.BStr)] string code, [In] [MarshalAs(UnmanagedType.BStr)] string Ver, [In] [MarshalAs(UnmanagedType.BStr)] string ip);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(312)]
        int SetEnumWindowDelay([In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(313)]
        int FindMulColor([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(314)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDict([In] int index, [In] int font_index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(315)]
        int GetBindWindow();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(316)]
        int FoobarStartGif([In] int hwnd, [In] int x, [In] int y, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name, [In] int repeat_limit, [In] int delay);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(317)]
        int FoobarStopGif([In] int hwnd, [In] int x, [In] int y, [In] [MarshalAs(UnmanagedType.BStr)] string pic_name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(318)]
        int FreeProcessMemory([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(319)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadFileData([In] [MarshalAs(UnmanagedType.BStr)] string file, [In] int start_pos, [In] int end_pos);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(320)]
        long VirtualAllocEx([In] int hwnd, [In] long addr, [In] int size, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(321)]
        int VirtualFreeEx([In] int hwnd, [In] long addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(322)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetCommandLine([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(323)]
        int TerminateProcess([In] int pid);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(324)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetNetTimeByIp([In] [MarshalAs(UnmanagedType.BStr)] string ip);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(325)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumProcess([In] [MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(326)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetProcessInfo([In] int pid);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(327)]
        long ReadIntAddr([In] int hwnd, [In] long addr, [In] int type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(328)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadDataAddr([In] int hwnd, [In] long addr, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(329)]
        double ReadDoubleAddr([In] int hwnd, [In] long addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(330)]
        float ReadFloatAddr([In] int hwnd, [In] long addr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(331)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string ReadStringAddr([In] int hwnd, [In] long addr, [In] int type, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(332)]
        int WriteDataAddr([In] int hwnd, [In] long addr, [In] [MarshalAs(UnmanagedType.BStr)] string data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(333)]
        int WriteDoubleAddr([In] int hwnd, [In] long addr, [In] double v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(334)]
        int WriteFloatAddr([In] int hwnd, [In] long addr, [In] float v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(335)]
        int WriteIntAddr([In] int hwnd, [In] long addr, [In] int type, [In] long v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(336)]
        int WriteStringAddr([In] int hwnd, [In] long addr, [In] int type, [In] [MarshalAs(UnmanagedType.BStr)] string v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(337)]
        int Delays([In] int min_s, [In] int max_s);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(338)]
        int FindColorBlock([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] int count, [In] int width, [In] int height, [MarshalAs(UnmanagedType.Struct)] out object x, [MarshalAs(UnmanagedType.Struct)] out object y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(339)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string FindColorBlockEx([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim, [In] int count, [In] int width, [In] int height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(340)]
        int OpenProcess([In] int pid);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(341)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumIniSection([In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(342)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumIniSectionPwd([In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(343)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumIniKey([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string file);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(344)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumIniKeyPwd([In] [MarshalAs(UnmanagedType.BStr)] string section, [In] [MarshalAs(UnmanagedType.BStr)] string file, [In] [MarshalAs(UnmanagedType.BStr)] string pwd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(345)]
        int SwitchBindWindow([In] int hwnd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(346)]
        int InitCri();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(347)]
        int SendStringIme2([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string str, [In] int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(348)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string EnumWindowByProcessId([In] int pid, [In] [MarshalAs(UnmanagedType.BStr)] string title, [In] [MarshalAs(UnmanagedType.BStr)] string class_name, [In] int filter);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(349)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDisplayInfo();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(350)]
        int EnableFontSmooth();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(351)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string OcrExOne([In] int x1, [In] int y1, [In] int x2, [In] int y2, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(352)]
        int SetAero([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(353)]
        int FoobarSetTrans([In] int hwnd, [In] int trans, [In] [MarshalAs(UnmanagedType.BStr)] string color, [In] double sim);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(354)]
        int EnablePicCache([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(355)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetInfo([In] [MarshalAs(UnmanagedType.BStr)] string cmd, [In] [MarshalAs(UnmanagedType.BStr)] string param);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(356)]
        int FaqIsPosted();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(357)]
        int LoadPicByte([In] int addr, [In] int size, [In] [MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(358)]
        int MiddleDown();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(359)]
        int MiddleUp();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(360)]
        int FaqCaptureString([In] [MarshalAs(UnmanagedType.BStr)] string str);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(361)]
        int VirtualProtectEx([In] int hwnd, [In] long addr, [In] int size, [In] int type, [In] int old_protect);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(362)]
        int SetMouseSpeed([In] int speed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(363)]
        int GetMouseSpeed();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(364)]
        int EnableMouseAccuracy([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(365)]
        int SetExcludeRegion([In] int type, [In] [MarshalAs(UnmanagedType.BStr)] string info);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(366)]
        int EnableShareDict([In] int en);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(367)]
        int DisableCloseDisplayAndSleep();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(368)]
        int Int64ToInt32([In] long v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(369)]
        int GetLocale();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(370)]
        int SetLocale();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(371)]
        int ReadDataToBin([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(372)]
        int WriteDataFromBin([In] int hwnd, [In] [MarshalAs(UnmanagedType.BStr)] string addr, [In] int data, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(373)]
        int ReadDataAddrToBin([In] int hwnd, [In] long addr, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(374)]
        int WriteDataAddrFromBin([In] int hwnd, [In] long addr, [In] int data, [In] int len);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(375)]
        int SetParam64ToPointer();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(376)]
        int GetDPI();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(377)]
        int SetDisplayRefreshDelay([In] int t);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(378)]
        int IsFolderExist([In] [MarshalAs(UnmanagedType.BStr)] string folder);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [DispId(379)]
        int GetCpuType();
    }
}