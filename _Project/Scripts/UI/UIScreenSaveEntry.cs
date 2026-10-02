using System.Collections.Generic;
using Godot;

public partial class UISaveEntryScreen : UIScreen, IScreenScreenBack
{
    [Export] protected SaveDataController saveDataController;
    [Export] protected Control buttonRoot;
    [Export] protected PackedScene buttonPrefab;

    [Export] private Button _btnBack;

    protected List<SaveEntryButton> buttons;
    protected int currentButtonIndex;
    
    public string BackScreenId { get; set; }

    public override void _Ready()
    {
        base._Ready();
        _btnBack.Pressed += GoBack;
    }

    public override void Init()
    {
        currentButtonIndex = -1;
        buttons = new List<SaveEntryButton>();
    }

    public override void _ExitTree()
    {
        RemoveButtons();
        _btnBack.Pressed -= GoBack;
        base._ExitTree();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("gameui_down"))
            SetNext();
        else if (@event.IsActionReleased("gameui_up"))
            SetPrevious();
        else if (@event.IsActionReleased("gameui_delete"))
            DeleteAction();
        
        base._Input(@event);
        
    }

    protected virtual void SetNext()
    {
        SetCurrentButton(buttons is {Count: > 0}
            ? ((currentButtonIndex + 1) % buttons.Count)
            : -1
        );
    }

    protected virtual void SetPrevious()
    {
        SetCurrentButton(buttons is {Count: > 0}
            ? ((currentButtonIndex - 1 + buttons.Count) % buttons.Count)
            : -1
        );
    }

    protected override void OnShown()
    {
        base.OnShown();
        RedrawButtons();
    }

    protected void RedrawButtons()
    {
        if (buttons is { Count: > 0 }) RemoveButtons();
        
        // Showing new buttons
        var saveCount = saveDataController.SaveCount;
        
        if (saveCount > 0)
        {
            buttons.EnsureCapacity(saveCount);
            
            GD.Print("Entry count: " + saveCount);
            
            foreach (var entry in saveDataController.SaveDatabase.Entries)
            {
                var button = buttonPrefab.Instantiate<SaveEntryButton>();
                
                if (button == null) continue;
                
                button.Subscribe(entry, SelectAction);
                buttons.Add(button);
                buttonRoot.AddChild(button);
            }
        }
        
        GrabFirstButton();
    }
    
    private void RemoveButtons()
    {
        foreach (var button in buttons)
        {
            if (button != null) button.QueueFree();
        }

        currentButtonIndex = -1;
        buttons.Clear();
    }


    protected virtual void GrabFirstButton()
    {
        SetCurrentButton(buttons is {Count: > 0} ? 0 : -1);
    }
    

    // Override this function in children for custom action on button press
    protected virtual void SelectAction(int buttonIndex) => SetCurrentButton(buttonIndex);

    protected virtual void SetCurrentButton(int buttonIndex)
    {
        currentButtonIndex = buttonIndex < 0 ? -1 : buttonIndex;
        if (buttonIndex >= 0) buttons[buttonIndex].GrabFocus();
    }

    private void DeleteAction()
    {
        if (currentButtonIndex == -1) return;

        var currentButton =  buttons[currentButtonIndex];
        
        saveDataController.RemoveSession(currentButton.SaveDataEntry);
        buttons.Remove(currentButton);
        currentButton.QueueFree();
        AfterDelete();
    }

    // Override this function in children for custom button navigation handling
    protected virtual void AfterDelete()
    {
        if (buttons.Count < 1) SetCurrentButton(-1);
        else SetCurrentButton(currentButtonIndex < 1 ? 0 : (currentButtonIndex  - 1));
    }
    
    public override void GoBack() => UIController.ShowScreen(BackScreenId);
}