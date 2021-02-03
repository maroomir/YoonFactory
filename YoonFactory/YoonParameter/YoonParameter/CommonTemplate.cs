using System;
using YoonFactory.Files;
using System.IO;

namespace YoonFactory.Param
{
    public class CommonTemplate<TKey, TValue> : IYoonTemplate where TKey : IConvertible
    {
        #region IDisposable Support
        ~CommonTemplate()
        {
            this.Dispose(false);
        }

        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                ////  .Net Framework에 의해 관리되는 리소스를 여기서 정리합니다.
                if (Container != null)
                    Container.Dispose();
                Container = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public int No { get; set; }
        public string Name { get; set; }
        public string RootDirectory { get; set; }

        public override string ToString()
        {
            return string.Format("{0:D2}_{1}", No, Name);
        }

        public IYoonContainer<TKey, TValue> Container { get; set; }


        public CommonTemplate(IYoonContainer<TKey, TValue> pContainer)
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            Container = pContainer;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is CommonTemplate<TKey, TValue> pTempOrigin)
            {
                Container.Dispose();
                Container = null;

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                Container = pTempOrigin.Container.Clone();
            }
        }

        public IYoonTemplate Clone()
        {
            CommonTemplate<TKey, TValue> pTemplate = new CommonTemplate<TKey, TValue>(Container);
            pTemplate.No = No;
            pTemplate.Name = Name;
            pTemplate.RootDirectory = RootDirectory;

            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"CommonTemplate.ini");
            Container.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                pIni.LoadFile();
                string strHeadName = "HEAD:" + typeof(TValue).ToString();
                No = pIni[strHeadName]["No"].ToInt(No);
                Name = pIni[strHeadName]["Name"].ToString(Name);
                int nCount = pIni[strHeadName]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    string strKeyName = "KEY:" + typeof(TValue).ToString();
                    TKey pKey = pIni[strKeyName][iParam.ToString()].To(default(TKey));
                    if (!Container.LoadValue(pKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"CommonTemplate.ini");
            Container.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                int iParam = 0;
                string strHeadName = "HEAD:" + typeof(TValue).ToString();
                pIni[strHeadName]["No"] = No;
                pIni[strHeadName]["Name"] = Name;
                pIni[strHeadName]["Count"] = Container.Count;
                foreach (TKey pKey in Container.Keys)
                {
                    string strKeyName = "KEY:" + typeof(TValue).ToString();
                    pIni[strKeyName][(iParam++).ToString()] = pKey.ToString();
                    if (!Container.SaveValue(pKey))
                        bResult = false;
                }
                pIni.SaveFile();
            }
            return bResult;
        }
    }
}
