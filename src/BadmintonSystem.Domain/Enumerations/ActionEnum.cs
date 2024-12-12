namespace BadmintonSystem.Domain.Enumerations;

public enum ActionEnum
{
    CREATE = 0, // => 1
    READ = 1, // => 2
    UPDATE = 2, // => 4
    DELETE = 3, // => 8
    IMPORT = 4, // => 16
    EXPORT = 5 // => 32
}
