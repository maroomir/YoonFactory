using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory
{
    public enum eYoonDir2DMode : int
    {
        Fixed = 0,
        Clock,
        AntiClock,
        Increase,
        Decrease,
        Whirlpool,
        AntiWhirlpool,
        AxisX,
        AxisY,
    }

    public enum eYoonMouseMode : int
    {
        GetNone,
        GetPosition,
        GetRectangle,
        GetWidth,
        GetCenter,
        FixedCenter,
        GetCircle,
        GetDistance,
        GetXPos,
        GetYPos,
        GetXSize,
    }
    public enum eYoonStatus : int
    {
        Normal = 0,
        Conform,
        Send,
        Receive,
        User,
        Inspect,
        Info,
        OK,
        NG,
        Error,
    }
    // 실좌표계 기준 Top-Right부터 1사분면임.
    // 따라서 모든 초기 위치를 Top / Right로 가져감.
    public enum eYoonDir2D : int
    {
        None = -1,
        Center = 0,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
    }

    public static class YoonDirFactory
    {
        public static eYoonDir2D Go(this eYoonDir2D e, eYoonDir2DMode nMode)
        {
            switch (nMode)
            {
                case eYoonDir2DMode.Fixed:
                    return e;
                case eYoonDir2DMode.Clock:
                    return e.PreviousOctant();
                case eYoonDir2DMode.AntiClock:
                    return e.NextOctant();
                case eYoonDir2DMode.Increase:
                    return e.NextOrder();
                case eYoonDir2DMode.Decrease:
                    return e.PreviousOrder();
                case eYoonDir2DMode.Whirlpool:
                    return e.NextWhirlpool();
                case eYoonDir2DMode.AntiWhirlpool:
                    return e.PreviousWhirlpool();
                case eYoonDir2DMode.AxisX:
                    return e.ReverseY();
                case eYoonDir2DMode.AxisY:
                    return e.ReverseX();
                default:
                    return eYoonDir2D.None;
            }
        }

        public static eYoonDir2D NextQuadrant(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Top:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:
                    return eYoonDir2D.Top;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.TopRight;
                default:
                    return e;
            }
        }

        public static eYoonDir2D PreviousQuadrant(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Top:
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.Top;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.TopRight;
                default:
                    return e;
            }
        }

        public static eYoonDir2D NextOctant(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Right:
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.Right;
                default:
                    return e;
            }
        }

        public static eYoonDir2D PreviousOctant(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Right:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top:
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.Right;
                default:
                    return e;
            }
        }

        public static eYoonDir2D ReverseX(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.Top:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.Top;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.TopRight;
                default:
                    return e;
            }
        }

        public static eYoonDir2D ReverseY(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.Left:
                    return eYoonDir2D.Right;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.Right:
                    return eYoonDir2D.Left;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.BottomLeft;
                default:
                    return e;
            }
        }

        public static eYoonDir2D NextOrder(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.TopLeft: // 1
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top:    // 2
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight:   // 3
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:   // 4
                    return eYoonDir2D.Center;
                case eYoonDir2D.Center: // 5
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:  // 6
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft: // 7
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom: // 8
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:    // 9
                    return eYoonDir2D.TopLeft;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static eYoonDir2D PreviousOrder(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.BottomRight: // 9
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:    // 8
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:   // 7
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:   // 6
                    return eYoonDir2D.Center;
                case eYoonDir2D.Center: // 5
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:  // 4
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight: // 3
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top: // 2
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:    // 1
                    return eYoonDir2D.BottomRight;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static eYoonDir2D NextWhirlpool(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top:
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.Center;
                case eYoonDir2D.Center:
                    return eYoonDir2D.TopLeft;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static eYoonDir2D PreviousWhirlpool(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Center:
                    return eYoonDir2D.Left;
                case eYoonDir2D.Left:
                    return eYoonDir2D.BottomLeft;
                case eYoonDir2D.BottomLeft:
                    return eYoonDir2D.Bottom;
                case eYoonDir2D.Bottom:
                    return eYoonDir2D.BottomRight;
                case eYoonDir2D.BottomRight:
                    return eYoonDir2D.Right;
                case eYoonDir2D.Right:
                    return eYoonDir2D.TopRight;
                case eYoonDir2D.TopRight:
                    return eYoonDir2D.Top;
                case eYoonDir2D.Top:
                    return eYoonDir2D.TopLeft;
                case eYoonDir2D.TopLeft:
                    return eYoonDir2D.Center;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static int ToOrder(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.TopLeft:
                    return 0;
                case eYoonDir2D.Top:
                    return 1;
                case eYoonDir2D.TopRight:
                    return 2;
                case eYoonDir2D.Left:
                    return 3;
                case eYoonDir2D.Center:
                    return 4;
                case eYoonDir2D.Right:
                    return 5;
                case eYoonDir2D.BottomLeft:
                    return 6;
                case eYoonDir2D.Bottom:
                    return 7;
                case eYoonDir2D.BottomRight:
                    return 8;
                default:
                    return -1;
            }
        }

        public static eYoonDir2D FromOrder(int nNum)
        {
            switch (nNum)
            {
                case 0:
                    return eYoonDir2D.TopLeft;
                case 1:
                    return eYoonDir2D.Top;
                case 2:
                    return eYoonDir2D.TopRight;
                case 3:
                    return eYoonDir2D.Left;
                case 4:
                    return eYoonDir2D.Center;
                case 5:
                    return eYoonDir2D.Right;
                case 6:
                    return eYoonDir2D.BottomLeft;
                case 7:
                    return eYoonDir2D.Bottom;
                case 8:
                    return eYoonDir2D.BottomRight;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static int ToClock(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Top:
                    return 0;
                case eYoonDir2D.TopRight:
                    return 1;
                case eYoonDir2D.Right:
                    return 3;
                case eYoonDir2D.BottomRight:
                    return 5;
                case eYoonDir2D.Bottom:
                    return 6;
                case eYoonDir2D.BottomLeft:
                    return 7;
                case eYoonDir2D.Left:
                    return 9;
                case eYoonDir2D.TopLeft:
                    return 11;
                default:
                    return -1;
            }
        }

        public static eYoonDir2D FromClock(int nClock)
        {
            switch (nClock)
            {
                case 0:
                case 12:
                    return eYoonDir2D.Top;
                case 1:
                case 2:
                    return eYoonDir2D.TopRight;
                case 3:
                    return eYoonDir2D.Right;
                case 4:
                case 5:
                    return eYoonDir2D.BottomRight;
                case 6:
                    return eYoonDir2D.Bottom;
                case 7:
                case 8:
                    return eYoonDir2D.BottomLeft;
                case 9:
                    return eYoonDir2D.Left;
                case 10:
                case 11:
                    return eYoonDir2D.TopLeft;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static int ToQuadant(this eYoonDir2D e)
        {
            switch (e)
            {
                case eYoonDir2D.Center:
                    return 0;
                case eYoonDir2D.Right:
                case eYoonDir2D.TopRight:
                    return 1;
                case eYoonDir2D.Top:
                case eYoonDir2D.TopLeft:
                    return 2;
                case eYoonDir2D.Left:
                case eYoonDir2D.BottomLeft:
                    return 3;
                case eYoonDir2D.Bottom:
                case eYoonDir2D.BottomRight:
                    return 4;
                default:
                    return -1;
            }
        }

        public static eYoonDir2D FromQuadant(int nNum)
        {
            switch (nNum)
            {
                case 0:
                    return eYoonDir2D.Center;
                case 1:
                    return eYoonDir2D.TopRight;
                case 2:
                    return eYoonDir2D.TopLeft;
                case 3:
                    return eYoonDir2D.BottomLeft;
                case 4:
                    return eYoonDir2D.BottomRight;
                default:
                    return eYoonDir2D.None;
            }
        }

        public static eYoonDir2D[] GetSquareDirections()
        {
            return new eYoonDir2D[4] { eYoonDir2D.TopRight, eYoonDir2D.TopLeft, eYoonDir2D.BottomLeft, eYoonDir2D.BottomRight };
        }

        public static eYoonDir2D[] GetRhombusDirections()
        {
            return new eYoonDir2D[4] { eYoonDir2D.Top, eYoonDir2D.Left, eYoonDir2D.Bottom, eYoonDir2D.Right };
        }

        public static eYoonDir2D[] GetHorizonDirections()
        {
            return new eYoonDir2D[2] { eYoonDir2D.Left, eYoonDir2D.Right };
        }

        public static eYoonDir2D[] GetVerticalDirections()
        {
            return new eYoonDir2D[2] { eYoonDir2D.Top, eYoonDir2D.Bottom };
        }
    }
}