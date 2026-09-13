using Godot;

[GlobalClass]
public partial class PlayerDataController : DataController<PlayerData>
{
    [Export] public string PlayerName;
    [Export] public int Health = 100;
    [Export] public int MaxHealth = 100;
    
    public override void SetDefault()
    {
        Data.Health = Health;
    }

    public override void Init()
    {
        base.Init();
        
        Data = new(
            Health
        );
    }
}