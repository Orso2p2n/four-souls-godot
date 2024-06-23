using Fractural.Tasks;
using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
    [Signal] public delegate void RngSeedChangedEventHandler();
    [Signal] public delegate void LootedPlayerEventHandler(Player player, Array<CardBase> cards);
    [Signal] public delegate void GaveCharacterToPlayerEventHandler(Player player, CharacterCard card);

    public static Game ME { get; private set; }

    private Node _playersContainer;
    private readonly int _maxPlayersCount = 4;
    public Array<Player> Players { get; set; }

    public RandomNumberGenerator Rng { get; set; }
    private bool _rngInit;

    public CardFactory CardFactory { get; set; }
    public DeckManager DeckManager { get; set; }
    public StackManager StackManager { get; set; }
    public TurnManager TurnManager { get; set; }

    public GameBoard GameBoard { get; set; }

    public override void _Ready() {
        Init();
    }

    protected async void Init() {
        ME = this;

        NetworkManager.ME.GameState = GameState.InGame;
        NetworkManager.ME.Multiplayer.ServerDisconnected += OnServerDisconnected;

        CreateRng();

        if (!_rngInit) {
            await ToSignal(this, SignalName.RngSeedChanged);
        }

        Players = new Array<Player>();

        _playersContainer = new Node { Name = "PlayersContainer" };
        _playersContainer.ChangeParent(this);

        CreateGameBoard();

        CreateCardFactory();
        CreateDeckManager();
        CreateStackManager();
        CreateTurnManager();

        CreatePlayers();

        InitDone();
    }

	[Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected virtual void InitDone(int peerId = -1) {}

    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected void StartFirstTurn() {
        TurnManager.StartTurn(Players[0]);
    }

    // --- Systems creation ---
    protected virtual void CreateRng() {
        Rng = new RandomNumberGenerator();
    }

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected virtual void SetRngSeed(ulong seed) {
        Console.Log("SET SEED " + seed);
        Rng.Seed = seed;

        if (!_rngInit) {
            _rngInit = true;
        }

        EmitSignal(SignalName.RngSeedChanged);
    }

    protected virtual void CreateCardFactory() {
        if (CardFactory != null) {
            return;
        }

        CardFactory = new CardFactory() { Name = "CardFactory" };
        CardFactory.ChangeParent(this);
    }

    protected virtual void CreateDeckManager() {
        if (DeckManager != null) {
            return;
        }

        var decks = new Array<DeckSetResource>() { Assets.ME.DeckSetBaseGame };

        DeckManager = new DeckManager(decks) { Name = "DeckManager" };
        DeckManager.ChangeParent(this);
    }

    protected virtual void CreateStackManager() {
        throw new NotImplementedException();
    }

    protected virtual void CreateTurnManager() {
        throw new NotImplementedException();
    }

    protected void CreateGameBoard() {
        GameBoard = Assets.ME.GameBoardScene.Instantiate() as GameBoard;
        GameBoard.ChangeParent(this);
    }

    // --- Players creation ---
    protected void CreatePlayers() {
        var users = NetworkManager.ME.Users;
        foreach (var user in users) {
            if (user.IsSelf) {
                CreateMainPlayer();
            }
            else {
                CreateOnlinePlayer();
            }
        }

        var emptySlotsCount = _maxPlayersCount - Players.Count;
        for (int i = 0; i < emptySlotsCount; i++) {
            CreateCpuPlayer();
        }
    }

    protected void CreateMainPlayer() {
        var createdPlayer = Assets.ME.MainPlayerScene.Instantiate() as MainPlayer; 
        OnPlayerCreated(createdPlayer);
    }

    protected void CreateOnlinePlayer() {
        var createdPlayer = Assets.ME.OnlinePlayerScene.Instantiate() as OnlinePlayer; 
        OnPlayerCreated(createdPlayer);
    }

    protected void CreateCpuPlayer() {
        var createdPlayer = Assets.ME.CpuPlayerScene.Instantiate() as CPUPlayer; 
        OnPlayerCreated(createdPlayer);
    }

    protected void OnPlayerCreated(Player player) {
        player.ChangeParent(_playersContainer);
        player.Name = "Player" + (Players.Count + 1);

        var playerNumber = Players.Count;
        player.Init(playerNumber, GameBoard.PlayerLocations[playerNumber]);

        Players.Add(player);
    }

    // --- Network ---
    private void OnServerDisconnected() {
		SceneManager.ME.GotoMainMenu();
	}

    // --- Gameplay ---
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public async void Loot(int lootingPlayerId, int count) {
        var maxLootTime = 1.5f;
        var delayPerCard = 0.3f;

        var fullDelay = delayPerCard * count;

        if (fullDelay > maxLootTime) {
            delayPerCard = maxLootTime / count;
        }

        var player = Players[lootingPlayerId];
        var cards = new Array<CardBase>();

        for (int i = 0; i < count; i++){
            var card = DeckManager.LootDeck.CreateTopCard();
            cards.Add(card);
            player.TryAddCardInHand(card);

            await GDTask.Delay(TimeSpan.FromSeconds(delayPerCard));
        }

        EmitSignal(SignalName.LootedPlayer, player, cards);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public async void GiveRandomCharacterToPlayer(int playerId) {
        var delay = 0.3f;

        var player = Players[playerId];
        var card = DeckManager.CharacterDeck.CreateTopCard() as CharacterCard;
        player.SetCharacter(card);
        card.Card3d.FlipUp(true);

        await GDTask.Delay(TimeSpan.FromSeconds(delay));

        EmitSignal(SignalName.GaveCharacterToPlayer, player, card);
    }
}