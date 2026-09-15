[System.Serializable]
public class PlayerData : ILoadableData<PlayerData>
{
    public int Health { get; set; }
    public string CurrentLevelId { get; set; }
    public string CurrentSpawnId { get; set; }
    
    public void Set(PlayerData data)
    {
        Health = data.Health;
        CurrentLevelId = data.CurrentLevelId;
        CurrentSpawnId = data.CurrentSpawnId;
    }
    
    public PlayerData(int health, string currentLevelId, string currentSpawnId)
    {
        Health = health;
        CurrentLevelId = currentLevelId;
        CurrentSpawnId = currentSpawnId;
    }
}