using Godot;

namespace ProjectCamera.Source.Cutscenes;

public partial class PlayCutscene : Cutscene
{
    [Export] private AnimationPlayer _animationPlayer;
    public override void _Ready()
    {
        
    }

    public override async void RunSequence()
    {
        GameManager.Instance.IsCutscenePlaying = true;
        if (_animationPlayer != null)
        {
            GD.Print("Play OpenGate");
            _animationPlayer.Play("OpenGate");
        }
        
    }
}