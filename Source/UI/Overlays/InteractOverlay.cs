using Godot;
using System;

public partial class InteractOverlay : VBoxContainer
{
    public Label actionNameLabel;
    public override void _Ready()
    {
        actionNameLabel = GetNode<Label>("ActionNameLabel");
        GD.Print("InteractOverlay ready");
        Visible = false;
        Position = (DisplayServer.WindowGetSize()-Size)/2;
        SignalManager.Instance.ChangeInteractableText += UpdateText;
        SignalManager.Instance.ToggleInteractableOverlay += ToggleOverlay;
    }

    public void UpdateText(string text)
    {
        actionNameLabel.Text = text;
    }

    public void ToggleOverlay()
    {
        Visible = !Visible;
    }
}
