using Godot;
using System;

[GlobalClass]
public partial class ReplaceSceneWithCutsceneButton : ButtonUI
{
    
    [Export] private Cutscene _cutscene;
    [Export(PropertyHint.File, "*.tscn")]
    
    public string ScenePath { get; set; }
    public override async void OnPressed()
    {
        if (_cutscene != null)
        {
           await _cutscene.RunSequence();
        }
        base.OnPressed();
        if (!string.IsNullOrEmpty(ScenePath))
        {
            SceneManager.Instance.ReplaceScene(ScenePath);
        }
    }
}