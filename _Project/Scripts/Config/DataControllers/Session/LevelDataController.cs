using Godot;

[GlobalClass]
public partial class LevelDataController : DataController<LevelData>
{
    [Export] public string LevelName;
    
    public override void Init()
    {
        base.Init();
        Data = new LevelData();
    }

    public override void SetDefault()
    {
        
    }
}