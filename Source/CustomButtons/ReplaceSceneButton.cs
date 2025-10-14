using Godot;
using System;

[GlobalClass]
public partial class ReplaceSceneButton : ButtonUI
{
    [Export] private Cutscene _cutscene;
    [Export(PropertyHint.File, "*.tscn")]
    
    public string ScenePath { get; set; }
    public override async void OnPressed()
    {
        if (_cutscene != null)
        {
            GD.Print("Run Sequence()");
           _cutscene.RunSequence();
           await _cutscene.WaitForSeconds(2.0f);
        }
        base.OnPressed();
        if (!string.IsNullOrEmpty(ScenePath))
        {
            GD.Print("Replace Scene");
            SceneManager.Instance.ReplaceScene(ScenePath);
        }
    }
}