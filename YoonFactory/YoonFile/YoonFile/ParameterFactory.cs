using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory;
using YoonFactory.Files;
using System.IO;

namespace YoonFactory.Files.Core
{
    public class YoonParameter
    {
        public IYoonParameter Parameter { get; set; }
        public Type ParameterType { get; private set; }

        public string RootDirectory { get; set; }

        public YoonParameter()
        {
            Parameter = null;
            ParameterType = typeof(object);
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public YoonParameter(IYoonParameter pParam, Type pType)
        {
            Parameter = pParam.Clone();
            ParameterType = pType;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public void SetParameter(IYoonParameter pParam, Type pType)
        {
            Parameter = pParam.Clone();
            ParameterType = pType;
        }

        public bool IsEqual(YoonParameter pParam)
        {
            if (pParam.Parameter.IsEqual(Parameter) && pParam.ParameterType == ParameterType && pParam.RootDirectory == RootDirectory)
                return true;
            else return false;
        }

        public bool SaveParameter()
        {
            return SaveParameter(ParameterType.FullName);
        }

        public bool SaveParameter(string strFileName)
        {
            if (RootDirectory == string.Empty || Parameter == null) return false;

            string strFilePath = Path.Combine(RootDirectory, string.Format(@"{0}.xml", strFileName));
            YoonXml pXml = new YoonXml(strFilePath);
            if (pXml.SaveFile(Parameter, ParameterType))
                return true;
            else
                return false;
        }

        public bool LoadParameter(bool bSaveIfFalse=false)
        {
            return LoadParameter(ParameterType.FullName, bSaveIfFalse);
        }

        public bool LoadParameter(string strFileName, bool bSaveIfFalse = false)
        {
            if (RootDirectory == string.Empty || Parameter == null) return false;

            object pParam;
            string strFilePath = Path.Combine(RootDirectory, string.Format(@"{0}.xml", strFileName));
            IYoonParameter pParamBk = Parameter.Clone();
            YoonXml pXml = new YoonXml(strFilePath);
            if (pXml.LoadFile(out pParam, ParameterType))
            {
                Parameter = pParam as IYoonParameter;
                if (Parameter != null)
                    return true;
                Parameter = pParamBk;
                if (bSaveIfFalse) SaveParameter();
            }
            return false;
        }
    }

    public class YoonContainer : IYoonContainer, IYoonContainer<YoonParameter>
    {
        #region IDisposable Support
        ~YoonContainer()
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
                if (ObjectDictionary != null)
                    ObjectDictionary.Clear();
                ObjectDictionary = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public Dictionary<string, YoonParameter> ObjectDictionary { get; private set; } = new Dictionary<string, YoonParameter>();
        public string RootDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        public YoonParameter this[string strKey]
        {
            get => GetValue(strKey);
            set => SetValue(strKey, value);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if(pContainer is YoonContainer pParamContainer)
            {
                ObjectDictionary.Clear();

                RootDirectory = pParamContainer.RootDirectory;
                ObjectDictionary = new Dictionary<string, YoonParameter>(pParamContainer.ObjectDictionary);
            }
        }

        public IYoonContainer Clone()
        {
            YoonContainer pContainer = new YoonContainer();
            pContainer.RootDirectory = RootDirectory;
            pContainer.ObjectDictionary = new Dictionary<string, YoonParameter>(ObjectDictionary);
            return pContainer;
        }

        public bool Add(string strKey, YoonParameter pParam)
        {
            try
            {
                ObjectDictionary.Add(strKey, pParam);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(string strKey)
        {
            return ObjectDictionary.Remove(strKey);
        }

        public void Clear()
        {
            ObjectDictionary.Clear();
        }

        public string GetKey(YoonParameter pParam)
        {
            foreach (string strKey in ObjectDictionary.Keys)
            {
                if (pParam.Parameter.IsEqual(ObjectDictionary[strKey].Parameter))
                    return strKey;
            }
            return string.Empty;
        }

        public YoonParameter GetValue(string strKey)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                return ObjectDictionary[strKey];
            else
                return default;
        }

        public void SetValue(string strKey, YoonParameter pParam)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                ObjectDictionary[strKey] = pParam;
            else
                Add(strKey, pParam);
        }

        public bool LoadValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty)
                return false;

            ObjectDictionary[strKey].RootDirectory = RootDirectory;
            if (!ObjectDictionary.ContainsKey(strKey))
                Add(strKey, new YoonParameter());

            if (ObjectDictionary[strKey].LoadParameter(strKey)) return true;
            else return false;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty || !ObjectDictionary.ContainsKey(strKey))
                return false;

            ObjectDictionary[strKey].RootDirectory = RootDirectory;
            return ObjectDictionary[strKey].SaveParameter(strKey);
        }
    }

    public class YoonTemplate : IYoonTemplate, IYoonTemplate<YoonParameter>
    {
        #region IDisposable Support
        ~YoonTemplate()
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
        public IYoonContainer<YoonParameter> Container { get; set; }

        public YoonTemplate()
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
            Container = new YoonContainer();
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if(pTemplate is YoonTemplate pTempOrigin)
            {
                Container.Dispose();
                Container = null;

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                Container = pTempOrigin.Container.Clone() as YoonContainer;
            }
        }

        public IYoonTemplate Clone()
        {
            YoonTemplate pTemplate = new YoonTemplate();
            pTemplate.No = No;
            pTemplate.Name = Name;
            pTemplate.RootDirectory = RootDirectory;
            pTemplate.Container = Container.Clone() as YoonContainer;

            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            bool bResult = true;
            string strSection = string.Format("{0}_{1}", No, Name);
            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            Container.RootDirectory = Path.Combine(RootDirectory, strSection);
            YoonIni pIni = new YoonIni(strIniFilePath);
            {
                pIni.LoadFile();
                int nCount = pIni[strSection]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    string strKey = pIni[strSection][iParam].ToString(string.Empty);
                    if (!Container.LoadValue(strKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            bool bResult = true;
            int iParam = 0;
            string strSection = string.Format("{0}_{1}", No, Name);
            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            Container.RootDirectory = Path.Combine(RootDirectory, strSection);
            YoonIni pIni = new YoonIni(strIniFilePath);
            pIni[strSection]["Count"] = Container.ObjectDictionary.Count;
            foreach(string strKey in Container.ObjectDictionary.Keys)
            {
                pIni[strSection][iParam] = strKey;
                if (!Container.SaveValue(strKey))
                    bResult = false;
                iParam++;
            }
            pIni.SaveFile();
            return bResult;
        }
    }
}
