using System.Collections.Generic;
using System.IO;
using YoonFactory.Files;

namespace YoonFactory.Cognex
{
    public class ResultTemplate : ResultContainer, IYoonTemplate
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string RootDirectory { get; set; }

        public override string ToString()
        {
            return string.Format("{0:D2}_{1}", No, Name);
        }

        public ResultTemplate()
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(DefaultComparer);
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is ResultTemplate pTempOrigin)
            {
                Clear();

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                foreach (eYoonCognexType pKey in pTempOrigin.Keys)
                {
                    Add(pKey, pTempOrigin[pKey]);
                }
            }
        }

        public new IYoonTemplate Clone()
        {
            ResultTemplate pTemplate = new ResultTemplate();
            {
                pTemplate.No = No;
                pTemplate.Name = Name;
                pTemplate.RootDirectory = RootDirectory;
                pTemplate.m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(m_pDicSection, DefaultComparer);
            }
            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicSection == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"ToolTemplate.ini");
            base.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                pIni.LoadFile();
                No = pIni["HEAD"]["No"].ToInt(No);
                Name = pIni["HEAD"]["Name"].ToString(Name);
                int nCount = pIni["HEAD"]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    eYoonCognexType pKey = pIni["KEY"][iParam.ToString()].To(eYoonCognexType.None);
                    if (!LoadValue(pKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicSection == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"ToolTemplate.ini");
            base.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                int iParam = 0;
                pIni["HEAD"]["No"] = No;
                pIni["HEAD"]["Name"] = Name;
                pIni["HEAD"]["Count"] = Count;
                foreach (eYoonCognexType pKey in Keys)
                {
                    pIni["KEY"][(iParam++).ToString()] = pKey.ToString();
                    if (!SaveValue(pKey))
                        bResult = false;
                }
                pIni.SaveFile();
            }
            return bResult;
        }
    }
}
