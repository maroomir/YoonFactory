namespace YoonFactory.Align
{
    /// <summary>
    /// Align 결과값을 담는 클래스
    /// </summary>
    public class AlignResult : IYoonResult
    {
        private double fResultX;
        private double fResultY;
        private double fResultT;
        public double ResultX
        {
            get { return fResultX; }
            set { fResultX = value; }
        }
        public double ResultY
        {
            get { return fResultY; }
            set { fResultY = value; }
        }
        public double ResultT
        {
            get { return fResultT; }
            set { fResultT = value; }
        }

        public AlignResult()
        {
            fResultX = 0.0;
            fResultY = 0.0;
            fResultT = 0.0;
        }

        public string Combine(string strDelimiter)
        {
            return fResultX.ToString() + strDelimiter + fResultY.ToString() + strDelimiter + fResultT.ToString();
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if (pResult is AlignResult pResultAlign)
            {
                if (ResultX == pResultAlign.ResultX && ResultY == pResultAlign.ResultY && ResultT == pResultAlign.ResultT)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if (pResult is AlignResult pResultAlign)
            {
                ResultX = pResultAlign.ResultX;
                ResultY = pResultAlign.ResultY;
                ResultT = pResultAlign.ResultT;
            }
        }

        public IYoonResult Clone()
        {
            AlignResult pTargetResult = new AlignResult();
            pTargetResult.ResultX = ResultX;
            pTargetResult.ResultY = ResultY;
            pTargetResult.ResultT = ResultT;
            return pTargetResult;
        }
    }

}
