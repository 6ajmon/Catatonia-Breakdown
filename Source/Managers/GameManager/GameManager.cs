using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<GameManager>("GameManager");
    public NovakPlayer PlayerInstance;
    public bool IsCutscenePlaying = false;
    public override void _Ready()
    {
        SignalManager.Instance.CutsceneEnded += OnCutsceneEnded;
    }
    private void OnCutsceneEnded()
    {
        IsCutscenePlaying = false;
    }
}
