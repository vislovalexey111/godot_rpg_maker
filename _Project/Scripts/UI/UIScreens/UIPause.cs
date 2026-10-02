using Godot;

public partial class UIPause : UIScreen
{
    [Export] private Button _btnResume;
    [Export] private Button _btnLoad;
    [Export] private Button _btnMenu;

    public override void _Ready()
    {
        base._Ready();
        _btnResume.Pressed += OnResumePressed;
        _btnLoad.Pressed += OnLoadPressed;
        _btnMenu.Pressed += OnMenuPressed;
    }

    public override void _ExitTree()
    {
        _btnResume.Pressed -= OnResumePressed;
        _btnLoad.Pressed -= OnLoadPressed;
        _btnMenu.Pressed -= OnMenuPressed;
        base._ExitTree();
    }

    protected override void OnShown()
    {
        base.OnShown();
        ShowButtons(true);
        _btnResume.GrabFocus();
    }

    private void ShowButtons(bool show)
    {
        _btnResume.Visible = show;
        _btnLoad.Visible = show;
        _btnMenu.Visible = show;
    }

    private void OnResumePressed()
    {
        ShowButtons(false);
        UIController.ShowScreen("HUD");
    }
    
    private void OnLoadPressed()
    {
        ShowButtons(false);
        UIController.ShowScreen("Load", "Pause");
    }

    private void OnMenuPressed()
    {
        ShowButtons(false);
        SceneController.LoadMainMenu();
    }

    public override void GoBack() => OnResumePressed();
}