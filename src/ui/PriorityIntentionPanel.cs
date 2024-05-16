using Godot;
using System;

public partial class PriorityIntentionPanel : Panel
{
	[Signal] public delegate void YesPressedEventHandler();
	[Signal] public delegate void NoPressedEventHandler();

	private void OnNoButtonPressed() {
		EmitSignal(SignalName.NoPressed);
	}
	private void OnYesButtonPressed() {
		EmitSignal(SignalName.YesPressed);
	}
}
