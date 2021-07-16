using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Viewer
{
    public delegate void UpdatePointCallback(object sender, PointArgs e);
    public delegate void UpdateMeasureCallback(object sender, MeasureArgs e);

    public class PointArgs : EventArgs
    {
        public int Index { get; set; }
        public YoonVector2N Position { get; set; }
        public byte Pixel { get; set; }

        public PointArgs(YoonVector2N pPos)
        {
            Index = 0;
            Position = pPos.Clone() as YoonVector2N;
            Pixel = 0;
        }

        public PointArgs(int nIndex, YoonVector2N pPos)
        {
            Index = nIndex;
            Position = pPos.Clone() as YoonVector2N;
            Pixel = 0;
        }

        public PointArgs(YoonVector2N pPos, byte nValue)
        {
            Index = 0;
            Position = pPos.Clone() as YoonVector2N;
            Pixel = nValue;
        }
    }

    public class MeasureArgs : EventArgs
    {
        public double Length { get; set; }
        public YoonVector2N Positioin { get; set; }

        public MeasureArgs(double dLength, int nX, int nY)
        {
            Length = dLength;
            Positioin = new YoonVector2N();
            Positioin.X = nX;
            Positioin.Y = nY;
        }
    }
}
