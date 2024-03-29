﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory
{

    public interface IYoonParameter
    {
        int GetLength();
        void Set(params string[] pArgs);

        bool Equals(IYoonParameter pParam);
        void CopyFrom(IYoonParameter pParam);
        IYoonParameter Clone();
    }

    public interface IYoonResult
    {
        int GetLength();
        
        string Combine(string strDelimiter);
        bool Insert(string strCombineResult, string strDelimiter);

        bool Equals(IYoonResult pResult);
        void CopyFrom(IYoonResult pResult);
        IYoonResult Clone();
    }

    public interface IYoonContainer : IDisposable
    {
        string FilesDirectory { get; set; }  // \\<CONTAINER> (Save On this Folder)

        void CopyFrom(IYoonContainer pContainer);
        IYoonContainer Clone();
        void Clear();
    }

    public interface IYoonContainer<TKey, TValue> : IYoonContainer, IYoonSection<TKey, TValue> where TKey : IConvertible
    {
        new IYoonContainer<TKey, TValue> Clone();
        bool LoadValue(TKey pKey);
        bool SaveValue(TKey pKey);
    }

    public interface IYoonTemplate : IDisposable
    {
        int No { get; set; }
        string Name { get; set; }
        string RootDirectory { get; set; }

        void CopyFrom(IYoonTemplate pTemplate);
        IYoonTemplate Clone();
        string ToString();

        bool SaveTemplate();
        bool LoadTemplate();
    }
}
