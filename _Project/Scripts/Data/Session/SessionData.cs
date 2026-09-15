using System;
using System.Collections.Generic;

[Serializable]
public class SessionData : ILoadableData<SessionData>
{
    public Dictionary<string, PlayerData> Players { get; set; }
    public Dictionary<string, LevelData> Levels { get; set; }
    
    public DateTime LastUpdate { get; set; }
    public string CurrentPlayerId { get; set; }

    public void Set(SessionData data)
    {
        LastUpdate = data.LastUpdate;
        CurrentPlayerId = data.CurrentPlayerId;

        foreach (var playerId in data.Players.Keys)
        {
            if (Players.TryGetValue(playerId, out var player)) player.Set(data.Players[playerId]);
        }

        foreach (var levelId in data.Levels.Keys)
        {
            if (Levels.TryGetValue(levelId, out var level)) level.Set(data.Levels[levelId]);
        }
    }
    
    public SessionData(
        DateTime lastUpdate,
        string currentPlayerId,
        Dictionary<string, PlayerData> players,
        Dictionary<string, LevelData> levels)
    {
        LastUpdate = lastUpdate;
        CurrentPlayerId = currentPlayerId;
        Players = players;
        Levels = levels;
    }
}