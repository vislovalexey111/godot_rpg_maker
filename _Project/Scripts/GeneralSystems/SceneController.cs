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
 
    [Export(PropertyHint.File, "*.tscn")] private string _mainMenuScene;

    [ExportGroup("References")]
    [Export] private SpawnDataController _spawnDataController;
    [Export] private LevelDatabase _levelDatabase;
    [Export] private Node _levelRoot;
    [Export] private Fader _fader;
    
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
        
        _loadingProgress = ELoadingProgress.None;
        SetProcess(false);
        
        if (status == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            if (ResourceLoader.LoadThreadedGet(_currentPath) is not PackedScene levelResource)
            {
                GD.PrintErr("Invalid resource conversion: " + _currentPath);
                LoadMenu(false);
            }
            else
            {
                if (_currentScene != null) _currentScene.QueueFree();
                
                _currentScene = levelResource.Instantiate();
                _levelRoot.AddChild(_currentScene);
                _fader.FadeOut();
            }
        }
        else
        {
            if (status == ResourceLoader.ThreadLoadStatus.Failed)
                GD.PrintErr("Failed to load level: " + _currentPath);
            else if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
                GD.PrintErr("Invalid resource: " + _currentPath);

            LoadMenu(false);
        }
    }

    public static void LoadMainMenu()
    {
        if (HasInstance) Instance.LoadMenu();
    }

    public static void LoadLevel(string scenePath)
    {
        if (HasInstance) Instance.LoadScene(scenePath);
    }

    public static void LoadCurrentLevel()
    {
        if (HasInstance) Instance.LoadCurrent();
    }

    public static void LoadLevel(LevelDataController levelData)
    {
        if (HasInstance) Instance.LoadScene(levelData.ScenePath);
    }
    
    public void LoadCurrent()
    {
        if (_levelDatabase.TryGetItem(_spawnDataController.Player.Data.CurrentLevelId, out var currentLevel))
            LoadScene(currentLevel.ScenePath);
        else
            GD.PrintErr($"Failed to load level: no level with id {currentLevel.ScenePath} is specified in a database");
    }
    
    public void LoadMenu(bool queueLastScene = true) => LoadScene(_mainMenuScene, queueLastScene);

    private void LoadScene(string scenePath, bool queueLastScene = true)
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

        UIController.ShowScreen("Cutscene");
        _loadingProgress = ELoadingProgress.LoadingStarted;

        _lastPath = queueLastScene ? _currentPath : scenePath;
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
