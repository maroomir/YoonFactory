using System;
using System.Collections.Generic;
using System.IO;

namespace YoonFactory.Files
{
    /* 일반 파일을 관리하는 Class */
    public static class FileFactory
    {
        public static bool VerifyDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    if (Directory.Exists(path))
                        return true;
                }
                else
                    return true;
            }
            return false;
        }

        public static bool VerifyFilePath(string path, bool bCreateFile = true)
        {
            if (!string.IsNullOrEmpty(path))
            {
                FileInfo fi = new FileInfo(path);
                if (!VerifyDirectory(fi.DirectoryName))
                    return false;

                try
                {
                    if (!fi.Exists)
                    {
                        if (!bCreateFile) return false;
                        FileStream fs = fi.Create();
                        fs.Close();
                        return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// VerifyFileExtension
        /// </summary>
        /// <param name="path"> File Path </param>
        /// <param name="strExt"> Extension string as like ".Ext" </param>
        /// <param name="bChangeExtension"> To change the extension as strExt </param>
        /// <returns></returns>
        public static bool VerifyFileExtension(ref string path, string strExt, bool bChangeExtension = false, bool bCreateFile = false)
        {
            if (VerifyFilePath(path, bCreateFile))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Extension != strExt)
                {
                    if (!bChangeExtension) return false;
                    string strFilePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(path) + strExt);
                    if (!VerifyFilePath(strFilePath, bCreateFile)) return false;
                    path = strFilePath;
                    return true;
                }
                else
                    return true;
            }
            return false;
        }

        public static bool VerifyFileExtensions(ref string path, params string[] pArgs)
        {
            bool bExist = false;
            foreach (string strExt in pArgs)
            {
                bExist = VerifyFileExtension(ref path, strExt, false, false);
                if (bExist) break;
            }
            return bExist;
        }

        public static bool IsFileExist(string filename)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            return fi.Exists;
        }

        public static List<string> GetFileListInDir(string rootPath, List<string> fListFile)
        {
            if (fListFile == null)
                return null;
            FileAttributes fAttribute = File.GetAttributes(rootPath);
            //// If rootpath is Directory
            if ((fAttribute & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo di = new DirectoryInfo(rootPath);
                //// continues abtract subdirectory
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    GetFileListInDir(dir.FullName, fListFile);
                }
                //// abstract subfiles
                foreach (FileInfo fir in di.GetFiles())
                {
                    GetFileListInDir(fir.FullName, fListFile);
                }
            }
            else
            {
                FileInfo fi = new FileInfo(rootPath);
                fListFile.Add(fi.FullName);
            }
            return fListFile;
        }

        public static string GetTextFromFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return string.Empty;
            }

            string strComplete = string.Empty;
            using (StreamReader sr = new StreamReader(fs))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                    strComplete += strLine;
            }
            return strComplete;
        }

        public static List<string> GetTextLinesFromFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return null;
            }

            List<string> pListTextComplete = new List<string>();
            using (StreamReader sr = new StreamReader(fs))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                    pListTextComplete.Add(strLine);
            }
            return pListTextComplete;
        }

        public static bool AppendTextToFile(string path, string data)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Append);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return false;
            }

            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(data);
            sw.Close();
            return true;
        }

        public static bool DeleteFilePath(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                if (!VerifyDirectory(fi.DirectoryName))
                    return false;

                try
                {
                    if (fi.Exists)
                    {
                        fi.Delete();
                        return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        public static void DeleteExtensionFilesInDirectory(string dirPath, string strExt)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (file.Extension == strExt)
                        file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteExtensionFilesInDirectory(dir.FullName, strExt);
            }
        }

        public static void DeleteIncludeSpecificInDirectory(string dirPath, string strSpecific, bool bCheckFileNameOnly = false)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (bCheckFileNameOnly)
                    {
                        string strFileName = file.Name + "." + file.Extension;
                        if (strFileName.Contains(strSpecific))
                            file.Delete();
                    }
                    else
                    {
                        if (file.FullName.Contains(strSpecific))
                            file.Delete();
                    }
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteExtensionFilesInDirectory(dir.FullName, strSpecific);
            }
        }

        public static void DeleteOldFilesInDirectory(string dirPath, int nDateSpan)
        {
            DeleteOldFilesInDirectory(dirPath, DateTime.Now, nDateSpan);
        }

        public static void DeleteOldFilesInDirectory(string dirPath, DateTime pDateStart, int nDateSpan)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    TimeSpan pSpan = pDateStart - file.CreationTime;

                    if ((int)pSpan.TotalDays >= nDateSpan)
                        file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteOldFilesInDirectory(dir.FullName, pDateStart, nDateSpan);
            }
        }

        public static void DeleteAllFilesInDirectory(string dirPath)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    DeleteAllFilesInDirectory(dirPath);
                }
            }
        }
    }

}
