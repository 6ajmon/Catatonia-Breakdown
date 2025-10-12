using Godot;
using System;

public partial class StringManager : Node
{
    public static StringManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<StringManager>("StringManager");
    public NamesClass Names = new NamesClass();
    public DialogueClass Dialogue = new DialogueClass();
    public class NamesClass
    {
        public string Novak = "Novak";
    }
    public class DialogueClass
    {
        public string WhatsThatNoise = "What's that noise?";
        public string ItsProbablyTrashDoor = "Hmm... It's probably just the garbage enclosure gate.";
        public string FuckICantSleep = "Fuck! I can't sleep.";
        public string ImGonnaCheckItOut = "I'll see what's making that noise from the window.";
    }

    

}
