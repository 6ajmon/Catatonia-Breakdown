using Godot;
using System;

public partial class InteractableLamp : InteractableObject
{
    public override void Interact()
    {
        var light = GetNode<OmniLight3D>("TableLamp/OmniLight3D");
        if (light.LightEnergy > 0)
        { 
            light.LightEnergy = 0;
            OverlayString = "Turn on";
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), OverlayString);
        }
        else
        {
            light.LightEnergy = 0.6f;
            OverlayString = "Turn off";
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), OverlayString);
        }
    }
}
