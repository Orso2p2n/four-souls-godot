using Godot;
using Godot.Collections;
using System;
public partial class DeckManager : Node
{
    public static DeckManager ME { get; private set; }

    private Array<DeckSetResource> _deckSets;

    public Deck CharacterDeck { get; private set; }
    public Deck LootDeck { get; private set; }

    public DeckManager(Array<DeckSetResource> deckSets) {
        _deckSets = deckSets;
    }

    public override void _EnterTree() {
        base._EnterTree();

        ME = this;

        Array<CardResource> lootCards = new();
        Array<CardResource> characterCards = new();

        foreach (var deckSet in _deckSets) {
            foreach (var deckResource in deckSet.Decks) {
                var cards = GetCardsFromDeckResource(deckResource);

                switch (deckResource.DeckType.deckType) {
                    case DeckType.Loot:
                        lootCards += cards;
                        break;

                    case DeckType.Character:
                        characterCards += cards;
                        break;
                }
            }
        }

        CreateCharacterDeck(characterCards);
        CreateLootDeck(lootCards);
    }

    private Array<CardResource> GetCardsFromDeckResource(DeckResource deckResource) {
        var script = deckResource.DeckListScript;

        var obj = new GodotObject();
        var cardList = obj.SafelySetScript<DeckCardList>(script);
        
        return cardList.GetCards();
    }

    private void CreateCharacterDeck(Array<CardResource> cards) {
        var deckLocation = Game.ME.GameBoard.GetDeckLocation(DeckType.Character);

        CharacterDeck = Assets.ME.DeckScene.Instantiate() as Deck;
        CharacterDeck.Name = "CharacterDeck";
        CharacterDeck.ChangeParent(this);
        CharacterDeck.Init(cards, Assets.ME.DeckTypeCharacter, deckLocation);
    }

    private void CreateLootDeck(Array<CardResource> cards) {
        var deckLocation = Game.ME.GameBoard.GetDeckLocation(DeckType.Loot);

        LootDeck = Assets.ME.DeckScene.Instantiate() as Deck;
        LootDeck.Name = "LootDeck";
        LootDeck.ChangeParent(this);
        LootDeck.Init(cards, Assets.ME.DeckTypeLoot, deckLocation);
    }
}
