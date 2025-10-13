using Godot;
using System;

public partial class InteractableLamp : InteractableObject
{
    [Export] public OmniLight3D Light;
    [Export] public float LightEnergyOn = 0.6f;
    public override void Interact()
    {
        if (Light == null) return;
        if (Light.LightEnergy > 0)
        {
            Light.LightEnergy = 0;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOff);
        }
        else
        {
            Light.LightEnergy = LightEnergyOn;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOn);
        }
    }
}
