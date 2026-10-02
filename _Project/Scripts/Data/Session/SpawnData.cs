public class SpawnData : ILoadableData<SpawnData>
{
    public string CurrentPlayerId { get; set; }
    
    public void Set(SpawnData data)
    {
        CurrentPlayerId = data.CurrentPlayerId;
    }

    public SpawnData(string currentPlayerId)
    {
        CurrentPlayerId = currentPlayerId;
    }
}