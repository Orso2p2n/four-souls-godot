using Godot;
using Godot.Collections;
using System;

public partial class Console : Control
{
    static Console ME;

    [Export] private ScrollContainer _scrollContainer;
    [Export] private VBoxContainer _vboxContainer;
    [Export] private RichTextLabel _richLabel;
    [Export] private float _timeVisibleOnLog;
    
    private Array<string> logs = new();

    private Timer timer;
    private Tween alphaTween;

    public override void _EnterTree() {
        ME = this;

        timer = new Timer();
        AddChild(timer);
        timer.Timeout += OnTimerTimeout;

        SetAlpha(0f);

        Log("Bonjour!");
    }

    private void SetVisibleForDuration(float seconds) {
        alphaTween?.Kill();
        
        SetAlpha(1f);

        timer.Start(seconds);
    }

    private void OnTimerTimeout() {
        alphaTween = CreateTween();

        var callable = Callable.From<float>(SetAlpha);
        alphaTween.TweenMethod(callable, 1f, 0f, 0.5f);

        callable.Call(0.5f);

        timer.Stop();
    }

    private void SetAlpha(float value) {
        var alpha = Mathf.Clamp(value, 0f, 1f);
        Modulate = Modulate with {A = alpha};
    }

    private void _Log(string text) {
        logs.Add(text);
        _richLabel.Text = _richLabel.Text += $"\n{text}";

        _scrollContainer.ScrollVertical = 999999;

        SetVisibleForDuration(_timeVisibleOnLog);
    }

    public static void Log(string text) {
        ME._Log(text);
    }
}
