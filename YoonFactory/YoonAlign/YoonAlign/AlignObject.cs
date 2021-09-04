﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using YoonFactory.Image;

namespace YoonFactory
{
    public class AlignObject : YoonObject, IEquatable<AlignObject>
    {
        public eYoonDir2D Direction { get; private set; }
        public IYoonFigure OriginFeature { get; private set; }
        public IYoonVector OriginPosition { get; private set; }

        public AlignObject(YoonObject pOriginObject)
        {
            Direction = eYoonDir2D.None;
            Label = pOriginObject.Label;
            Score = pOriginObject.Score;
            PixelCount = pOriginObject.PixelCount;
            ObjectImage = pOriginObject.ObjectImage.Clone() as YoonImage;
            Position = pOriginObject.Position.Clone();
            OriginPosition = pOriginObject.Position.Clone();
            Feature = pOriginObject.Feature.Clone();
            OriginFeature = pOriginObject.Feature.Clone();
        }

        public AlignObject(YoonObject pObject, AlignObject pOrigin)
        {
            Label = pObject.Label;
            Score = pObject.Score;
            PixelCount = pObject.PixelCount;
            ObjectImage = pObject.ObjectImage.Clone() as YoonImage;
            Position = pObject.Position.Clone();
            Feature = pObject.Feature.Clone();
            // Bring the origin object
            Direction = pOrigin.Direction;
            OriginPosition = pOrigin.OriginPosition.Clone();
            OriginFeature = pOrigin.OriginFeature.Clone();
        }

        public AlignObject(YoonObject pOriginObject, YoonImage pImage, IYoonVector pVector) : this(pOriginObject)
        {
            ObjectImage = pImage.Clone() as YoonImage;
            Position = pVector.Clone();
        }

        public AlignObject(YoonObject pOriginObject, YoonImage pImage, IYoonFigure pFeature, IYoonVector pVector) :
            this(pOriginObject)
        {
            ObjectImage = pImage.Clone() as YoonImage;
            Position = pVector.Clone();
            Feature = pFeature.Clone();
        }

        public void SetOrigin(eYoonDir2D nDir = eYoonDir2D.None)
        {
            Direction = nDir;
            OriginPosition = Position.Clone();
            OriginFeature = Feature.Clone();
        }

        public YoonVector2D GetOriginVector2D()
        {
            if (OriginPosition is YoonVector2D pVector2D || OriginPosition is YoonVector2N pVector2N)
                return (OriginPosition is YoonVector2N pOriginVector2N)
                    ? pOriginVector2N.ToVector2D()
                    : OriginPosition as YoonVector2D;
            else
                throw new TypeAccessException("[YOONALIGN] Position Type is abnormal");
        }

        public YoonVector2N GetOriginVector2N()
        {
            if (OriginPosition is YoonVector2D pVector2D || OriginPosition is YoonVector2N pVector2N)
                return (OriginPosition is YoonVector2D pOriginVector2D)
                    ? pOriginVector2D.ToVector2N()
                    : OriginPosition as YoonVector2N;
            else
                throw new TypeAccessException("[YOONALIGN] Position Type is abnormal");
        }

        public YoonVector2D GetCurrentVector2D()
        {
            if (Position is YoonVector2D pVector2D || Position is YoonVector2N pVector2N)
                return (Position is YoonVector2N pCurrentVector2N)
                    ? pCurrentVector2N.ToVector2D()
                    : Position as YoonVector2D;
            else
                throw new TypeAccessException("[YOONALIGN] Position Type is abnormal");
        }

        public YoonVector2N GetCurrentVector2N()
        {
            if (Position is YoonVector2D pVector2D || Position is YoonVector2N pVector2N)
                return (Position is YoonVector2D pCurrentVector2D)
                    ? pCurrentVector2D.ToVector2N()
                    : Position as YoonVector2N;
            else
                throw new TypeAccessException("[YOONALIGN] Position Type is abnormal");
        }

        public double GetOriginTheta()
        {
            return (Feature is YoonRectAffine2D pRectOrigin) ? pRectOrigin.Rotation : 0.0;
        }

        public double GetCurrentTheta()
        {
            return (Feature is YoonRectAffine2D pRectObject) ? pRectObject.Rotation : 0.0;
        }

        public new void CopyFrom(IYoonParameter pObject)
        {
            if (pObject is AlignObject pAlignObject)
            {
                Direction = pAlignObject.Direction;
                Label = pAlignObject.Label;
                Score = pAlignObject.Score;
                PixelCount = pAlignObject.PixelCount;
                ObjectImage = pAlignObject.ObjectImage.Clone() as YoonImage;
                Position = pAlignObject.Position.Clone();
                OriginPosition = pAlignObject.OriginPosition.Clone();
                Feature = pAlignObject.Feature.Clone();
                OriginFeature = pAlignObject.OriginFeature.Clone();
            }
        }

        public new IYoonParameter Clone()
        {
            return new AlignObject(this);
        }

        bool IEquatable<AlignObject>.Equals(AlignObject other)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            return Label == other.Label &&
                   Score == other.Score &&
                   EqualityComparer<IYoonFigure>.Default.Equals(Feature, other.Feature) &&
                   EqualityComparer<IYoonFigure>.Default.Equals(OriginFeature, other.OriginFeature) &&
                   EqualityComparer<IYoonVector>.Default.Equals(Position, other.Position) &&
                   EqualityComparer<IYoonVector>.Default.Equals(OriginPosition, other.OriginPosition) &&
                   EqualityComparer<YoonImage>.Default.Equals(ObjectImage, other.ObjectImage) &&
                   PixelCount == other.PixelCount;
        }
    }
}
