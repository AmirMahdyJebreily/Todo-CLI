using System.Text.Json;
using todo.codevmodels.exceptions;

namespace todo.codevmodels;

public class DBAccess
{
    // get windows user folder _app_folder_path
    private static string BasePath
    {
        get
        {
            string _app_folder_path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                _app_folder_path = Directory.GetParent(_app_folder_path).ToString();
            }

            return _app_folder_path;
        }
    }

    // Constant Fields
    private const string _appFolderName = ".todocli";
    private const string _tasks_file_name = "tasks.json";

    // Readonly Fields
    private static readonly string _app_folder_path = Path.Combine(BasePath, _appFolderName);
    private static readonly string _app_internal_data_file_path = Path.Combine(_app_folder_path, "_application_internal_data_");
    private string tasks_file_path = string.Empty;

    private List<TodoTask> _tasks;
    public List<TodoTask> Tasks
    {
        get { return _tasks; }
    }


    // Constructors
    private DBAccess(string usr_inp_path)
    {
        this.tasks_file_path = Path.Combine(usr_inp_path, _tasks_file_name);
    }

    private DBAccess()
    {
        this.tasks_file_path = Path.Combine(_app_folder_path, _tasks_file_name);
    }

    // I use the Factory pattern because I need to call the async method when my class is created
    #region Factory

    // for run async proccess in start of class
    private async Task<DBAccess> _initDB()
    {
        _tasks = await GetAllDataFromTasks();
        return this;
    }

    public static Task<DBAccess> CreateDBAsync(string usr_int_path) => new DBAccess(usr_int_path)._initDB();
    public static Task<DBAccess> CreateDBAsync() => new DBAccess()._initDB();

    #endregion

    #region Utils 

    // Checking the existence of the application directiory in _app_folder_path
    public bool AppDirectoryExists() => Directory.Exists(_app_folder_path);

    // Checking the existence of the application database in _app_folder_path
    public bool AppDBExists() => File.Exists(tasks_file_path);

    public async Task WriteJsonToTasksFile(FileStream dbFile) => await JsonSerializer.SerializeAsync(dbFile, Tasks);
    public async Task<List<TodoTask>> ReadJsonFromTasksFile(FileStream dbFile) => await JsonSerializer.DeserializeAsync<List<TodoTask>>(dbFile);

    #endregion

    #region Internal data Setup
    // save all internall datas
    public async void SaveInternalData()
    {

        await File.WriteAllTextAsync(_app_internal_data_file_path
           , JsonSerializer.Serialize(
               new
               {
                   Path = tasks_file_path,
               }));
    }
    #endregion

    #region Tasks file starting setup

    // make application directory in _app_folder_path
    public void MakeDirectory()
    {
        if (!AppDirectoryExists())
        {
            Directory.CreateDirectory(_app_folder_path);
        }
        else
        {
            throw new DirectoryExistsException("the directory alredy exists");
        }
    }

    // make tasks json file in tasks_file_path
    public async Task MakeTasksFiles()
    {
        if (!AppDBExists())
        {
            using FileStream file = File.Create(tasks_file_path);
            await WriteJsonToTasksFile(file);

        }
        else
        {
            throw new DBFileExistsException("the database file alredy exists");
        }
    }

    // Setup directory and database file in them paths
    public async Task SetupRequ()
    {
        // use makedirectory methode
        try { MakeDirectory(); }
        catch (DirectoryExistsException) { }
        catch (Exception) { throw; }

        // use make db methode
        try { await MakeTasksFiles(); }
        catch (DBFileExistsException) { }
        catch (Exception) { throw; }
    }

    #endregion


    #region Tasks file using API

    // Read all data from db file
    public async Task<List<TodoTask>> ReadDataFromTasksFile()
    {
        try
        {
            if (!File.Exists(tasks_file_path))
            {
                using FileStream file = File.OpenRead(tasks_file_path);
                var res = await ReadJsonFromTasksFile(file);
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

    // get all data that's saved in the db file
    public async Task<List<TodoTask>> GetAllDataFromTasks()
    {
        if (!AppDBExists())
        {
            await SetupRequ();
            return new List<TodoTask>();
        }

        return await ReadDataFromTasksFile();
    }

    // write all data into tasks file
    public async Task WriteOnTasksFile()
    {
        if (!AppDBExists())
        {
            await SetupRequ();
        }
        else
        {
            using FileStream file = File.OpenRead(this.tasks_file_path);
            await WriteJsonToTasksFile(file);
            await file.DisposeAsync();
        }
    }

    #endregion

    #region CRUD
    public async Task AddNewTask(TodoTask task)
    {
        _tasks.Add(task);
        await WriteOnTasksFile();
    }
    #endregion

}
