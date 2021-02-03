using System;

namespace YoonFactory.Calibration
{
    public delegate void PoseSendCallback(object sender, CalibPoseArgs e);
    public class CalibPoseArgs : EventArgs
    {
        public eYoonDirCompass GrapDirection;
        public eYoonDirClock RotationDirection;
        public YoonCartD Pose;
        public CalibPoseArgs(eYoonDirCompass nDir, YoonCartD pPos)
        {
            GrapDirection = nDir;
            RotationDirection = eYoonDirClock.None;
            Pose = pPos.Clone() as YoonCartD;
        }
        public CalibPoseArgs(eYoonDirCompass nDir, eYoonDirClock nRot, YoonCartD pPos)
        {
            GrapDirection = nDir;
            RotationDirection = nRot;
            Pose = pPos.Clone() as YoonCartD;
        }
    }

    public delegate void PointSendCallback(object sender, CalibPointArgs e);
    public class CalibPointArgs : EventArgs
    {
        public eYoonDirRect DeviceDirection;
        public eYoonDirCompass GrapDirection;
        public YoonVector2D Point;

        public CalibPointArgs(eYoonDirCompass nDir, YoonVector2D pPos)
        {
            DeviceDirection = eYoonDirRect.MaxDir;
            GrapDirection = nDir;
            Point = pPos.Clone() as YoonVector2D;
        }

        public CalibPointArgs(eYoonDirRect nDirDevice, eYoonDirCompass nDirObject, YoonVector2D pPos)
        {
            DeviceDirection = nDirDevice;
            GrapDirection = nDirObject;
            Point = pPos.Clone() as YoonVector2D;
        }
    }

    public delegate void RectSendCallback(object sender, CalibRectArgs e);
    public class CalibRectArgs : EventArgs
    {
        public eYoonDirRect DeviceDirection;
        public eYoonDirCompass GrapDirection;
        public YoonRectAffine2D Rect;

        public CalibRectArgs(eYoonDirRect nDirDevice, eYoonDirCompass nDirObject, YoonRectAffine2D pRect)
        {
            DeviceDirection = nDirDevice;
            GrapDirection = nDirObject;
            Rect = pRect.Clone() as YoonRectAffine2D;
        }

        public CalibRectArgs(eYoonDirCompass nDir, YoonRectAffine2D pRect)
        {
            DeviceDirection = eYoonDirRect.MaxDir;
            GrapDirection = nDir;
            Rect = pRect.Clone() as YoonRectAffine2D;
        }
    }

    public delegate void GrapRequestCallback(object sender, CalibGrapArgs e);
    public class CalibGrapArgs : EventArgs
    {
        public eYoonDirCompass GrapDirection;

        public CalibGrapArgs(eYoonDirCompass nDir)
        {
            GrapDirection = nDir;
        }
    }

}
