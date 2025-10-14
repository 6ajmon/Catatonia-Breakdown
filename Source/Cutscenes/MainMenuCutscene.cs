using System.Threading.Tasks;
using Godot;

namespace ProjectCamera.Source.Cutscenes;

public partial class MainMenuCutscene : Cutscene
{
    [Export] private AnimationPlayer _animationPlayer;
    public override void _Ready()
    {
        
    }

    public override async Task AwaitAndRunSequence()
    {
        GameManager.Instance.IsCutscenePlaying = true;
        if (_animationPlayer != null)
        {
            _animationPlayer.Play("OpenGate");
            await WaitForSeconds(2.0f);
            SignalManager.Instance.EmitSignal(nameof(SignalManager.CutsceneEnded));
        }
        
    }
}