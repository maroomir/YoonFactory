using System;
using System.Collections.Generic;
using System.IO;

namespace YoonFactory.Files
{
    public interface IYoonFile
    {
        string FilePath { get; }

        void CopyFrom(IYoonFile pFile);
        IYoonFile Clone();
        bool IsFileExist();
    }

    public interface IYoonParameter
    {
        bool IsEqual(IYoonParameter pParam);

        void CopyFrom(IYoonParameter pParam);
        IYoonParameter Clone();
    }

    public interface IYoonContainer : IDisposable
    {
        string RootDirectory { get; set; }  // \\<CONTAINER> (Save On this Folder)

        void CopyFrom(IYoonContainer pContainer);
        IYoonContainer Clone();
        void Clear();
        bool LoadValue(string strKey);
        bool SaveValue(string strKey);
    }

    public interface IYoonContainer<T> : IYoonContainer
    {
        Dictionary<string, T> ObjectDictionary { get; }

        bool Add(string strKey, T pValue);
        bool Remove(string strKey);
        string GetKey(T pValue);
        T GetValue(string strKey);
        void SetValue(string strKey, T pValue);
    }

    public interface IYoonTemplate : IDisposable
    {
        int No { get; set; }
        string Name { get; set; }
        string RootDirectory { get; set; }

        void CopyFrom(IYoonTemplate pTemplate);
        IYoonTemplate Clone();

        bool SaveTemplate();
        bool LoadTemplate();
    }

    public interface IYoonTemplate<T> : IYoonTemplate
    {
        IYoonContainer<T> Container { get; set; }
    }
}
