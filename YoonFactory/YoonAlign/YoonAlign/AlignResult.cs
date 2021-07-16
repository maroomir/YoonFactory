namespace YoonFactory.Align
{
    /// <summary>
    /// Align 결과값을 담는 클래스
    /// </summary>
    public class AlignResult : IYoonResult
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }

        public AlignResult()
        {
            X = 0.0;
            Y = 0.0;
            Theta = 0.0;
        }

        public string Combine(string strDelimiter)
        {
            return X.ToString() + strDelimiter + Y.ToString() + strDelimiter + Theta.ToString();
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if (pResult is AlignResult pResultAlign)
            {
                if (X == pResultAlign.X && Y == pResultAlign.Y && Theta == pResultAlign.Theta)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if (pResult is AlignResult pResultAlign)
            {
                X = pResultAlign.X;
                Y = pResultAlign.Y;
                Theta = pResultAlign.Theta;
            }
        }

        public IYoonResult Clone()
        {
            AlignResult pTargetResult = new AlignResult();
            pTargetResult.X = X;
            pTargetResult.Y = Y;
            pTargetResult.Theta = Theta;
            return pTargetResult;
        }
    }
}
