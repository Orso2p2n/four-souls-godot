using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum LogSeverity {
    Custom,
    Info,
    Warning,
    Error,
    Network,
    Rpc
}

public partial class Console : Control
{
    public static Console ME;

    [Export] private ScrollContainer _scrollContainer;
    [Export] private VBoxContainer _vboxContainer;
    [Export] private RichTextLabel _richLabel;
    [Export] private TextEdit _textEdit;
    [Export] private float _timeVisibleOnLog;
    
    private Array<string> _logHistory = new();
    private Array<string> _commandHistory = new();

    private Timer timer;
    private Tween alphaTween;

    private ConsoleCommands _consoleCommands;

    private bool opened = false;

    public override void _EnterTree() {
        ME = this;

        timer = new Timer();
        timer.ChangeParent(this);
        timer.Timeout += OnTimerTimeout;

        _consoleCommands = new ConsoleCommands(this);
        _consoleCommands.ChangeParent(this);

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

        // Split command into members and remove empty members
        Array<string> members = Variant.From(enteredText.Split(" ")).AsGodotArray<string>();
        while (members.Contains("")) {
            members.Remove("");
        }

        var commandName = members[0];
        var arguments = members.Duplicate();
        arguments.RemoveAt(0);

        var command = _consoleCommands.GetCommand(commandName);
        if (command == null) {
            LogWarning($"Unknown command \"{enteredText}\".");
            return;
        }

        if (command.allArgsAsOne) {
            var commandNameLength = commandName.Length;
            arguments = new Array<string>() { enteredText.Remove(0, commandNameLength+1) };
        }

        _consoleCommands.CallCommand(command, arguments);
    }

    private void DisableTextEdit() {
        ClearTextEdit();
        _textEdit.Hide();
    }

    private void ClearTextEdit() {
        _textEdit.CallDeferred(TextEdit.MethodName.Clear);
    }

    private async Task ScrollToBottom() {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        _scrollContainer.SetDeferred(ScrollContainer.PropertyName.ScrollVertical, 999999);
    }

    private Color? GetColorForSeverity(LogSeverity severity) {
        switch (severity) {
            case LogSeverity.Info:
                return Colors.GreenYellow;

            case LogSeverity.Warning:
                return Colors.Orange;

            case LogSeverity.Error:
                return Colors.IndianRed;

            case LogSeverity.Network:
                return Colors.CornflowerBlue;

            case LogSeverity.Rpc:
                return Colors.DarkSeaGreen;

            default:
                return null;
        }
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void Test() {
        Log("Test");
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void _Log(string text, LogSeverity severity = LogSeverity.Custom) {
        var colorOrNull = GetColorForSeverity(severity);
        if (colorOrNull != null) {
            var color = (Color) colorOrNull;
            var colorHtml = color.ToHtml(false);
            text = $"[color=#{colorHtml}]{text}[/color]";
        }

        GD.PrintRich(text);

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

    public static void LogInfo(string text) {
        ME._Log(text, LogSeverity.Info);
    }

    public static void LogWarning(string text) {
        ME._Log(text, LogSeverity.Warning);
    }

    public static void LogError(string text) {
        ME._Log(text, LogSeverity.Error);
    }

    public static void LogNetwork(string text) {
        ME._Log(text, LogSeverity.Network);
    }

    public void LogRpc(string text) {
        Rpc(MethodName._Log, $"[{NetworkManager.ME.PeerID}] {text}", (int) LogSeverity.Custom);
    }
}
