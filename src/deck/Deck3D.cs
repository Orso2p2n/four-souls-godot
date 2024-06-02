using Godot;
using System;

public partial class Deck3D : Node3D
{
	[Export] public Node3D Model { get; set; }
	public MeshInstance3D Mesh { get; set; }

	private ShaderMaterial _topMaterial;
	private ShaderMaterial _sidesMaterial;

	public Deck Deck { get; set; }

	private int _cardsCount;

    public void Init(Deck deck) {
		Deck = deck;

		Mesh = Model.GetChild(0) as MeshInstance3D;

		_sidesMaterial = Mesh.GetSurfaceOverrideMaterial(0).Duplicate() as ShaderMaterial;
		_topMaterial = Mesh.GetSurfaceOverrideMaterial(1).Duplicate() as ShaderMaterial;
		
		_topMaterial.SetShaderParameter("albedo_texture", Deck.DeckType.BackTexture);

		Mesh.SetSurfaceOverrideMaterial(0, _sidesMaterial);
		Mesh.SetSurfaceOverrideMaterial(1, _topMaterial);
	}

	public void SetCardsCount(int count) {
		_cardsCount = count;

		var yScale = 0.01f * _cardsCount;
		Model.Scale = Model.Scale with { Y = yScale };
	}
}
