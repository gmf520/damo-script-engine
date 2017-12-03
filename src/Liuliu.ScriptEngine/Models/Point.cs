
using OSharp.Utility.Extensions;


namespace Liuliu.ScriptEngine.Models
{
    /// <summary>
    /// 游戏坐标信息
    /// </summary>
    public class Point
    {
        public Point()
        { }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(string pointStr)
        {
            string[] strs = pointStr.Split(",");
            X = strs[0].CastTo<double>();
            Y = strs[1].CastTo<double>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Map { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double RunDistance { get; set; }

        public int ClickX { get; set; }

        public int ClickY { get; set; }

        public int Order { get; set; }

        public bool IsRun { get; set; }

        public PointType PointType { get; set; }

        public string Remark { get; set; }
    }

    public enum PointType
    {
        Npc = 0,
        Monster = 1,
        Resource = 2,
        Task = 3,
        Caokuang = 4
    }
}
