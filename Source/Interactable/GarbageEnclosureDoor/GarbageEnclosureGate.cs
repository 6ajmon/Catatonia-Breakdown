using Godot;
using System;

public partial class GarbageEnclosureGate : InteractableObject
{
    [Export] public AnimationPlayer AnimationPlayer;
    private bool closed = false;
    [Export] public bool startsClosed = true;

    public override void _Ready()
    {
        base._Ready();
        if (startsClosed)
        {
            closed = true;
        }
        else
        {
            AnimationPlayer?.Advance(0.3f);
            closed = false;
        }
    }
    
    public override void Interact()
    {
        if (AnimationPlayer == null) return;
        if (AnimationPlayer.IsPlaying()) return;
        
        if (closed)
        {
            AnimationPlayer.PlayBackwards("Close"); // Open
            AnimationPlayer.AnimationFinished += OnOpenAnimationFinished;
        }
        else
        {
            AnimationPlayer.Play("Close");
            AnimationPlayer.AnimationFinished += OnCloseAnimationFinished;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.GateClosed));
        }
    }
    
    private void OnCloseAnimationFinished(StringName animName)
    {
        if (animName == "Close")
        {
            closed = true;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.DoorOpen);
            AnimationPlayer.AnimationFinished -= OnCloseAnimationFinished;
        }
    }
    
    private void OnOpenAnimationFinished(StringName animName)
    {
        if (animName == "Close")
        {
            closed = false;
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeInteractableText), StringManager.Instance.Interactables.DoorClose);
            AnimationPlayer.AnimationFinished -= OnOpenAnimationFinished;
        }
    }
}
