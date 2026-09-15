using System.Collections.Generic;
using Godot;

public partial class ControllerDatabase<T1, T2> : Resource
    where T1 : DataController<T2>
{
    public Dictionary<string, T1> Items { get; private set; }
    
    // Godot does not support generic exports, so children will have specific exports
    // Children will have public init function, that injects exports to this protected function
    protected void Init(T1[] items)
    {
        Items = new Dictionary<string, T1>(items.Length);
        
        foreach (var item in items)
        {
            item.Init();
            Items.Add(item.ResourceName, item);
        }
    }
    
    public bool TryGetItem(string id, out T1 item) => Items.TryGetValue(id, out item);

    public Dictionary<string, T2> CreateDataDictionary()
    {
        var dictionary = new Dictionary<string, T2>(Items.Count);
        
        foreach(var entry in Items) dictionary.Add(entry.Key, entry.Value.Data);

        return dictionary;
    }

    public void SetDefault()
    {
        foreach(var entry in Items.Values) entry.SetDefault();
    }
}