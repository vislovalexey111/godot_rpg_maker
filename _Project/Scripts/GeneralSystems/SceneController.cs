using Godot;

[GlobalClass]
public partial class SceneController : AbstractSingleton<SceneController>
{
    private enum ELoadingProgress : byte
    {
        None = 0,
        LoadingStarted,
        Loading
    }
    
    [ExportGroup("Config")]
    [Export] private string _levelsPath = "res://_Project/Scenes/Levels/GameplayLevels/";
    [Export] private string _levelsExtension = ".tscn";
    [Export] private string _mainMenuScene = "res://_Project/Scenes/Levels/_MainMenu.tscn";

    [ExportGroup("References")]
    [Export] private PlayerLink _playerLink;
    [Export] private Fader _fader;
    [Export] private Node _levelRoot;
    
    private ELoadingProgress _loadingProgress;
    private string _currentPath;
    private string _lastPath;
    private Node _currentScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        _loadingProgress = ELoadingProgress.None;
        SetProcess(false);
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_loadingProgress != ELoadingProgress.Loading) return;
        
        var status = ResourceLoader.LoadThreadedGetStatus(_currentPath);
        GD.Print("Loading Status is: " + status);
        
        if (status == ResourceLoader.ThreadLoadStatus.InProgress) return;
        
        if (status == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            if (ResourceLoader.LoadThreadedGet(_currentPath) is not PackedScene levelResource)
            {
                GD.PrintErr("Invalid resource conversion: " + _currentPath);
                _currentPath = _lastPath;
            }
            else
            {
                if (_currentScene != null) _currentScene.Free();
                
                _currentScene = levelResource.Instantiate();
                _levelRoot.AddChild(_currentScene);
            }
        }
        else
        {
            if (status == ResourceLoader.ThreadLoadStatus.Failed)
                GD.PrintErr("Failed to load level: " + _currentPath);
            else if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
                GD.PrintErr("Invalid resource: " + _currentPath);

            _currentPath = _lastPath;
        }
        
        _loadingProgress = ELoadingProgress.None;
        _fader.FadeOut();
        SetProcess(false);
    }

    public static void LoadCurrentLevel()
    {
        if (HasInstance) Instance.LoadCurrent();
    }

    public static void LoadMainMenu()
    {
        if (HasInstance) Instance.LoadMenu();
    }

    public void LoadCurrent()
    {
        LoadScene($"{_levelsPath}{_playerLink.Data.CurrentLevelId}{_levelsExtension}");
    }
    
    public void LoadMenu() => LoadScene(_mainMenuScene);

    private void LoadScene(string scenePath)
    {
        if (_loadingProgress != ELoadingProgress.None)
        {
            GD.PrintErr("Loading scene error: Scene loading is in progress");
            return;
        }

        if (!ResourceLoader.Exists(scenePath))
        {
            GD.PrintErr("Loading scene error: Scene is not found: " + scenePath);
            return;
        }

        if (_currentPath == scenePath)
        {
            GD.Print("Loading scene warning: Scene is already loaded");
            return;
        }

        _loadingProgress = ELoadingProgress.LoadingStarted;
        _lastPath = _currentPath;
        _currentPath = scenePath;
        _fader.FadeIn(OnFadeInFinished);
    }

    private void OnFadeInFinished()
    {
        if (_loadingProgress != ELoadingProgress.LoadingStarted) return;

        _loadingProgress = ELoadingProgress.Loading;
        ResourceLoader.LoadThreadedRequest(_currentPath, "", true);
        SetProcess(true);
    }
}
