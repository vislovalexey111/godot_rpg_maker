using Godot;

public partial class Game : AbstractSingleton<Game>
{
    [ExportGroup("Databases")]
    [Export] private LevelDatabase _levelDatabase;
    [Export] private PlayerDatabase _playerDatabase;
    
    [ExportGroup("Data controllers")]
    [Export] private SessionDataController _sessionDataController;
    [Export] private SaveDataController _saveDataController;

    public override void _Ready()
    {
        // initializing databases
        _playerDatabase.Init();
        _levelDatabase.Init();
        
        // Initializing data controllers (resources)
        _sessionDataController.Init();
        _saveDataController.Init();
        
        SceneController.LoadMainMenu();
    }
}