using System.IO;
using System.Threading.Tasks;
using Godot;

namespace ProjectCamera.Source.Cutscenes;

public partial class MainMenuCutscene : Cutscene
{
    [Export] private AnimationPlayer _animationPlayer;

    [Export(PropertyHint.File, "*.wav")] private string soundPath;
    [Export] private Node3D soundPositionNode;
    private static AudioStreamWav soundStream;
    public override void _Ready()
    {
        soundStream = ResourceLoader.Load<AudioStreamWav>(soundPath);
    }

    public override async Task RunSequence()
    {
        GameManager.Instance.IsCutscenePlaying = true;
        if (_animationPlayer != null)
        {
            await WaitForSeconds(0.5f);
            _animationPlayer.Play("OpenGate");
            AudioManager.Instance.CreateAudioOneShotAtPosition(soundStream, soundPositionNode.GlobalPosition);
            await WaitForSeconds(2.5f);
            SignalManager.Instance.EmitSignal(nameof(SignalManager.CutsceneEnded));
        }

    }
    private void OnSourceButtonPressed()
    {
        OS.ShellOpen("https://github.com/6ajmon/Catatonia-Breakdown");
    }
}