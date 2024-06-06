using Godot;
using System;

public partial class CardVisualStatContainer : Control
{
	[Export] private Label _hpLabel;
	[Export] private Label _diceLabel;
	[Export] private Label _atkLabel;

    public override void _Ready() {
        Hide();
    }

    public void SetHp(int value) {
		_hpLabel.Text = value.ToString();
	}

	public void SetDice(int value) {
		if (_diceLabel == null) {
			return;
		}

		var text = value.ToString();

		if (value > 0 && value < 6) {
			text += "+";
		}

		_diceLabel.Text = text;
	}

	public void SetAtk(int value) {
		_atkLabel.Text = value.ToString();
	}
}
