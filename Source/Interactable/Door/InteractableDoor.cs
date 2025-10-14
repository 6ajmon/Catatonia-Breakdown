using Godot;
using System;

public partial class InteractableDoor : InteractableObject
{
    
    [Export(PropertyHint.File, "*.tscn")]
    public string ScenePath { get; set; }
    public override void Interact()
    {
        if (!string.IsNullOrEmpty(ScenePath))
        {
            GD.Print("Replace Scene");
            SceneManager.Instance.ReplaceScene(ScenePath);
        }
    }
}
