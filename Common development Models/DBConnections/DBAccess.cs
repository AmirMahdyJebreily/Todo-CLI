using System.Text.Json;
using todo.codevmodels.exceptions;
using todo.Common_development_Models.DBConnections;

namespace todo.codevmodels;

public class DBAccess
{

    // Constant Fields
    private const string _appFolderName = ".todocli";
    private const string _tasks_file_name = "tasks.json";

    // Readonly Fields
    private static readonly string _app_folder_path = Path.Combine(_appFolderName);
    private static readonly string _app_internal_data_file_path = Path.Combine(_app_folder_path, "_application_internal_data_");

    //internal data
    private _internal_data_model _Internal_Data;

    private List<TodoTask> _tasks;
    public List<TodoTask> Tasks
    {
        get
        {
            if (_tasks == null) return new List<TodoTask>();
            return _tasks;
        }
    }


    // I use the Factory pattern because I need to call the async method when my class is created
    #region Factory

    // for run async proccess in start of class
    private async Task<DBAccess> _initDB()
    {
        await SetupInternalData();
        return this;
    }

    public static Task<DBAccess> CreateDBAsync() => new DBAccess()._initDB();

    #endregion

    #region Utils 

    // Checking the existence of the application directiory in _app_folder_path
    public bool AppDirectoryExists() => Directory.Exists(_app_folder_path);

    // Checking the existence of the application database in _app_folder_path
    public bool AppDBExists() => File.Exists(_Internal_Data._tasks_file_path);

    public async Task WriteJsonToTasksFile(FileStream dbFile) => await JsonSerializer.SerializeAsync(dbFile, Tasks);
    public async Task<List<TodoTask>> ReadJsonFromTasksFile(FileStream dbFile) => await JsonSerializer.DeserializeAsync<List<TodoTask>>(dbFile);

    public string GetTaskFilePath() => _Internal_Data._tasks_file_path;

    #endregion

    #region Internal data Setup

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

    // Check the path is valid or not 
    public static bool TaskFileLocationIsValid(string filePath)
    {
        try
        {
            var dir = new DirectoryInfo(filePath);
            return dir.Exists;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    // save all internall datas
    private async void SaveInternalData()
    {
        await File.WriteAllTextAsync(_app_internal_data_file_path, JsonSerializer.Serialize(_Internal_Data));
    }

    private async Task<_internal_data_model> ReadInternalData()
    {
        using (var file = File.OpenRead(_app_internal_data_file_path))
        {

            return await JsonSerializer.DeserializeAsync<_internal_data_model>(file);
        }

    }

    private async Task SetupInternalData()
    {
        var internalDataFile = new FileInfo(_app_internal_data_file_path);
        if (internalDataFile.Exists)
        {
            _Internal_Data = await ReadInternalData();
        }
        else
        {
            _Internal_Data = new _internal_data_model() { _tasks_file_path = string.Empty };
            if (!AppDirectoryExists())
            {
                // use makedirectory methode
                try { MakeDirectory(); }
                catch (DirectoryExistsException) { }
                catch (Exception) { throw; }
            }

            SaveInternalData();
        }
    }

    public bool TaskFilePathExist()
    {
        return (!string.IsNullOrEmpty(_Internal_Data._tasks_file_path)) && (File.Exists(_Internal_Data._tasks_file_path));
    }

    public void EnterTasksFilePath(string location)
    {

        if (DBAccess.TaskFileLocationIsValid(location))
        {
            _Internal_Data._tasks_file_path = Path.Combine(location, _tasks_file_name);
            SaveInternalData();
        }
        else
        {
            throw new FileNotFoundException("file not found");
        }

    }

    public async Task InitializeTasks()
    {
        this._tasks = await GetAllDataFromTasks();
    }

    #endregion

    #region Tasks file starting setup

    // make tasks json file in tasks_file_path
    public async Task MakeTasksFiles()
    {
        if (!AppDBExists())
        {
            using (FileStream file = File.Create(_Internal_Data._tasks_file_path))
            {

                await WriteJsonToTasksFile(file);
            }
        }
        else
        {
            throw new DBFileExistsException("the database file alredy exists");
        }
    }

    // Setup directory and database file in them paths
    public async Task SetupRequ()
    {
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
            if (File.Exists(_Internal_Data._tasks_file_path))
            {
                using FileStream file = File.OpenRead(_Internal_Data._tasks_file_path);
                var res = await ReadJsonFromTasksFile(file);
                await file.DisposeAsync();
                if (res is not null)
                    return res;
                // else
                return new List<TodoTask>();
            }
            else
            {
                await SetupRequ();
                return new List<TodoTask>();
            }
        }
        catch (Exception) { throw; }
    }

    // get all data that's saved in the db file
    public async Task<List<TodoTask>> GetAllDataFromTasks()
    {
        if (string.IsNullOrEmpty(_Internal_Data._tasks_file_path))
        {
            await SetupRequ();
            return new List<TodoTask>();
        }
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
            using FileStream file = File.Open(this._Internal_Data._tasks_file_path, FileMode.Create);
            await WriteJsonToTasksFile(file);
            await file.DisposeAsync();
        }
    }

    #endregion

    #region CRUD
    // add task to db
    public async Task AddNewTask(string title, bool isDone, int? parrentId)
    {
        if (parrentId == null)
        {
            _tasks.Add(new TodoTask()
            {
                Id = Utils.SetId(_tasks),
                Title = title,
                IsDone = isDone,
                SubTasks = new List<TodoTask>()
            });
        }
        else
        {
            TodoTask matchedItem = _tasks.FirstOrDefault(t => t.Id == parrentId);
            if (matchedItem != null)
            {
                _tasks[_tasks.IndexOf(matchedItem)].SubTasks.Add(new TodoTask()
                {
                    Id = Utils.SetId(_tasks),
                    Title = title,
                    IsDone = isDone,
                    SubTasks = new List<TodoTask>()
                });
            }
        }
        await WriteOnTasksFile();
    }

    // get all tasks from db
    public List<TodoTask> GetAllTasks()
    {
        return _tasks;
    }

    // get all tasks from db asyncronus
    public async Task<List<TodoTask>> GetAllTasksAsync()
    {
        _tasks = await ReadDataFromTasksFile();
        return _tasks;
    }

    // edit tasks
    public async Task EditTaskTitleById(int taskId, string newTitle)
    {
        _tasks[taskId].Title = newTitle;
        await WriteOnTasksFile();
    }

    // get task by id
    public TodoTask GetTaskById(int taskId)
    {
        return _tasks[taskId];
    }

    //set task done or not
    public async Task setTaskDoneState(int taskId, bool done)
    {
        _tasks[taskId].IsDone = done;
        await WriteOnTasksFile();
    }

    // delete task from file
    public async Task RemoveTask(int taskId)
    {
        _tasks.Remove(_tasks[taskId]);
        await WriteOnTasksFile();
    }

    #endregion
}
