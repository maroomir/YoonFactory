using System;
using YoonFactory.Files;
using System.IO;

namespace YoonFactory.Param
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

        public bool Equals(YoonParameter pParam)
        {
            if (pParam.Parameter.Equals(Parameter) && pParam.ParameterType == ParameterType && pParam.RootDirectory == RootDirectory)
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

        public bool LoadParameter(bool bSaveIfFalse = false)
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
}
