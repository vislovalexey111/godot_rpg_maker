using Godot;

[GlobalClass]
public partial class PlayerLink : Resource
{
    [Export] private string _playerPrefabFolder = "res://_Project/Prefabs/Players/";

    public PlayerDataController Controller;
    public PlayerData Data => Controller.Data;

    public string GetPlayerPrefabPath() => $"{_playerPrefabFolder}{Controller.ResourceName}.tscn";
}