using Godot;

public partial class LevelChanger2D : Area2D
{
    [Export] private SpawnDataController _spawnDataController;
    [Export] private string _nextSpawnId;
    [Export] private LevelDataController _nextLevelDataController;

    private const string PLAYER_AREA = "PlayerArea";

    public override void _Ready()
    {
        base._Ready();
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
        base._ExitTree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.Name != PLAYER_AREA || _spawnDataController.Player == null) return;
        
        _spawnDataController.SetCurrentLevelSpawn(_nextLevelDataController, _nextSpawnId);
        SceneController.LoadLevel(_nextLevelDataController);
    }
}