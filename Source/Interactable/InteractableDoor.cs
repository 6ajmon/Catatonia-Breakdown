using Godot;
using System;

public partial class InteractableDoor : Interactable
{
    public override void Interact()
    {
        GD.Print("Doors interacted");
    }
}
