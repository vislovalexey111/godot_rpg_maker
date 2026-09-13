[System.Serializable]
public class PlayerData : ILoadableData<PlayerData>
{
    public int Health { get; set; }
    
    public void Set(PlayerData Data)
    {
        Health = Data.Health;
    }
    
    public PlayerData(int health)
    {
        Health = health;
    }
}