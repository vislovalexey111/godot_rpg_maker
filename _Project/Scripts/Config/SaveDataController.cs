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
    private JsonSerializerOptions _jsonSerializerOptions;
    
    public SaveData SaveDatabase { get; private set; }
    
    public int SaveCount => SaveDatabase?.SaveCount ?? 0;
    
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

        SaveDatabase = new SaveData(new List<SaveDataEntry>());

        if (!File.Exists(_saveDatabasePath))
        {
            GD.Print("No save database found, creating a new one");
            UpdateDatabase(SaveDatabase);
        }
        else
        {
            try
            {
                string saveDatabaseString = File.ReadAllText(_saveDatabasePath);
                var saveDatabase = JsonSerializer.Deserialize<SaveData>(saveDatabaseString);
                
                if (saveDatabase.CheckForInvalidSaves()) UpdateDatabase(saveDatabase);
                
                SaveDatabase.Set(saveDatabase);
                GD.Print("Save database load successful");
            }
            catch (Exception exception)
            {
                GD.PrintErr($"Loading database file failed: {exception.Message},{exception.StackTrace}");
                SaveDatabase.Entries.Clear();
                UpdateDatabase(SaveDatabase);
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

    public bool TryLoadSession(int slotIndex)
    {
        if (IsDatabaseNull() || !SaveDatabase.TryGet(slotIndex, out SaveDataEntry entry)) return false;
        
        var path = entry.FilePath;

        if (!File.Exists(path))
        {
            GD.Print("No save file found for the entry with path: " + path);
            SaveDatabase.TryRemove(entry);
            UpdateDatabase(SaveDatabase);
            return false;
        }

        try
        {
            var session = JsonSerializer.Deserialize<SessionData>(File.ReadAllText(path));
            _sessionDataController.Load(session);
            return true;
        }
        catch(Exception exception)
        {
            GD.PrintErr($"Unable to load save file: {path}, {exception.Message}, {exception.StackTrace}");

            SaveDatabase.TryRemove(entry);
            File.Delete(path);
            UpdateDatabase(SaveDatabase);
            
            _sessionDataController.SetDefault();
            return false;
        }
    }

    public bool TrySaveSession(int slotIndex, out SaveDataEntry entry)
    {
        entry = null;
        
        if (IsDatabaseNull()) return false;

        DateTime now = DateTime.Now;
        
        if (!SaveDatabase.TryGet(slotIndex, out entry))
        {
            string saveFilePath = $"{_saveFolderPath}/{_saveFileName}{now:yyyy_MM_dd_HH_mm_ss}{_saveFileExtension}";
            GD.Print("Creating a new file by path: " + saveFilePath);
            entry = new SaveDataEntry(DateTime.Now, saveFilePath);
            SaveDatabase.Add(entry);
        }
        else
        {
            GD.Print("Modifying file: " + entry.FilePath);
            entry.LastUpdate = now;
        }
        
        _sessionDataController.Data.LastUpdate = now;
        
        UpdateDatabase(SaveDatabase);
        File.WriteAllText(entry.FilePath, JsonSerializer.Serialize(_sessionDataController.Data, _jsonSerializerOptions));
        return true;
    }

    
    public void RemoveSession(SaveDataEntry entry)
    {
        if (entry == null) return;
        
        if (File.Exists(entry.FilePath)) File.Delete(entry.FilePath);

        SaveDatabase.TryRemove(entry);
        UpdateDatabase(SaveDatabase);
    }

    private void UpdateDatabase(SaveData saveData)
    {
        File.WriteAllText(_saveDatabasePath, JsonSerializer.Serialize(saveData, _jsonSerializerOptions));
    }

    
    private bool IsDatabaseNull()
    {
        if (SaveDatabase != null) return false;
        
        GD.PrintErr("Save database is not initialized");
        return true;
    }
}
