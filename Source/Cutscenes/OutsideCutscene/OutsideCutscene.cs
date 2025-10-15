using Godot;
using System;
using System.Threading.Tasks;

public partial class OutsideCutscene : Cutscene
{
    public override async Task RunSequence()
    {
        await Task.CompletedTask;
    }
}
