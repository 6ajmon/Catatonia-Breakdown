using Godot;
using System;

public partial class InteractableLamp : InteractableObject
{
    [Export] public OmniLight3D Light;
    [Export] public float LightEnergyOn = 0.6f;
    [Export] public MeshInstance3D ShiningMesh;
    
    private StandardMaterial3D shiningMaterial;
    private Color originalAlbedo = Colors.White;
    [Export] private Color brightAlbedo = new Color(5.0f, 5.0f, 5.0f);
    
    public override void _Ready()
    {
        base._Ready();

        if (ShiningMesh != null && ShiningMesh.Mesh != null)
        {
            var baseMat = ShiningMesh.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;  

            if (baseMat != null)
            {
                shiningMaterial = baseMat.Duplicate() as StandardMaterial3D;  
                ShiningMesh.SetSurfaceOverrideMaterial(0, shiningMaterial); 
                originalAlbedo = shiningMaterial.AlbedoColor;
            }
        }
        if (Light != null && Light.LightEnergy > 0)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOn);
            if (shiningMaterial != null)
            {
                shiningMaterial.AlbedoColor = brightAlbedo;
            }
        }
        else
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOff);
            if (shiningMaterial != null)
            {
                shiningMaterial.AlbedoColor = originalAlbedo;
            }
        }
    }
    
    public override void Interact()
    {
        if (Light == null) return;
        if (Light.LightEnergy > 0)
        {
            Light.LightEnergy = 0;
            if (shiningMaterial != null)
            {
                shiningMaterial.AlbedoColor = originalAlbedo;
            }
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOff);
        }
        else
        {
            Light.LightEnergy = LightEnergyOn;
            if (shiningMaterial != null)
            {
                shiningMaterial.AlbedoColor = brightAlbedo;
            }
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.LampOn);
        }
    }
}
