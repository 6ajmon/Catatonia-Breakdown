using Godot;
using System;

[GlobalClass]
public partial class ReplaceSceneButton : ButtonUI
{
    [Export(PropertyHint.File, "*.tscn")]
    public string ScenePath { get; set; }
    public override void OnPressed()
    {
        GD.Print("Play pressed");
        base.OnPressed();
        if (!string.IsNullOrEmpty(ScenePath))
        {
            SceneManager.Instance.ReplaceScene(ScenePath);
        }
    }
}