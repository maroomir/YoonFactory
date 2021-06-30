using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Image.Result
{
    public class ImageResult : IYoonResult
    {
        public YoonImage ResultImage { get; private set; } = null;
        public ObjectList ObjectList { get; private set; } = new ObjectList();
        public double TotalScore { get; private set; } = 0.0;

        public ImageResult()
        {
            //
        }

        public ImageResult(YoonImage pImageResult)
        {
            ResultImage = pImageResult.Clone() as YoonImage;
        }

        public ImageResult(YoonImage pImageResult, YoonObject<YoonRect2N> pObject)
        {
            ResultImage = pImageResult.Clone() as YoonImage;
            ObjectList.Add(pObject);
        }

        public ImageResult(ObjectList pListObject)
        {
            foreach (YoonObject<YoonRect2N> pObject in pListObject)
                ObjectList.Add(pObject);
        }

        public ImageResult(YoonImage pImageResult, ObjectList pListObject)
        {
            ResultImage = pImageResult.Clone() as YoonImage;
            foreach (YoonObject<YoonRect2N> pObject in pListObject)
                ObjectList.Add(pObject);
        }

        public string Combine(string strDelimiter)
        {
            return ObjectList.Count.ToString() + strDelimiter + TotalScore.ToString();
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if (pResult is ImageResult pResultProcessing)
            {
                if (!ResultImage.Equals(pResultProcessing.ResultImage) || TotalScore != pResultProcessing.TotalScore || ObjectList.Count != pResultProcessing.ObjectList.Count)
                    return false;
                for (int i = 0; i < ObjectList.Count; i++)
                    if (!ObjectList[i].Equals(pResultProcessing.ObjectList[i]))
                        return false;
                return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;
            if (pResult is ImageResult pResultProcessing)
            {
                if (pResultProcessing.ResultImage != null)
                    ResultImage = pResultProcessing.ResultImage.Clone() as YoonImage;
                foreach (YoonObject<YoonRect2N> pObject in pResultProcessing.ObjectList)
                    ObjectList.Add(pObject);
                TotalScore = pResultProcessing.TotalScore;
            }
        }

        public IYoonResult Clone()
        {
            ImageResult pTargetResult = new ImageResult();
            pTargetResult.ResultImage = ResultImage.Clone() as YoonImage;
            foreach (YoonObject<YoonRect2N> pObject in ObjectList)
                pTargetResult.ObjectList.Add(pObject);
            pTargetResult.TotalScore = TotalScore;
            return pTargetResult;
        }
    }
}
