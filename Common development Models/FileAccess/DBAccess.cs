using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using todo.codevmodels.exceptions;

namespace todo.codevmodels;

public class DBAccess
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

    private List<TodoTask> _tasks;
    public List<TodoTask> Tasks
    {
        get { return _tasks; }
    }


    // I use the Factory pattern because I need to call the async method when my class is created
    #region Factory
    private async Task<DBAccess> _initDB()
    {
        _tasks = await GetAllDataFromDB();
        return this;
    }

    public static Task<DBAccess> CreateDBAsync() => new DBAccess()._initDB();

    #endregion

    #region Static Methodes

    // Checking the existence of the application directiory in C:\ drive...
    public static bool AppDirectoryExists() => Directory.Exists(path);

    // Checking the existence of the application database in C:\ drive...
    public static bool AppDBExists() => File.Exists(db_path);

    public async Task WriteDB(FileStream dbFile) => await JsonSerializer.SerializeAsync(dbFile, Tasks);
    public async Task<List<TodoTask>> ReadDB(FileStream dbFile) => await JsonSerializer.DeserializeAsync<List<TodoTask>>(dbFile);

    #endregion

    #region DBFileSetup

    // make application directory in path
    public void MakeDirectory()
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
    public async Task MakeDB()
    {
        if (!AppDBExists())
        {
            using FileStream file = File.Create(db_path);
            await WriteDB(file);

        }
        else
        {
            throw new DBFileExistsException("the database file alredy exists");
        }
    }

    // Setup directory and database file in path
    public async Task SetupRequ()
    {
        // use makedirectory methode
        try { MakeDirectory(); }
        catch (DirectoryExistsException) { }
        catch (Exception) { throw; }

        // use make db methode
        try { await MakeDB(); }
        catch (DBFileExistsException) { }
        catch (Exception) { throw; }
    }

    // Read all data from db file
    public async Task<List<TodoTask>> ReadDataFromDBFile()
    {
        try
        {
            if (!File.Exists(db_path))
            {
                using FileStream file = File.OpenRead(db_path);
                var res = await ReadDB(file);
                await file.DisposeAsync();
                return res;
            }
            else
            {
                throw new FileNotFoundException("the databse file was not found");
            }
        }
        catch (Exception) { throw; }
    }

    public async Task<List<TodoTask>> GetAllDataFromDB()
    {
        if (!AppDirectoryExists())
        {
            await SetupRequ();
            return new List<TodoTask>();
        }

        if (!AppDBExists())
        {
            await SetupRequ();
            return new List<TodoTask>();
        }

        return await ReadDataFromDBFile();
    }

    #endregion

    #region CRUD
    public void AddNewTask(TodoTask task)
    {
        _tasks.Add(task);

    }
    #endregion


}
