using Godot;

public abstract partial class DataController<T> : Godot.Resource
{
    public T Data { get; protected set; }
    public abstract void SetDefault();

    public virtual void Init()
    {
        ResourceName = ResourcePath.GetFile().GetBaseName();
    }
}