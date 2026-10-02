using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UIController : AbstractSingleton<UIController>
{
    [Export] private Node _screenRoot;

    private Dictionary<string, UIScreen> _screens;
    
    private string _currentScreenId;

    public void Init()
    {
        _screens = new Dictionary<string, UIScreen>();
        var screens = _screenRoot.GetChildren();
        foreach (var screen in screens)
        {
            var uiScreen = screen as UIScreen;
            
            if (uiScreen == null) continue;
            
            uiScreen.Init();
            _screens.Add(screen.Name, uiScreen);
        }
    }
    
    public static void ShowScreen(string newScreenId, string backScreenId = null)
    {
        if (HasInstance) Instance.ShowSpecificScreen(newScreenId, backScreenId);
    }
    
    private void ShowSpecificScreen(string newScreenId, string backScreenId)
    {
        GD.Print($"NEWSCREEN IS: {newScreenId}, BACKSCREEN IS: {backScreenId}");
        
        if (newScreenId == _currentScreenId || !_screens.TryGetValue(newScreenId, out var newScreen))
            return;
        
        // Checking if we have active screen and (if there is) disabling it first
        if (_screens.TryGetValue(_currentScreenId, out var currentScreen))
        {
            currentScreen.Hide();
            
            if (!string.IsNullOrWhiteSpace(backScreenId) && newScreen is IScreenScreenBack backScreenDynamic)
                backScreenDynamic.BackScreenId = backScreenId;
        }
        
        _currentScreenId = newScreenId;
        newScreen.Show();
    }
}
