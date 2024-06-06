using Godot;
using System;

public partial class Card3DModel : Node3D
{
	public Card3D Card3D { get; set; }

	public MeshInstance3D Mesh { get; set; }

	private ShaderMaterial _sidesMaterial;
	private StandardMaterial3D _frontMaterial;
	private StandardMaterial3D _backMaterial;

	public Vector3 BaseRotation { 
		get {
			return _baseRotation;
		}		
		set {
			_baseRotation = value;
			RefreshRotation();
		}
	}

	private Vector3 _baseRotation;
	private Vector3 _oldRot;

	public override void _Ready() {
		BaseRotation = Vector3.Zero;
		
		Mesh = GetChild(0) as MeshInstance3D;

		_sidesMaterial = Mesh.GetSurfaceOverrideMaterial(0).Duplicate() as ShaderMaterial;
		_frontMaterial = Mesh.GetSurfaceOverrideMaterial(1).Duplicate() as StandardMaterial3D;
		_backMaterial = Mesh.GetSurfaceOverrideMaterial(2).Duplicate() as StandardMaterial3D;

		Mesh.SetSurfaceOverrideMaterial(0, _sidesMaterial);
		Mesh.SetSurfaceOverrideMaterial(1, _frontMaterial);
		Mesh.SetSurfaceOverrideMaterial(2, _backMaterial);
    }

    public override void _Process(double delta) {
		RefreshRotation();
    }

	private void RefreshRotation() {
		var rot = BaseRotation;

		if (_oldRot != rot) {
			_oldRot = rot;
			RotationDegrees = rot;
		}
	}

	public void SetFrontTexture(Texture2D texture) {
		_frontMaterial.AlbedoTexture = texture;
	}

	public void SetBackTexture(Texture2D texture) {
		_backMaterial.AlbedoTexture = texture;
	}

	public void SetFrontDarkened(bool darkened) {
		var modulateColor = Colors.White;
		
		if (darkened) {
			modulateColor = Colors.Black;
		}

		_frontMaterial.AlbedoColor = modulateColor;
	}
}
