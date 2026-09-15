using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

[GlobalClass]
public partial class SaveDataController : Resource
{
    [Export] private SessionDataController _sessionDataController;
    
    [ExportGroup("Config")]
    [Export] private string _saveDatabaseName = "SaveDatabase.json";
    [Export] private string _saveFolderName = "Saves";
    [Export] private string _saveFileName = "Save_";
    [Export] private string _saveFileExtension = ".json";

    private string _saveDatabasePath;
    private string _saveFolderPath;
    private SaveData _saveDatabase;

    private JsonSerializerOptions _jsonSerializerOptions;
    
    public void Init()
    {
        _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        
        // Loading Save database
        if (string.IsNullOrWhiteSpace(_saveDatabaseName))
        {
            GD.PrintErr("No save database file name provided");
            return;
        }
        
        string userDirPath = OS.GetUserDataDir();
        _saveDatabasePath = $"{userDirPath}/{_saveDatabaseName}";

        _saveDatabase = new SaveData(new List<SaveDataEntry>());

        if (!File.Exists(_saveDatabasePath))
        {
            GD.Print("No save database found, creating a new one");
            File.WriteAllText(_saveDatabasePath, JsonSerializer.Serialize(_saveDatabase, _jsonSerializerOptions));
        }
        else
        {
            try
            {
                string saveDatabaseString = File.ReadAllText(_saveDatabasePath);
                var saveDatabase = JsonSerializer.Deserialize<SaveData>(saveDatabaseString);
                _saveDatabase.Set(saveDatabase);
                GD.Print("Save database load successful");
            }
            catch (Exception exception)
            {
                GD.PrintErr($"Loading database file failed: {exception.Message},{exception.StackTrace}");
                _saveDatabase.Entries.Clear();
                File.WriteAllText(_saveDatabasePath, JsonSerializer.Serialize(_saveDatabase, _jsonSerializerOptions));
            }
        }

        if (string.IsNullOrWhiteSpace(_saveFolderName))
        {
            GD.PrintErr("No save folder name provided");
            return;
        }

        _saveFolderPath = $"{userDirPath}/{_saveFolderName}";

        if (!Directory.Exists(_saveFolderPath))
        {
            GD.Print("No save folder found, creating a new one");
            Directory.CreateDirectory(_saveFolderPath);
        }
    }

    public void LoadSession(int slotIndex)
    {
        if (_saveDatabase == null)
        {
            GD.PrintErr($"No save database initialized");
            return;
        }
        
        if (_saveDatabase.Entries.Count < slotIndex
            || _saveDatabase.Entries[slotIndex] == null)
        {
            GD.PrintErr("No save entry found");
            return;
        }

        var path = _saveDatabase.Entries[slotIndex].FilePath;

        try
        {
            var session = JsonSerializer.Deserialize<SessionData>(File.ReadAllText(path));
            _sessionDataController.Load(session);
        }
        catch(Exception exception)
        {
            GD.PrintErr($"Unable to load save file: {_saveFileName}, {exception.Message}, {exception.StackTrace}");
            _sessionDataController.SetDefault();
        }
    }

    public void SaveSession(int slotIndex)
    {
        if (_saveDatabase == null)
        {
            GD.PrintErr("No save database initialized");
            return;
        }

        var finalIndex = Math.Clamp(slotIndex, 0, _saveDatabase.Entries.Count);
        string saveFilePath = string.Empty;
        
        if (finalIndex == _saveDatabase.Entries.Count)
        {
            int nameIndex = finalIndex;

            string saveFilePrefix = $"{_saveFolderPath}/{_saveFileName}";
            
            while (File.Exists($"{saveFilePrefix}{nameIndex}{_saveFileExtension}"))
                ++nameIndex;

            saveFilePath = $"{saveFilePrefix}{nameIndex}{_saveFileExtension}";
            
            _saveDatabase.Entries.Add(new SaveDataEntry(
                DateTime.Now,
                saveFilePath
            ));
        }
        else
        {
            saveFilePath =  $"{_saveFolderPath}/{_saveFileName}{finalIndex}{_saveFileExtension}";
            _saveDatabase.Entries[finalIndex].LastUpdate = DateTime.Now;
        }
        
        _sessionDataController.Data.LastUpdate = DateTime.Now;
        File.WriteAllText(_saveDatabasePath, JsonSerializer.Serialize(_saveDatabase, _jsonSerializerOptions));
        File.WriteAllText(saveFilePath, JsonSerializer.Serialize(_sessionDataController.Data, _jsonSerializerOptions));
    }
}