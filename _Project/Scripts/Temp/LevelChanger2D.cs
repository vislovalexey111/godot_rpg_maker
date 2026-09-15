using Godot;

public partial class LevelChanger2D : Area2D
{
    [Export] private PlayerLink _playerLink;
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
        if (area.Name != PLAYER_AREA || _playerLink.Controller == null) return;
        
        var data = _playerLink.Data;
        var nextLevel = _nextLevelDataController.ResourceName;
        data.CurrentSpawnId = _nextSpawnId;
        data.CurrentLevelId = nextLevel;
        
        GD.Print($"Triggered! Loading {nextLevel}");
        SceneController.LoadCurrentLevel();
    }
}