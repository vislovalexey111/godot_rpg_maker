using System;
using System.Collections.Generic;

[Serializable]
public class SessionData : ILoadableData<SessionData>
{
    public Dictionary<string, PlayerData> Players { get; set; }
    public DateTime LastUpdate { get; set; }
    public string CurrentPlayerId { get; set; }

    public void Set(SessionData sessionData)
    {
        LastUpdate = sessionData.LastUpdate;
        CurrentPlayerId = sessionData.CurrentPlayerId;

        foreach (var playerId in sessionData.Players.Keys)
        {
            if (Players.ContainsKey(playerId)) Players[playerId].Set(sessionData.Players[playerId]);
        }
    }
    
    public SessionData(DateTime lastUpdate, string currentPlayerId, Dictionary<string, PlayerData> players)
    {
        LastUpdate = lastUpdate;
        CurrentPlayerId = currentPlayerId;
        Players = players;
    }
}