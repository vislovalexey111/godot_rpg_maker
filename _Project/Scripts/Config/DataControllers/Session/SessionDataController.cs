using System;
using Godot;

[GlobalClass]
public partial class SessionDataController : DataController<SessionData>
{
    [Export] public PlayerDatabase _playerDatabase;
    [Export] public LevelDatabase _levelDatabase;
    [Export] private SpawnDataController _spawnDataController;

    
    public override void Init()
    {
        _spawnDataController.Init();
        
        Data = new SessionData(
            DateTime.Now,
            _spawnDataController.Data,
            _playerDatabase.CreateDataDictionary(),
            _levelDatabase.CreateDataDictionary()
        );
    }

    public override void SetDefault()
    {
        _playerDatabase.SetDefault();
        _levelDatabase.SetDefault();
        _spawnDataController.SetDefault();

        Data.LastUpdate = DateTime.Now;
    }
    
    public void Load(SessionData data)
    {
        Data.Set(data);

        if (_playerDatabase.TryGetItem(Data.SpawnData.CurrentPlayerId, out PlayerDataController player))
            _spawnDataController.Player = player;
        else GD.PrintErr("No level data controller found");
    } 
}