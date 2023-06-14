using System;
using System.Collections.Generic;
using todo.codevmodels.exceptions;

namespace todo.codevmodels;

public class FileAccess
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


    // Constructos
    public FileAccess()
    {
        try
        {
            SetupRequ();
        }
        catch (System.Exception ex) { throw ex; }

    }


    #region Static Methodes

    // Checking the existence of the application directiory in C:\ drive...
    public static bool AppDirectoryExists() => Directory.Exists(path);

    // Checking the existence of the application database in C:\ drive...
    public static bool AppDBExists() => File.Exists(db_path);

    #endregion

    #region DBFileSetup

    // make application directory in c:\
    public static void MakeDirectory()
    {
        if (!AppDirectoryExists())
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            throw new DirectoryExistsException("the directory alredy exists");
        }
    }
    // make database json file in path
    public static void MakeDB()
    {
        if (!AppDBExists())
        {
            File.Create(db_path);
        }
        else
        {
            throw new DBFileExistsException("the database file alredy exists");
        }
    }
    // Setup directory and database file in c:\
    public static void SetupRequ()
    {
        // use makedirectory methode
        try
        {
            MakeDirectory();
        }
        catch (DirectoryExistsException) { }
        catch { throw; }

        // use make db methode
        try
        {
            MakeDB();
        }
        catch (DBFileExistsException) { }
        catch { throw; }

    }

    #endregion

    #region CRUD
    
    #endregion


}
