using Godot;
using System;

public partial class Container : VBoxContainer
{
    public override void _Ready()
    {
        Position = (DisplayServer.WindowGetSize()-Size)/2;
        GD.Print(Position);
    }
}
