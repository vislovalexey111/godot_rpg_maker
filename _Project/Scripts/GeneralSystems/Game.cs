using Godot;

public partial class Game : AbstractSingleton<Game>
{
    [Export] private SessionDataController _sessionDataController;
    [Export] private SaveDataController _saveDataController;

    public override void _Ready()
    {
        _sessionDataController.Init();
        _saveDataController.Init(_sessionDataController);
    }
}