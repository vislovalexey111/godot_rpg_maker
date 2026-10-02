using Godot;

public partial class LevelGameplay : Node
{
    public override void _Ready()
    {
        base._Ready();
        UIController.ShowScreen("HUD");
    }
}