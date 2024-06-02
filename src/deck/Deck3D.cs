using Godot;
using System;

public partial class Deck3D : Node3D
{
	[Export] private Node3D _model;
	private MeshInstance3D _mesh;

	private ShaderMaterial _topMaterial;
	private ShaderMaterial _sidesMaterial;

	public Deck Deck { get; set; }

	private int _cardsCount;

    public void Init(Deck deck) {
		Deck = deck;

		_mesh = _model.GetChild(0) as MeshInstance3D;

		_sidesMaterial = _mesh.GetSurfaceOverrideMaterial(0).Duplicate() as ShaderMaterial;
		_topMaterial = _mesh.GetSurfaceOverrideMaterial(1).Duplicate() as ShaderMaterial;
		
		_topMaterial.SetShaderParameter("albedo_texture", Deck.DeckType.BackTexture);

		_mesh.SetSurfaceOverrideMaterial(0, _sidesMaterial);
		_mesh.SetSurfaceOverrideMaterial(1, _topMaterial);
	}

	public void SetCardsCount(int count) {
		_cardsCount = count;

		var yScale = 0.01f * _cardsCount;
		_model.Scale = _model.Scale with { Y = yScale };
	}
}
