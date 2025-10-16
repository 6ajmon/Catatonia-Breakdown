using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class Cutscene : Node3D
{   
    [Export] public SubtitlesOverlay subtitlesOverlay;
    public StringManager.NamesClass names;
    public StringManager.DialogueClass dialogue;
    public bool inputReceived = false;
    public override async void _Ready()
    {
        names = StringManager.Instance.Names;
        dialogue = StringManager.Instance.Dialogue;
        await RunSequence();
    }

    public override void _ExitTree()
    {
        inputReceived = true;
    }

    public virtual async Task RunSequence()
    {
        await Task.CompletedTask;
    }

    public async Task WaitForPlayerInput()
    {
        inputReceived = false;
        
        while (!inputReceived)
        {
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Next"))
        {
            if (subtitlesOverlay != null && subtitlesOverlay.IsTyping())
            {
                subtitlesOverlay.CompleteTyping();
            }
            else
            {
                inputReceived = true;
            }
        }
        else if (subtitlesOverlay != null && subtitlesOverlay.HasActiveDurationTimerEnded())
        {
            inputReceived = true;
        }
    }
    public async Task WaitForSeconds(float seconds)
    {
        await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
    }
}
