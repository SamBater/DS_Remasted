using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    public static bool WriteToFile(string fileName,string content)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(path,content);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {path} with exception {e}");
            return false;
        }
    }

    public static bool ReadFromFile(string fileName, out string result)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            result = File.ReadAllText(path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read {path} with exception {e}");
            result = "";
            return false;
        }
    }
}
