using Godot;
using System;
using System.Threading.Tasks;

public partial class CardVisual : SubViewport
{
	[ExportGroup("TextureRects")]
	[Export] public TextureRect maskTextureRect;
	[Export] public TextureRect bgArtTextureRect;
	[Export] public TextureRect borderTextureRect;
	[Export] public TextureRect bottomTextureRect;
	[Export] public TextureRect topTextureRect;
	[Export] public TextureRect statblockTextureRect;
	[Export] public TextureRect rewardTextureRect;
	[Export] public TextureRect soulTextureRect;
	[Export] public TextureRect charmedTextureRect;
	[Export] public TextureRect fgArtTextureRect;
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task<ViewportTexture> UpdateAndGetTexture() {
		await UpdateTexture();

		return GetTexture();
	}

	private async Task UpdateTexture() {
		RenderTargetUpdateMode = UpdateMode.Once;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}
}
