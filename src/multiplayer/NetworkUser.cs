using Godot;
using System;

public partial class NetworkUser : Node
{
    [Signal] public delegate void UsernameUpdatedEventHandler();

    public long Id { get; set; }
    public string Username { get; private set; } = "";
    public bool IsHost { get; set; }
    public bool IsSelf { get; set; }

    public void SetUsername(string username) {
        Username = username;
        EmitSignal(SignalName.UsernameUpdated);
    }
}