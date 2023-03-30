using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Storage
{
    private readonly string _filePath;
    private BinaryFormatter formatter;

    public Storage()
    {
        var directory = Application.persistentDataPath;
        if (Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        _filePath = directory + "/GameSave.save";
        InitBinaryFormatter();
    }

    private void InitBinaryFormatter()
    {
        formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();

        var buildingsSurrogate = new BuildingsSerializationSurrogate();
        var soldersSurogate = new SoldersSerializationSurrogate();

        selector.AddSurrogate(typeof(Buildings), new StreamingContext(StreamingContextStates.All), buildingsSurrogate);
        selector.AddSurrogate(typeof(AlliesSolders), new StreamingContext(StreamingContextStates.All), soldersSurogate);

        formatter.SurrogateSelector = selector;
    }

    public object LoadGame(object saveDataByDefault)
    {
        if (!File.Exists(_filePath))
        {
            if (saveDataByDefault != null)
                SaveGame(saveDataByDefault);
            return saveDataByDefault;
        }

        var file = File.Open(_filePath, FileMode.Open);
        var savedData = formatter.Deserialize(file);
        file.Close();
        return savedData;
    }

    public void SaveGame(object saveData)
    {
        if (File.Exists((_filePath)))
            File.Delete(_filePath);
        
        var file = File.Create(_filePath);
        formatter.Serialize(file, saveData);
        file.Close();
    }
}
