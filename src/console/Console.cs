using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

enum LogSeverity {
    Info,
    Warning,
    Error
}

public partial class Console : Control
{
    static Console ME;

    [Export] private ScrollContainer _scrollContainer;
    [Export] private VBoxContainer _vboxContainer;
    [Export] private RichTextLabel _richLabel;
    [Export] private TextEdit _textEdit;
    [Export] private float _timeVisibleOnLog;
    
    private Array<string> _logHistory = new();
    private Array<string> _commandHistory = new();

    private Timer timer;
    private Tween alphaTween;

    private bool opened = false;

    public override void _EnterTree() {
        ME = this;

        timer = new Timer();
        AddChild(timer);
        timer.Timeout += OnTimerTimeout;

        SetAlpha(0f);

        _textEdit.Hide();
    }

    private void SetVisibleForDuration(float seconds) {
        alphaTween?.Kill();
        
        SetAlpha(1f);

        timer.Start(seconds);
    }

    private void OnTimerTimeout() {
        timer.Stop();

        if (_textEdit.Visible) {
            return;
        }

        alphaTween = CreateTween();

        var callable = Callable.From<float>(SetAlpha);
        alphaTween.TweenMethod(callable, 1f, 0f, 0.5f);

        callable.Call(0.5f);
    }

    private void SetAlpha(float value) {
        var alpha = Mathf.Clamp(value, 0f, 1f);
        Modulate = Modulate with {A = alpha};
    }

    private void Open() {
        if (opened) {
            return;
        }

        opened = true;

        SetVisibleForDuration(Mathf.Inf);

        ClearTextEdit();
        _textEdit.Show();
        _textEdit.GrabFocus();
    }

    private void Close() {
        if (!opened) {
            return;
        }

        opened = false;

        DisableTextEdit();
        SetVisibleForDuration(_timeVisibleOnLog);
    }

    private void Enter() {
        if (!opened) {
            return;
        }

        var enteredText = _textEdit.Text;

        ClearTextEdit();

        if (enteredText == "") {
            return;
        }

        LogWarning($"Unknown command \"{enteredText}\"");
    }

    private void DisableTextEdit() {
        ClearTextEdit();
        _textEdit.Hide();
    }

    private void ClearTextEdit() {
        _textEdit.CallDeferred(TextEdit.MethodName.Clear);
    }

    private Color? GetColorForSeverity(LogSeverity severity) {
        switch (severity) {
            case LogSeverity.Warning:
                return Colors.Orange;

            case LogSeverity.Error:
                return Colors.Red;

            default:
                return null;
        }
    }

    private async Task ScrollToBottom() {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        _scrollContainer.SetDeferred(ScrollContainer.PropertyName.ScrollVertical, 999999);
    }

    private void _Log(string text, LogSeverity severity = LogSeverity.Info) {
        var colorOrNull = GetColorForSeverity(severity);
        if (colorOrNull != null) {
            var color = (Color) colorOrNull;
            var colorHtml = color.ToHtml(false);
            text = $"[color=#{colorHtml}]{text}[/color]";
        }

        text = $"\n{text}";

        _logHistory.Add(text);
        _richLabel.Text += text;

        _ = ScrollToBottom();

        SetVisibleForDuration(_timeVisibleOnLog);
    }

    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("console_open")) {
            Open();
        }
        else if (@event.IsActionPressed("console_close")) {
            Close();
        }
        else if (@event.IsActionPressed("console_enter")) {
            Enter();
        }
    }

    // Static functions
    public static void Log(string text) {
        ME._Log(text);
    }

    public static void LogWarning(string text) {
        ME._Log(text, LogSeverity.Warning);
    }

    public static void LogError(string text) {
        ME._Log(text, LogSeverity.Error);
    }
}
