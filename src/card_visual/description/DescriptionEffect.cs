using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class DescriptionEffect : RichTextLabel
{
	[ExportGroup("Icons")]
	[Export] private Texture2D _hpIcon;
	[Export] private Texture2D _atkIcon;
	[Export] private Texture2D _diceIcon;

	private string _unprocessedText;

	private int _baseFontSize;
	private int _curFontSize;

	public FontVariation font;

	public override void _Ready() {
		_curFontSize = _baseFontSize = Theme.GetFontSize("normal_font_size", "RichTextLabel");

		var oldFont = Theme.GetFont("normal_font", "RichTextLabel");
        font = oldFont.Duplicate() as FontVariation;
		AddThemeFontOverride("normal_font", font);
	}

	public void SetText(string text) {
		_unprocessedText = text;
		
		Text = ProcessText(text);
	}

	public void RefreshText() {
		SetText(_unprocessedText);
	}

	public void ReduceFontSize(int amount) {
		_curFontSize -= amount;
		AddThemeFontSizeOverride("normal_font_size", _curFontSize);
		RefreshText();
	}

	private string ProcessText(string text) {
		text = "[center]" + text;

		text = ReplaceIconsInText(text, "[HP]", _hpIcon);
		text = ReplaceIconsInText(text, "[ATK]", _atkIcon);
		text = ReplaceIconsInText(text, "[DICE]", _diceIcon);

		return text;
	}

	private string ReplaceIconsInText(string text, string key, Texture2D icon) {
		var path = icon.ResourcePath;

		int targetHeight = _curFontSize;
		int iconHeight = icon.GetHeight();
		int iconWidth = icon.GetWidth();

		float ratio = targetHeight / iconHeight;

		int targetWidth = (int) (ratio * iconWidth);
		
		string newVal = $"[img=bottom,bottom,{targetWidth}x{targetHeight}]{path}[/img]";

		return text.Replace(key, newVal);
	}
}
