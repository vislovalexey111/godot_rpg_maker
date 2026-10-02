using System;
using System.Collections.Generic;
using System.IO;
using Godot;

[Serializable]
public class SaveData : ILoadableData<SaveData>
{
    public List<SaveDataEntry> Entries { get; private set; }
    public int SaveCount => Entries?.Count ?? 0;

    public void Set(SaveData saveData)
    {
        int loadSaveCount = saveData.SaveCount;
        GD.Print($"loading entries count: {loadSaveCount}");
        int min = Math.Min(Entries.Count, loadSaveCount);

        for(int i = 0; i < min; i++) Entries[i].Set(saveData.Entries[i]);

        if (loadSaveCount <= Entries.Count) return;
        
        // If loaded save data has more entries, then we do have already - we're expanding our entries
        // with the rest loaded ones
        Entries.EnsureCapacity(loadSaveCount);
        
        for(int i = min; i < loadSaveCount; i++)
        {
            var entry = saveData.Entries[i];
            Entries.Add(new SaveDataEntry(entry.LastUpdate, entry.FilePath));
        }
    }
    
    public SaveData(List<SaveDataEntry> entries)
    {
        Entries = entries;
    }
    
    public bool CheckForInvalidSaves()
    {
        bool result = false;
        
        for (int i = Entries.Count - 1; i >= 0; i--)
        {
            var path = Entries[i].FilePath;
            if (File.Exists(path)) continue;

            result = true;
            Entries.RemoveAt(i);
        }

        return result;
    }
    
    public void Add(SaveDataEntry saveDataEntry) => Entries.Add(saveDataEntry);
    public bool TryRemove(SaveDataEntry saveDataEntry) => Entries.Remove(saveDataEntry);

    public bool Contains(SaveDataEntry saveDataEntry)
    {
        return saveDataEntry != null && Entries.Contains(saveDataEntry);
    }

    public bool TryGet(int index, out SaveDataEntry dataEntry)
    {
        dataEntry = null;
        
        if (index < 0 || index >= Entries.Count) return false;
        
        dataEntry = Entries[index];
        return dataEntry != null;
    }
}