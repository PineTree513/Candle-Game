using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager : MonoBehaviour
{
    public static bool Save(string saveName, SaveData saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists("C:/Users/cayde/AppData/LocalLow/DefaultCompany/Candle Game" + "/saves"))
        {
            Directory.CreateDirectory("C:/Users/cayde/AppData/LocalLow/DefaultCompany/Candle Game" + "/saves");
        }

        string path = "C:/Users/cayde/AppData/LocalLow/DefaultCompany/Candle Game" + "/saves/" + saveName + ".save";

        FileStream file = File.Create(path);

        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public static SaveData Load(string saveName)
    {
        string path = "C:/Users/cayde/AppData/LocalLow/DefaultCompany/Candle Game" + "/saves/" + saveName + ".save";

        if (!File.Exists(path))
        {
            Debug.Log("No file at: " + path);
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            SaveData saveData = (SaveData)formatter.Deserialize(file);
            Debug.Log("Successful load at: " + path);
            file.Close();
            return saveData;
        }
        catch
        {
            Debug.Log("Failed to load file at: " + path);
            file.Close();
            return null;
        }
    }


    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}
