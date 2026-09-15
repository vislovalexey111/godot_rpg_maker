using Godot;

public partial class Level2D : Node
{
    [Export] private PlayerSpawner2D _playerSpawner2D;
    
    public override void _Ready()
    {
        base._Ready();
        _playerSpawner2D.SpawnPlayer();
    }
}
