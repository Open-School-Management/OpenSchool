namespace SharedKernel.Contracts;

public static class BaseCacheKeys
{
    public static string GetConfigKey(object ownerId) => $"config:{ownerId}";
    public static string GetSystemFullRecordsKey(string tableName) => $"system-full-records:{tableName}";
    public static string GetSystemRecordByIdKey(string tableName, object recordId) => $"system-record-by-id:{tableName}:{recordId}";
    public static string GetSystemRecordByForeignIdKey(string tableName, object foreignKeyId) => $"system-record-by-foreignkey-id:{tableName}:{foreignKeyId}";
    public static string GetFullRecordsKey(string tableName, object ownerId) => $"full-records:{tableName}:{ownerId}";
    public static string GetRecordByIdKey(string tableName, object recordId, object ownerId) => $"record-by-id:{tableName}:{recordId}:{ownerId}";
    public static string GetSecretKey(string keyName, object ownerId) => $"{keyName}:{ownerId}";

}