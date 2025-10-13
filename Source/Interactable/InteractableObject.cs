using Godot;
using System;

public partial class InteractableObject : Node3D
{
    [Export] public InteractOverlay Overlay;
    private bool inInteractionRange = false;

    public override void _Ready()
    {
       
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("Interact") && inInteractionRange)
        {
            Interact();
            GD.Print("Interaction");
        }
    }

    public  void Interact() //abstract
    {
        //Send specific signal
    }
    public void _on_body_entered(Node3D body)
    {
        if (body is NovakPlayer)
        {
            Overlay.Visible = true;
            inInteractionRange = true;
        }
       
    }

    public void _on_body_exited(Node3D body)
    {
        if (body is NovakPlayer)
        {
            Overlay.Visible = false;
            inInteractionRange = false;
        }
        
    }
}
