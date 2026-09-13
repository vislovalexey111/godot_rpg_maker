using System;
using System.Collections.Generic;

[Serializable]
public class SaveData : ILoadableData<SaveData>
{
    public List<SaveDataEntry> Entries { get; private set; }

    public void Set(SaveData saveData)
    {
        int min = Math.Min(Entries.Count, saveData.Entries.Count);
        for(int i = 0; i < min; i++) Entries[i].Set(saveData.Entries[i]);
    }
    
    public SaveData(List<SaveDataEntry> entries)
    {
        Entries = entries;
    }
}