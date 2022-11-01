using System.IO;

namespace zenref.Tests
{
    public static class DatabaseHelper
    {
        public static bool isFile(string databaseName)
        {
            if (File.Exists(databaseName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteFile(string databaseName)
        {
            if (isFile(databaseName))
            {
                File.Delete(databaseName);
            }
        }
    }
}
