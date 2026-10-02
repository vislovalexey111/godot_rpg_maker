using Godot;

public partial class UILoad : UISaveEntryScreen
{
    protected override void SelectAction(int buttonIndex)
    {
        base.SelectAction(buttonIndex);

        if (currentButtonIndex < 0) return;
        
        UIController.ShowScreen("Cutscene");
        
        if (saveDataController.TryLoadSession(currentButtonIndex))
            SceneController.LoadCurrentLevel();
        else
            RedrawButtons();
    }
}
