using System;
using YoonFactory.Files;
using System.IO;


namespace YoonFactory.Param
{
    public class YoonResult
    {
        public IYoonResult Result { get; private set; }
        public Type ResultType { get; private set; } = null;

        public string RootDirectory { get; set; } = string.Empty;

        public YoonResult()
        {
            Result = null;
            ResultType = null;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public YoonResult(IYoonResult pResult, Type pType)
        {
            Result = pResult.Clone();
            ResultType = pType;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public void SetResult(IYoonResult pResult, Type pType)
        {
            if (ResultType != null && ResultType != pType) return;
            Result = pResult.Clone();
            ResultType = pType;
        }

        public bool Equals(YoonResult pResult)
        {
            if (pResult.Result.Equals(Result) && pResult.ResultType == ResultType && pResult.RootDirectory == RootDirectory)
                return true;
            else return false;
        }

        public bool SaveResult()
        {
            return SaveResult(ResultType.FullName);
        }

        public bool SaveResult(string strFileName)
        {
            if (RootDirectory == string.Empty || Result == null) return false;

            string strFilePath = Path.Combine(RootDirectory, string.Format(@"{0}.csv", strFileName));
            YoonCsv pCsv = new YoonCsv(strFilePath);
            pCsv.SetLine(Result.Combine(","));
            return pCsv.SaveFile();
        }
    }
}
