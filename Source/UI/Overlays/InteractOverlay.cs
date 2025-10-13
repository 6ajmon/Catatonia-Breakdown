using Godot;
using System;

public partial class InteractOverlay : VBoxContainer
{
    public override void _Ready()
    {
        GD.Print("InteractOverlay ready");
        Visible = false;
        Position = (DisplayServer.WindowGetSize()-Size)/2;
    }
}
