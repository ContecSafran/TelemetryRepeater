using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{
    public class Draw
    {
        public static System.Drawing.Point ToCenter(System.Drawing.Rectangle rc)
        {
            return new System.Drawing.Point(rc.X + (rc.Width / 2), rc.Y + (rc.Height / 2));
        }

        public static System.Drawing.Size ToCenterSize(System.Drawing.Rectangle rc)
        {
            return new System.Drawing.Size(rc.X + (rc.Width / 2), rc.Y + (rc.Height / 2));
        }
        public static System.Drawing.Rectangle ToRectangle(List<System.Drawing.Point> points)
        {
            int minX = points.Min(p => p.X);
            int minY = points.Min(p => p.Y);
            int maxX = points.Max(p => p.X);
            int maxY = points.Max(p => p.Y);
            return new System.Drawing.Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
