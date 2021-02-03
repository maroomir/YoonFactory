using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory
{
    // 실좌표계 기준 Top-Right부터 1사분면임.
    // 따라서 모든 초기 위치를 Top / Right로 가져감.
    public enum eYoonDirX : int
    {
        Right = 0,
        Left,
        MaxDir,
    }
    public enum eYoonDirY : int
    {
        Top = 0,
        Bottom,
        MaxDir,
    }
    public enum eYoonDirRect : int
    {
        TopRight = 0,
        TopLeft,
        BottomLeft,
        BottomRight,
        MaxDir,
    }
    public enum eYoonDirCompass : int
    {
        Center=0,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        MaxDir,
    }
    public enum eYoonDirClock : int
    {
        None = -3,
        Clock = -2,
        AntiClock = -1,
        H12 = 0,
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,
        H7,
        H8,
        H9,
        H10,
        H11,
        MaxDir,
    }
    public enum eYoonDirNature : int
    {
        Motionless,
        Ascend,
        Descend,
        Horizon,
        Vertical,
        MaxDir,
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
}
