using System;

namespace todo.codevmodels.exceptions;


[System.Serializable]
public class DirectoryExistsException : System.Exception
{
    public DirectoryExistsException() { 
        
    }
    public DirectoryExistsException(string message) : base(message) { }
    public DirectoryExistsException(string message, System.Exception inner) : base(message, inner) { }
    protected DirectoryExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
};


[System.Serializable]
public class DBFileExistsException : System.Exception
{
    public DBFileExistsException() { }
    public DBFileExistsException(string message) : base(message) { }
    public DBFileExistsException(string message, System.Exception inner) : base(message, inner) { }
    protected DBFileExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
