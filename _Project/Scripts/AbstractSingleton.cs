using Godot;

public partial class AbstractSingleton<T> : Node where T : class
{
    public static T Instance { get; protected set; }
    public static bool HasInstance => Instance != null;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (Instance == null) Instance = this as T;
        else QueueFree();
    }

    public override void _ExitTree()
    {
        Instance = null;
        base._ExitTree();
    }
}