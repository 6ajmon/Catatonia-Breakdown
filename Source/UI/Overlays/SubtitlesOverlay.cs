using Godot;
using System;

public partial class SubtitlesOverlay : Control
{
    [Export] public Label personNameLabel;
    [Export] public Label subtitleLabel;
    [Export] public float charactersPerSecond = 30.0f; // Prędkość pisania znaków
    
    private string currentSubtitle = "";
    private int currentCharIndex = 0;
    private float charTimer = 0.0f;
    private bool isTyping = false;
    
    public void ShowSubtitle(string personName, string subtitle, float duration = 0.0f)
    {
        personNameLabel.Text = personName;
        currentSubtitle = subtitle;
        currentCharIndex = 0;
        charTimer = 0.0f;
        isTyping = true;
        subtitleLabel.Text = "";
        Visible = true;
        if (duration > 0.0f)
        {
            GetTree().CreateTimer(duration).Timeout += () => HideSubtitle();
        }
    }
    
    public void HideSubtitle()
    {
        Visible = false;
        isTyping = false;
        currentSubtitle = "";
        currentCharIndex = 0;
    }
    
    public void CompleteTyping()
    {
        if (isTyping && currentCharIndex < currentSubtitle.Length)
        {
            currentCharIndex = currentSubtitle.Length;
            subtitleLabel.Text = currentSubtitle;
            isTyping = false;
        }
    }
    
    public bool IsTyping()
    {
        return isTyping;
    }
    
    public override void _Process(double delta)
    {
        if (!isTyping) return;
        
        charTimer += (float)delta;
        float timePerChar = 1.0f / charactersPerSecond;
        
        while (charTimer >= timePerChar && currentCharIndex < currentSubtitle.Length)
        {
            currentCharIndex++;
            subtitleLabel.Text = currentSubtitle.Substring(0, currentCharIndex);
            charTimer -= timePerChar;
        }
        
        if (currentCharIndex >= currentSubtitle.Length)
        {
            isTyping = false;
        }
    }
}
