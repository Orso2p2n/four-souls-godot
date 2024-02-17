using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardButton : MenuButton
{
	[Export(PropertyHint.Dir)] public string cardResourcesDir;
	
	List<CardResource> cardResources;

	public override void _Ready() {
		var popup = GetPopup();
		
		cardResources = Assets.GetAllResourcesOfTypeInPath<CardResource>(cardResourcesDir);

		var i = 0;
		foreach (var cardResource in cardResources) {
			popup.AddItem(cardResource.CardName);
			popup.SetItemIcon(i, cardResource.BgArt);
			popup.SetItemIconMaxWidth(i, 24);
			i++;
		}

		popup.IndexPressed += OnPopupPressed;
	}

	public void OnPopupPressed(long index) {
		var cardResource = cardResources[(int) index];

		CardManager.ME.CreateCard3D(cardResource, Vector3.Zero);
	}
}
