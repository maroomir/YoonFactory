using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace YoonFactory.Files
{
    /* ZIP File Management Class @2020 */
    public class YoonZip
    {
        private string m_zipFilePath;
        public YoonZip(string strPath)
        {
            if (FileFactory.VerifyFileExtension(ref strPath, ".zip"))
                m_zipFilePath = strPath;
            else
                m_zipFilePath = "YoonZip.zip";
        }

        public string DirectoryPath { get; set; }

        public string ZipFilePath
        {
            get
            {
                return m_zipFilePath;
            }
            set
            {
                if (FileFactory.VerifyFileExtension(ref value, ".zip"))
                    m_zipFilePath = value;
            }
        }

        public void CompressZip()
        {
            if (!FileFactory.VerifyDirectory(DirectoryPath))
                return;

            List<string> fListFile = FileFactory.GetFilePaths(DirectoryPath);
            using (FileStream fs = new FileStream(ZipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (string str in fListFile)
                    {
                        string path = str.Substring(DirectoryPath.Length + 1);
                        zipArchive.CreateEntryFromFile(str, path);
                    }
                }
            }
        }

        public void CompressZip(string strDirPath)
        {
            if (!FileFactory.VerifyDirectory(strDirPath))
                return;

            List<string> fListFile = FileFactory.GetFilePaths(strDirPath);
            using (FileStream fs = new FileStream(ZipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (string str in fListFile)
                    {
                        string path = str.Substring(strDirPath.Length + 1);
                        za.CreateEntryFromFile(str, path);
                    }
                }
            }
        }

        public void ExtractZip()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            using (ZipArchive za = System.IO.Compression.ZipFile.OpenRead(ZipFilePath))
            {
                foreach (ZipArchiveEntry zaentry in za.Entries)
                {
                    string strFilePath = Path.Combine(DirectoryPath, zaentry.FullName);
                    string strSubDir = Path.GetDirectoryName(strFilePath);
                    if (!Directory.Exists(strSubDir))
                    {
                        Directory.CreateDirectory(strSubDir);
                    }
                    zaentry.ExtractToFile(strFilePath);
                }
            }
        }

        public void ExtractZip(string strDirPath)
        {
            if (!Directory.Exists(strDirPath))
            {
                Directory.CreateDirectory(strDirPath);
            }
            using (ZipArchive za = System.IO.Compression.ZipFile.OpenRead(ZipFilePath))
            {
                foreach (ZipArchiveEntry zaentry in za.Entries)
                {
                    string strFilePath = Path.Combine(strDirPath, zaentry.FullName);
                    string strSubDir = Path.GetDirectoryName(strFilePath);
                    if (!Directory.Exists(strSubDir))
                    {
                        Directory.CreateDirectory(strSubDir);
                    }
                    zaentry.ExtractToFile(strFilePath);
                }
            }
        }
    }

}
