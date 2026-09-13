using System;

[Serializable]
public class SaveDataEntry : ILoadableData<SaveDataEntry>
{
    public DateTime LastUpdate { get; set; }
    public string FilePath { get; set; }
    
    public SaveDataEntry(DateTime lastUpdate, string filePath)
    {
        FilePath = filePath;
        LastUpdate = lastUpdate;
    }

    public void Set(SaveDataEntry data)
    {
        LastUpdate = data.LastUpdate;
        FilePath = data.FilePath;
    }
}