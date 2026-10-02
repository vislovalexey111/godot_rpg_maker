using Godot;

[GlobalClass]
public partial class PlayerDataController : DataController<PlayerData>
{
    [ExportGroup("Config")]
    [Export(PropertyHint.File, "*.tscn")] public string PrefabPath;
    [Export] public string PlayerName;
    [Export] public int MaxHealth = 100;
    
    [ExportGroup("Defaults")]
    [Export] private int _defaultHealth = 100;
    [Export] private LevelDataController _defaultLevel;
    [Export] private string _defaultSpawnId;
    
    public override void SetDefault()
    {
        Data.Health = _defaultHealth;
        Data.CurrentLevelId = _defaultLevel.ResourceName;
        Data.CurrentSpawnId = _defaultSpawnId;
        GD.Print("Now spawn is: " + Data.CurrentSpawnId);
    }

    public override void Init()
    {
        base.Init();

        Data = new PlayerData(
            _defaultHealth,
            _defaultLevel.ResourceName,
            _defaultSpawnId
        );
    }

    public void SetLevelSpawn(string levelId, string spawnId)
    {
        Data.CurrentLevelId = levelId;
        Data.CurrentSpawnId = spawnId;
    }
    
    public void SetSpawn(string spawnId) => Data.CurrentSpawnId = spawnId;
}