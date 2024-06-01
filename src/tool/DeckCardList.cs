using Godot;
using System;
using System.Collections.Generic;

public struct CardEntry {
    public CardEntry(string path, int count = 1) {
        this.path = path;
        this.count = count;
    }

    public string path;
    public int count;
}

public partial class DeckCardList : GodotObject
{
    protected virtual List<CardEntry> CardsList {
        get {
            return new();
        }
    }

    public Godot.Collections.Array<CardResource> GetCards() {
        var list = CardsList;
        var cards = new Godot.Collections.Array<CardResource>();

        if (list.Count == 0) {
            return cards;
        }

        foreach (var cardEntry in list) {
            var count = cardEntry.count;
            var path = $"res://resources/cards/{cardEntry.path}.tres";
            var cardResource = ResourceLoader.Load<CardResource>(path);
            for (int i = 0; i < count; i++) {
                cards.Add(cardResource);
            }
        }

        return cards;
    }
}
