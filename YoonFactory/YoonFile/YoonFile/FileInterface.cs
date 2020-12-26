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
}
