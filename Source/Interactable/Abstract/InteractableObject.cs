using Godot;
using System;

[GlobalClass] 
public partial class InteractableObject: Area3D
{
    [Export] public string OverlayString;
    [Export] public MeshInstance3D[] OutlinedMeshes;
    [Export] float OutlineThickness = 0.001f;
    public bool inInteractionRange = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
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
   
    public void OnBodyEntered(Node3D body)
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
                mat.StencilOutlineThickness = OutlineThickness;
            }
            
        }
       
    }

    public void OnBodyExited(Node3D body)
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
