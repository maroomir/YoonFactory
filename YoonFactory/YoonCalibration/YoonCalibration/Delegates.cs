using System;

namespace YoonFactory.Calibration
{
    public delegate void PoseSendCallback(object sender, CalibPoseArgs e);
    public class CalibPoseArgs : EventArgs
    {
        public eYoonDir2D GrapDirection;
        public eYoonDir2DMode DirectionMode;
        public YoonCartD Pose;
        public CalibPoseArgs(eYoonDir2D nDir, YoonCartD pPos)
        {
            GrapDirection = nDir;
            DirectionMode = eYoonDir2DMode.Fixed;
            Pose = pPos.Clone() as YoonCartD;
        }
        public CalibPoseArgs(eYoonDir2D nDir, eYoonDir2DMode nRot, YoonCartD pPos)
        {
            GrapDirection = nDir;
            DirectionMode = nRot;
            Pose = pPos.Clone() as YoonCartD;
        }
    }

    public delegate void PointSendCallback(object sender, CalibPointArgs e);
    public class CalibPointArgs : EventArgs
    {
        public eYoonDir2D DeviceDirection;
        public eYoonDir2D GrapDirection;
        public YoonVector2D Point;

        public CalibPointArgs(eYoonDir2D nDir, YoonVector2D pPos)
        {
            DeviceDirection = eYoonDir2D.None;
            GrapDirection = nDir;
            Point = pPos.Clone() as YoonVector2D;
        }

        public CalibPointArgs(eYoonDir2D nDirDevice, eYoonDir2D nDirObject, YoonVector2D pPos)
        {
            DeviceDirection = nDirDevice;
            GrapDirection = nDirObject;
            Point = pPos.Clone() as YoonVector2D;
        }
    }

    public delegate void RectSendCallback(object sender, CalibRectArgs e);
    public class CalibRectArgs : EventArgs
    {
        public eYoonDir2D DeviceDirection;
        public eYoonDir2D GrapDirection;
        public YoonRectAffine2D Rect;

        public CalibRectArgs(eYoonDir2D nDirDevice, eYoonDir2D nDirObject, YoonRectAffine2D pRect)
        {
            DeviceDirection = nDirDevice;
            GrapDirection = nDirObject;
            Rect = pRect.Clone() as YoonRectAffine2D;
        }

        public CalibRectArgs(eYoonDir2D nDir, YoonRectAffine2D pRect)
        {
            DeviceDirection = eYoonDir2D.None;
            GrapDirection = nDir;
            Rect = pRect.Clone() as YoonRectAffine2D;
        }
    }

    public delegate void GrapRequestCallback(object sender, CalibGrapArgs e);
    public class CalibGrapArgs : EventArgs
    {
        public eYoonDir2D GrapDirection;

        public CalibGrapArgs(eYoonDir2D nDir)
        {
            GrapDirection = nDir;
        }
    }

}
