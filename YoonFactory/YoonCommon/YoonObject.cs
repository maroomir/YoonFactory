namespace YoonFactory
{
    public class YoonObjectRect : IYoonObject
    {
        public int LabelNo { get; set; }
        public IYoonVector FeaturePos { get; set; }
        public IYoonRect PickArea { get; set; }
        public int PixelCount { get; set; }
        public double Score { get; set; }

        public IYoonObject Clone()
        {
            YoonObjectRect pObject = new YoonObjectRect();
            pObject.LabelNo = LabelNo;
            pObject.FeaturePos = FeaturePos.Clone();
            pObject.PickArea = PickArea.Clone();
            pObject.PixelCount = PixelCount;
            return pObject;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject == null) return;

            if(pObject is YoonObjectRect pObjectRect)
            {
                LabelNo = pObjectRect.LabelNo;
                FeaturePos = pObjectRect.FeaturePos.Clone();
                PickArea = pObjectRect.PickArea.Clone();
                PixelCount = pObjectRect.PixelCount;
            }
        }
    }

    public class YoonObjectLine : IYoonObject
    {
        public int LabelNo { get; set; }
        public IYoonVector StartPos { get; set; }
        public IYoonVector EndPos { get; set; }
        public double Score { get; set; }

        public IYoonObject Clone()
        {
            YoonObjectLine pObject = new YoonObjectLine();
            pObject.LabelNo = LabelNo;
            pObject.StartPos = StartPos.Clone();
            pObject.EndPos = EndPos.Clone();
            return pObject;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject == null) return;

            if (pObject is YoonObjectLine pObjectLine)
            {
                LabelNo = pObjectLine.LabelNo;
                StartPos = pObjectLine.StartPos.Clone();
                EndPos = pObjectLine.EndPos.Clone();
            }
        }
    }
}
