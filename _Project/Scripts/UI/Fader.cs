using System;
using Godot;

[GlobalClass]
public partial class Fader : Node
{
    [Export] private AnimationPlayer _player;
    [Export] private StringName _animFadeIn = "FadeIn";
    [Export] private StringName _animFadeOut = "FadeOut";

    private Action _onFadeInFinished;

    public override void _Ready()
    {
        base._Ready();
        _player.AnimationFinished += OnAnimationFinished;
    }

    public override void _ExitTree()
    {
        _player.AnimationFinished -= OnAnimationFinished;
        _onFadeInFinished = null;
        base._ExitTree();
    }

    private void OnAnimationFinished(StringName animName)
    {
        _onFadeInFinished?.Invoke();
        _onFadeInFinished = null;
    }

    public void FadeIn(Action callback)
    {
        _player.Play(_animFadeIn);
        _onFadeInFinished = callback;
    }
    
    public void FadeOut() => _player.Play(_animFadeOut);
}