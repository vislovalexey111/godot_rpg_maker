using Godot;

[GlobalClass]
public partial class LevelDatabase : ControllerDatabase<LevelDataController, LevelData>
{
    [Export] private LevelDataController[] _levels;

    public void Init() => Init(_levels);
}