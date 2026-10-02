using Godot;

[GlobalClass]
public partial class SpawnDataController : DataController<SpawnData>
{
    [Export] private PlayerDataController _defaultPlayer;
    
    public PlayerDataController Player;


    public void SetCurrentSpawn(string spawnId) => Player.SetSpawn(spawnId);
    public void SetCurrentLevelSpawn(LevelDataController level, string spawnId) => Player.SetLevelSpawn(level.ResourceName, spawnId);

    public void ChangeCurrentPlayer(PlayerDataController player)
    {
        Player = player;
        Data.CurrentPlayerId = player.ResourceName;
    }
    
    public void SetPlayerWithLevelSpawn(PlayerDataController player, LevelDataController level, string spawnId)
    {
        ChangeCurrentPlayer(player);
        SetCurrentLevelSpawn(level, spawnId);
    }

    public override void Init()
    {
        Data = new SpawnData(_defaultPlayer.ResourceName);
        Player = _defaultPlayer;
    }

    public override void SetDefault()
    {
        Data.CurrentPlayerId = _defaultPlayer.ResourceName;
        Player = _defaultPlayer;
    }
}