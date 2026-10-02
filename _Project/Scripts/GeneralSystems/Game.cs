using Godot;

public partial class Game : AbstractSingleton<Game>
{
    [Export] private UIController _uiController;
    
    [ExportGroup("Databases")]
    [Export] private LevelDatabase _levelDatabase;
    [Export] private PlayerDatabase _playerDatabase;
    
    [ExportGroup("Data controllers")]
    [Export] private SessionDataController _sessionDataController;
    [Export] private SaveDataController _saveDataController;
    

    public override void _Ready()
    {
        // initializing databases
        _levelDatabase.Init();
        _playerDatabase.Init();
        
        // Initializing data controllers (resources)
        _sessionDataController.Init();
        _saveDataController.Init();
        
        _uiController.Init();
        
        SceneController.LoadMainMenu();
    }
}