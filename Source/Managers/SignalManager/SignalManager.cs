using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class SignalManager : Node
{
    public static SignalManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SignalManager>("SignalManager");
    [Signal] public delegate void ChangeCameraEventHandler(int index);
    [Signal] public delegate void SwitchToFirstPersonCameraEventHandler();
    [Signal] public delegate void CutsceneEndedEventHandler();
    [Signal] public delegate void ChangeInteractableTextEventHandler(string text);
    [Signal] public delegate void ToggleInteractableOverlayEventHandler();
}
