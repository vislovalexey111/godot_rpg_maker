using System;
using Godot;

[GlobalClass]
public partial class SaveEntryButton : Button
{
    [Export] private Label _lblDate;
    [Export] private Label _lblPath;
    
    public SaveDataEntry SaveDataEntry { get; private set; }
    
    private Action<int> _pressCallback;
    
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPress;
    }

    public override void _ExitTree()
    {
        Unsubscribe();
        Pressed -= OnPress;
        base._ExitTree();
    }

    private void OnPress() => _pressCallback?.Invoke(GetIndex());

    public void Subscribe(SaveDataEntry dataEntry,  Action<int> pressCallback)
    {
        _lblDate.Text = dataEntry.LastUpdate.ToString("yyyy MM dd - HH:mm:ss");
        _lblPath.Text = dataEntry.FilePath;
        SaveDataEntry = dataEntry;
        _pressCallback = pressCallback;
    }

    private void Unsubscribe()
    {
        _lblDate.Text = null;
        _lblPath.Text = null;
        SaveDataEntry = null;
        _pressCallback = null;
    }
}