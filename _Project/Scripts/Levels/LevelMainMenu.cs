using Godot;

public partial class LevelMainMenu : Node
{
    [Export] private float _timeout = 10f;

    private double _timer;

    public override void _Ready()
    {
        base._Ready();
        _timer = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_timer < _timeout) _timer += delta;
        else
        {
            SetProcess(false);
            SceneController.LoadCurrentLevel();
        }
    }
}
