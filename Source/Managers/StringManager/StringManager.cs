using Godot;
using System;

public partial class StringManager : Node
{
    public static StringManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<StringManager>("StringManager");
    public NamesClass Names = new NamesClass();
    public DialogueClass Dialogue = new DialogueClass();
    public InteractablesClass Interactables = new InteractablesClass();
    public class NamesClass
    {
        public string Novak = "Novak";
    }
    public class DialogueClass
    {
        // sleeping
        public string WhatsThatNoise = "What's that noise?";
        public string ItsProbablyTrashDoor = "Hmm... It's probably just the garbage enclosure gate.";
        public string FuckICantSleep = "Fuck! I can't sleep.";
        public string ImGonnaCheckItOut = "I'll see what's making that noise from the window.";

        // outside
        public string ItsSoCold = "Brrr... It's so cold. I'd better get this done quickly and go to sleep.";
        public string IllCloseThisGate = "I’ll just close the gate and then I’m off to bed.";
        public string WhatWasThatNoise = "What the fuck is that?! Fuck this shit, I’m heading back to sleep.";
        public string WasThatAGorilla = "Oh fuck! Am I seeing things? Was that... a gorilla? I must be really tired...";


    }

    public class InteractablesClass
    {
        public string LampOn = "Turn off";
        public string LampOff = "Turn on";
        public string DoorOpen = "Open";
        public string DoorClose = "Close";
    }

}
