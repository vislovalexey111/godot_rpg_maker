using Godot;

public partial class UISave : UISaveEntryScreen
{
    [Export] private Button _btnNewSave;

    public override void _Ready()
    {
        base._Ready();
        _btnNewSave.Pressed += OnNewSave;
    }

    public override void _ExitTree()
    {
        _btnNewSave.Pressed -= OnNewSave;
        base._ExitTree();
    }

    protected override void GrabFirstButton() => SetCurrentButton(-1);
    private void OnNewSave() => SelectAction(-1);

    protected override void SelectAction(int buttonIndex)
    {
        base.SelectAction(buttonIndex);
        
        if (!saveDataController.TrySaveSession(currentButtonIndex, out var entry) || currentButtonIndex >= 0) return;
        
        var button = buttonPrefab.Instantiate<SaveEntryButton>();
        button.Subscribe(entry, SelectAction);
        buttons.Add(button);
        buttonRoot.AddChild(button);
    }

    protected override void SetCurrentButton(int buttonIndex)
    {
        base.SetCurrentButton(buttonIndex);
        if (buttonIndex < 0) _btnNewSave.GrabFocus();
    }

    protected override void AfterDelete()
    {
        if (buttons.Count < 1) SetCurrentButton(-1);
        else SetCurrentButton(currentButtonIndex < 0 ? -1 : (currentButtonIndex  - 1));
    }
    
    protected override void SetNext()
    {
        SetCurrentButton(buttons is {Count: > 0} && currentButtonIndex < (buttons.Count - 1)
            ? (currentButtonIndex + 1)
            : -1
        );
    }

    protected override void SetPrevious()
    {
        if (buttons is not {Count: > 0}) SetCurrentButton(-1);
        
        SetCurrentButton(currentButtonIndex < 0
            ? (buttons.Count - 1)
            : (currentButtonIndex - 1) % buttons.Count
        );
    }
}
