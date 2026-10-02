using Godot;

[GlobalClass]
public partial class LevelDataController : DataController<LevelData>
{
    [Export] public string LevelName;
    [Export(PropertyHint.File, "*.tscn")] public string ScenePath;
    
    public override void Init()
    {
        base.Init();
        Data = new LevelData();
    }

    public override void SetDefault()
    {
        
    }
}