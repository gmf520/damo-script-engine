using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSharp.Utility.Secutiry;

using Qf;


namespace Liuliu.ScriptEngine.Consoles
{
    class Program
    {
        private static DmPlugin _dm;

        static void Main(string[] args)
        {
            _dm = new DmPlugin();
            while (true)
            {
                Console.WriteLine($"大漠插件版本号：{_dm.Ver()}，请输入注册码：");
                string code = Console.ReadLine();
                int ret = _dm.Reg(code);
                if (ret == 1)
                {
                    Console.WriteLine("大漠插件注册成功");
                    break;
                }
                Console.WriteLine($"大漠插件注册失败：{ret}");
            }

            bool exit = false;
            while (true)
            {
                try
                {
                    Console.WriteLine(@"请输入命令：0; 退出程序，功能命令：1 - n");
                    string input = Console.ReadLine();
                    if (input == null)
                    {
                        continue;
                    }
                    switch (input.ToLower())
                    {
                        case "0":
                            exit = true;
                            break;
                        case "1":
                            Method01();
                            break;
                        case "2":
                            Method02();
                            break;
                        case "3":
                            Method03();
                            break;
                        case "4":
                            Method04();
                            break;
                        case "5":
                            Method05();
                            break;
                        case "6":
                            Method06();
                            break;
                        case "7":
                            Method07();
                            break;
                        case "8":
                            Method08();
                            break;
                        case "9":
                            Method09();
                            break;
                        case "10":
                            Method10();
                            break;
                        case "11":
                            Method11();
                            break;
                    }
                    if (exit)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void Method01()
        {
            Console.WriteLine(_dm.Beep(1000, 1000));
        }

        private static void Method02()
        {
            QfDmSoft dm = new QfDmSoft();
            Console.WriteLine(dm.Ver());
        }

        private static void Method03()
        {
            throw new NotImplementedException();
        }

        private static void Method04()
        {
            throw new NotImplementedException();
        }

        private static void Method05()
        {
            throw new NotImplementedException();
        }

        private static void Method06()
        {
            throw new NotImplementedException();
        }

        private static void Method07()
        {
            throw new NotImplementedException();
        }

        private static void Method08()
        {
            throw new NotImplementedException();
        }

        private static void Method09()
        {
            throw new NotImplementedException();
        }

        private static void Method10()
        {
            throw new NotImplementedException();
        }

        private static void Method11()
        {
            throw new NotImplementedException();
        }
    }
}
