using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Image
{
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

    public enum eYoonRGBMode : int
    {
        None,
        Parallel,
        Mixed,
    }
}
