using System;
using System.Collections.Generic;

namespace CoDevModels
{
    public static class FileAccess
    {

        private static string BasePath
        {
            get
            {
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString();
                }

                return path;
            }
        }

        private const string _appFolderName = ".todocli";

        private static readonly string path = $"{BasePath}\\{_appFolderName}";


        // TODO : Methodes for CRUD data on file


    }
}