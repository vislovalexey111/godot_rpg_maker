using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class SessionDataController : DataController<SessionData>
{
    [Export] public PlayerDataController[] Players;

    private Dictionary<string, PlayerDataController> _players;
    
    public PlayerDataController CurrentPlayer => _players[Data.CurrentPlayerId];
    
    public override void Init()
    {
        base.Init();
        
        _players = new Dictionary<string, PlayerDataController>(Players.Length);
        var playerDataDictionary = new Dictionary<string, PlayerData>(Players.Length);
        
        foreach (var player in Players)
        {
            player.Init();
            string playerId = player.ResourceName;
            _players.Add(playerId, player);
            playerDataDictionary.Add(playerId, player.Data);
        }
        
        // First player will be a default starting player
        Data = new(
            DateTime.Now,
            Players[0].ResourceName,
            playerDataDictionary
        );
    }

    public void Load(SessionData data)
    {
        Data.Set(data);
    }

    public override void SetDefault()
    {
        foreach (var player in Players) player.SetDefault();

        Data.CurrentPlayerId = Players[0].ResourceName;
        Data.LastUpdate = DateTime.Now;
    }
}