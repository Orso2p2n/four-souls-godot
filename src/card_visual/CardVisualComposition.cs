using Godot;
using System;

public partial class CardVisualComposition : Control
{
	[ExportGroup("TextureRects")]
	[Export] public TextureRect bgArtTextureRect;
	[Export] public TextureRect borderTextureRect;
	[Export] public TextureRect bottomTextureRect;
	[Export] public TextureRect topTextureRect;
	[Export] public TextureRect statblockTextureRect;
	[Export] public TextureRect rewardTextureRect;
	[Export] public TextureRect soulTextureRect;
	[Export] public TextureRect charmedTextureRect;
	[Export] public TextureRect fgArtTextureRect;

	[ExportGroup("Texts")]
	[Export] public CardVisualTitle titleLabel;
	[Export] public DescriptionContainer descriptionContainer;
}
