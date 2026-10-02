using Godot;

// TODO: Make hud to display various hud elements 
public partial class UIHud : UIScreen
{
    [ExportGroup("Buttons")]
    [Export] private Button _btnPause;

    public override void _Ready()
    {
        base._Ready();
        _btnPause.Pressed += OnPause;
    }

    public override void _ExitTree()
    {
        _btnPause.Pressed -= OnPause;
        base._ExitTree();
    }

    private void OnPause() => UIController.ShowScreen("Pause");

    protected override void OnShown() => GetTree().Paused = false;
}