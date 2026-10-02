using Godot;

public partial class UIMainMenu : UIScreen
{
    [Export] private SessionDataController _sessionDataController;
    
    [ExportGroup("Buttons")]
    [Export] private Button _btnStart;
    [Export] private Button _btnLoad;
    [Export] private Button _btnQuit;

    public override void _Ready()
    {
        base._Ready();
        _btnStart.Pressed += OnStartPressed;
        _btnLoad.Pressed += OnLoadPressed;
        _btnQuit.Pressed += OnQuitPressed;
    }

    public override void _ExitTree()
    {
        _btnStart.Pressed -= OnStartPressed;
        _btnLoad.Pressed -= OnLoadPressed;
        _btnQuit.Pressed -= OnQuitPressed;
        base._ExitTree();
    }

    protected override void OnShown()
    {
        base.OnShown();
        ShowButtons(true);
        _btnStart.GrabFocus();
    }

    private void ShowButtons(bool show)
    {
        _btnStart.Visible = show;
        _btnLoad.Visible = show;
        _btnQuit.Visible = show;
    }

    private void OnStartPressed()
    {
        ShowButtons(false);
        GD.Print("START WAS PRESSED");
        _sessionDataController.SetDefault();
        SceneController.LoadCurrentLevel();
    }

    private void OnLoadPressed()
    {
        ShowButtons(false);
        UIController.ShowScreen("Load", "MainMenu");
    }

    private void OnQuitPressed()
    {
        ShowButtons(false);
        GetTree().Quit();
    }
}
