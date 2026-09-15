using Godot;

[GlobalClass]
public partial class PlayerDataController : DataController<PlayerData>
{
    [ExportGroup("Config")]
    [Export] public string PlayerName;
    [Export] public int MaxHealth = 100;
    
    [ExportGroup("Defaults")]
    [Export] private int _defaultHealth = 100;
    [Export] private string _defaultLevelId;
    [Export] private string _defaultSpawnId;
    
    public override void SetDefault()
    {
        Data.Health = _defaultHealth;
        Data.CurrentLevelId = _defaultLevelId;
        Data.CurrentSpawnId = _defaultSpawnId;
    }

    public override void Init()
    {
        base.Init();
        
        Data = new PlayerData(
            _defaultHealth,
            _defaultLevelId,
            _defaultSpawnId
        );
    }
}