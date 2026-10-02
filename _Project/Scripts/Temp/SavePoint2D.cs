using Godot;

public partial class SavePoint2D : Area2D
{
    [Export] private SpawnDataController _spawnDataController;
    [Export] private Node2D _spawnPoint;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        AreaEntered += OnAreaEnter;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEnter;
        base._ExitTree();
    }

    private void OnAreaEnter(Area2D area)
    {
        if (area.Name != "PlayerArea") return;

        _spawnDataController.SetCurrentSpawn(_spawnPoint.Name);
        UIController.ShowScreen("Save", "HUD");
    }
}
