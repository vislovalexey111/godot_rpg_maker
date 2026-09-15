using Godot;

public partial class Player2D : Node2D
{
    [Export] private float _speed;
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        var dir = Input.GetVector( 
            "ui_left",
            "ui_right",
            "ui_up",
            "ui_down"
        );

        if (Vector2.Zero.IsEqualApprox(dir)) return;
        
        GlobalPosition += _speed * (float)delta * dir;
    }
}
