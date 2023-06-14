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

        // Constant Fields
        private const string _appFolderName = ".todocli";
        private const string _appDBName = "tasks.json";

        // Readonly Fields
        private static readonly string path = Path.Combine(BasePath, _appFolderName);
        private static readonly string db_path = Path.Combine(path, _appDBName);


        #region Static Methodes

        // Checking the existence of the application directiory in C:\ drive...
        public static bool AppDirectoryExists() => Directory.Exists(path);

        // Checking the existence of the application database in C:\ drive...
        public static bool AppDBExists() => File.Exists(db_path);

        #endregion

        #region CRUD
            
        #endregion


    }
}