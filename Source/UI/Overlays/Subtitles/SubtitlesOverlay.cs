using Godot;
using System;

public partial class SubtitlesOverlay : Control
{
    [Export] public Label personNameLabel;
    [Export] public Label subtitleLabel;
    [Export] public float charactersPerSecond = 30.0f;
    [Export] public float punctuationPauseDuration = 0.3f;
    
    private string currentSubtitle = "";
    private int currentCharIndex = 0;
    private float charTimer = 0.0f;
    private bool isTyping = false;
    private bool hasDurationTimerEnded = false;
    private bool isWaitingAfterPunctuation = false;
    private float punctuationWaitTimer = 0.5f;
    private SceneTreeTimer activeTimer = null;
    
    public void ShowSubtitle(string personName, string subtitle, float duration = 0.0f)
    {
        if (activeTimer != null)
        {
            activeTimer.Timeout -= OnDurationTimerTimeout;
            activeTimer = null;
        }
        
        personNameLabel.Text = personName;
        currentSubtitle = subtitle;
        currentCharIndex = 0;
        charTimer = 0.0f;
        isTyping = true;
        hasDurationTimerEnded = false;
        isWaitingAfterPunctuation = false;
        punctuationWaitTimer = 0.0f;
        subtitleLabel.Text = "";
        Visible = true;
        if (duration > 0.0f)
        {
            activeTimer = GetTree().CreateTimer(duration);
            activeTimer.Timeout += OnDurationTimerTimeout;
        }
    }
    
    private void OnDurationTimerTimeout()
    {
        activeTimer = null;
        HideSubtitle();
    }
    
    public void HideSubtitle()
    {
        Visible = false;
        isTyping = false;
        hasDurationTimerEnded = true;
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
    
    public bool HasActiveDurationTimerEnded()
    {
        return hasDurationTimerEnded;
    }
    
    public override void _Process(double delta)
    {
        if (!isTyping) return;
        
        if (isWaitingAfterPunctuation)
        {
            punctuationWaitTimer += (float)delta;
            if (punctuationWaitTimer >= punctuationPauseDuration)
            {
                isWaitingAfterPunctuation = false;
                punctuationWaitTimer = 0.0f;
            }
            return;
        }
        
        charTimer += (float)delta;
        float timePerChar = 1.0f / charactersPerSecond;
        
        while (charTimer >= timePerChar && currentCharIndex < currentSubtitle.Length)
        {
            currentCharIndex++;
            subtitleLabel.Text = currentSubtitle.Substring(0, currentCharIndex);
            charTimer -= timePerChar;
            
            if (currentCharIndex < currentSubtitle.Length)
            {
                char currentChar = currentSubtitle[currentCharIndex - 1];
                if (currentChar == ',' || currentChar == '.' || currentChar == '?')
                {
                    isWaitingAfterPunctuation = true;
                    break;
                }
            }
        }
        
        if (currentCharIndex >= currentSubtitle.Length)
        {
            isTyping = false;
        }
    }
}
