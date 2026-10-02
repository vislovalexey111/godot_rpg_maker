using Godot;

public partial class LevelMainMenu : Node
{
    public override void _Ready()
    {
        base._Ready();
        UIController.ShowScreen("MainMenu");
    }
}