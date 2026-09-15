using Godot;

public partial class PlayerSpawner2D : Node
{
    [Export] private Node _spawnPointRoot;
    [Export] private PlayerLink _playerLink;

    public void SpawnPlayer()
    {
        var nodes =  _spawnPointRoot.GetChildren();
        var currentSpawnPoint = _playerLink.Data.CurrentSpawnId;
        
        bool found = false;
        
        foreach (var node in nodes)
        {
            if (node.Name != currentSpawnPoint || node is not Node2D point) continue;

            found = true;
            SpawnPlayerAtPoint(point);
            break;
        }

        if (!found) GD.PrintErr("Spawn point not found");
    }

    private void SpawnPlayerAtPoint(Node2D point)
    {
        var playerPath = _playerLink.GetPlayerPrefabPath();

        if (!ResourceLoader.Exists(playerPath))
        {
            GD.PrintErr("Player resource does not exist");
            return;
        }
            
        var playerResource = ResourceLoader.Load<PackedScene>(_playerLink.GetPlayerPrefabPath());

        if (playerResource == null)
        {
            GD.PrintErr("Failed to load resource");
            return;
        };
            
        var player2D = playerResource.Instantiate<Player2D>();
        AddChild(player2D);
        player2D.GlobalPosition = point.GlobalPosition;
    }
}
