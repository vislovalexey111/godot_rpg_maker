using Godot;

[GlobalClass]
public partial class UIScreen : Control
{
    public override void _Ready()
    {
        SetProcessInput(false);
        base._Ready();
        VisibilityChanged += OnVisibilityChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        VisibilityChanged -= OnVisibilityChanged;
    }

    public virtual void Init() {}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
        
        //if (Input.IsActionJustReleased("gameui_back")) GoBack();
        //if (@event.IsActionReleased("gameui_back")) GoBack();
        /*if (@event is InputEventShortcut shortcutEvent)
            GetViewport().SetInputAsHandled();*/
    }

    private void OnVisibilityChanged()
    {
        if (Visible) OnShown();
        else OnHidden();
    }

    protected virtual void OnHidden()
    {
        SetProcessInput(false);
        GD.Print($"{Name} is hidden");
    }

    protected virtual void OnShown()
    {
        SetProcessInput(true);
        GD.Print($"{Name} is shown");
        GetTree().Paused = true;
    }

    public virtual void GoBack() {}
}
