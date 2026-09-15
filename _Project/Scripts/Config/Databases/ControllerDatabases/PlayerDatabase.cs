using Godot;

[GlobalClass]
public partial class PlayerDatabase : ControllerDatabase<PlayerDataController, PlayerData>
{
    [Export] private PlayerDataController[] _players;
    public void Init() => Init(_players);
}