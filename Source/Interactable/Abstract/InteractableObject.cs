using Godot;
using System;

public abstract partial class Interactable: Area3D
{
    [Export] public String OverlayString;
    [Export] public InteractOverlay Overlay;
    [Export] public MeshInstance3D[] OutlinedMeshes;
    private bool inInteractionRange = false;

    public override void _Ready()
    {
       
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("Interact") && inInteractionRange)
        {
            Interact();
        }
    }

    public abstract void Interact(); //abstract
   
    public void _on_body_entered(Node3D body)
    {
        if (body is NovakPlayer)
        {
            Overlay.Visible = true;
            Overlay.actionNameLabel.Text = OverlayString;
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
            Overlay.Visible = false;
            inInteractionRange = false;
            foreach (var OutMesh in OutlinedMeshes)
            {
                StandardMaterial3D mat = OutMesh.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;
                mat.StencilMode = BaseMaterial3D.StencilModeEnum.Disabled;
            }
        }
        
    }
}
