using Godot;
using System;

public partial class CardVisualComposition : Control
{
	[ExportGroup("TextureRects")]
	[Export] public TextureRect BgArtTextureRect { get; private set; }
	[Export] public TextureRect BorderTextureRect { get; private set; }
	[Export] public TextureRect BottomTextureRect { get; private set; }
	[Export] public TextureRect TopTextureRect { get; private set; }
	[Export] public TextureRect StatblockTextureRect { get; private set; }
	[Export] public TextureRect RewardTextureRect { get; private set; }
	[Export] public TextureRect SoulTextureRect { get; private set; }
	[Export] public TextureRect CharmedTextureRect { get; private set; }
	[Export] public TextureRect FgArtTextureRect { get; private set; }

	[ExportGroup("Texts")]
	[Export] public CardVisualTitle TitleLabel { get; private set; }
	[Export] public DescriptionContainer DescriptionContainer { get; private set; }
}
