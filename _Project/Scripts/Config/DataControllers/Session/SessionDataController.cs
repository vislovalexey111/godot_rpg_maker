using System;
using Godot;

[GlobalClass]
public partial class SessionDataController : DataController<SessionData>
{
    [Export] public PlayerDatabase _players;
    [Export] public LevelDatabase _levels;
    [Export] public PlayerDataController _defaultPlayer;
    [Export] public PlayerLink _playerLink;

    public void SetCurrentPlayer(PlayerDataController controller)
    {
        Data.CurrentPlayerId = controller.ResourceName;
        _playerLink.Controller = controller;
    }
    
    public override void Init()
    {
        _playerLink.Controller = _defaultPlayer;
        
        Data = new SessionData(
            DateTime.Now,
            _defaultPlayer.ResourceName,
            _players.CreateDataDictionary(),
            _levels.CreateDataDictionary()
        );
    }

    public override void SetDefault()
    {
        _players.SetDefault();
        _levels.SetDefault();

        _playerLink.Controller = _defaultPlayer;
        Data.CurrentPlayerId = _defaultPlayer.ResourceName;
        Data.LastUpdate = DateTime.Now;
    }
    
    public void Load(SessionData data)
    {
        Data.Set(data);

        if (_players.TryGetItem(Data.CurrentPlayerId, out PlayerDataController player))
            _playerLink.Controller = player;
        else
            GD.PrintErr("No data controller found");
    } 
}