using Godot;
using Godot.Collections;
using System;

public partial class SpawnCardButton : MenuButton
{
	[Export(PropertyHint.Dir)] private string _cardResourcesDir;
	
	private Array<CardResource> _cardResources;

	public override void _Ready() {
		var popup = GetPopup();
		
		_cardResources = Assets.GetAllResourcesOfTypeInPath<CardResource>(_cardResourcesDir);

		var i = 0;
		foreach (var cardResource in _cardResources) {
			popup.AddItem(cardResource.CardName);
			popup.SetItemIcon(i, cardResource.BgArt);
			popup.SetItemIconMaxWidth(i, 24);
			i++;
		}

		popup.IndexPressed += OnPopupPressed;
	}

	public void OnPopupPressed(long index) {
		var cardResource = _cardResources[(int) index];

		SpawnCard(cardResource);
	}

	public virtual CardBase SpawnCard(CardResource cardResource) {
		var card = CardFactory.ME.CreateCard3D(cardResource, Vector3.Zero);

		return card;
	}
}
