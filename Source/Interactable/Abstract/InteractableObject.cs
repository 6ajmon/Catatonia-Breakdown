using Godot;
using System;
[GlobalClass] 
public partial class InteractableObject: Area3D
{
    [Export] public String OverlayString;
    [Export] public MeshInstance3D[] OutlinedMeshes;
    private bool inInteractionRange = false;

    public override void _Ready()
    {
       
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Interact") && inInteractionRange)
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        
    } 
   
    public void _on_body_entered(Node3D body)
    {
        if (body is NovakPlayer)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ToggleInteractableOverlay));
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), OverlayString);
            inInteractionRange = true;
            foreach (var OutMesh in OutlinedMeshes)
            {
                StandardMaterial3D mat = OutMesh.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;
                mat.StencilMode = BaseMaterial3D.StencilModeEnum.Outline;
                mat.StencilColor = Colors.White;
                mat.StencilOutlineThickness = 0.001f;
            }
            
        }
       
    }

    public void _on_body_exited(Node3D body)
    {
        if (body is NovakPlayer)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ToggleInteractableOverlay));
            inInteractionRange = false;
            foreach (var OutMesh in OutlinedMeshes)
            {
                StandardMaterial3D mat = OutMesh.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;
                mat.StencilMode = BaseMaterial3D.StencilModeEnum.Disabled;
            }
        }
        
    }
}
