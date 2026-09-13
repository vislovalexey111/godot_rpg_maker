using Godot;

public partial class TempSessionSaveButton : Node
{
    [Export] private Button _saveButton;
    [Export] private int _slot = 0;
    [Export] private SaveDataController _saveDataController;

    public override void _EnterTree()
    {
        base._EnterTree();
        _saveButton.Pressed += OnSavePressed;
    }

    public override void _ExitTree()
    {
        _saveButton.Pressed -= OnSavePressed;
        base._ExitTree();
    }

    private void OnSavePressed()
    {
        _saveDataController.SaveSession(_slot);
    }
}
