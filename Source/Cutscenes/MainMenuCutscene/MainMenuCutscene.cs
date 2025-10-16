using System.Threading.Tasks;
using Godot;

namespace ProjectCamera.Source.Cutscenes;

public partial class MainMenuCutscene : Cutscene
{
    [Export] private AnimationPlayer _animationPlayer;
    public override void _Ready()
    {
        
    }

    public override async Task RunSequence()
    {
        GameManager.Instance.IsCutscenePlaying = true;
        if (_animationPlayer != null)
        {
            await WaitForSeconds(0.5f);
            _animationPlayer.Play("OpenGate");
            await WaitForSeconds(2.5f);
            SignalManager.Instance.EmitSignal(nameof(SignalManager.CutsceneEnded));
        }
        
    }
}