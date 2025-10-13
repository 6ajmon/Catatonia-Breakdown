using Godot;
using System;

public partial class InteractableObject : Node3D
{
    [Export] public String ObjectId;
    private Container uiContainer;
    private bool inInteractionRange = false;

    public override void _Ready()
    {
        uiContainer = (Container)GetNode("UIContainer");
        uiContainer.Visible = false;
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
        uiContainer.Visible = true;
        inInteractionRange = true;
    }

    public void _on_body_exited(Node3D body)
    {
        uiContainer.Visible = false;
        inInteractionRange = false;
    }
}
