using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
    [Signal] public delegate void RngSeedChangedEventHandler();

    public static Game ME { get; private set; }

    private Node _playersContainer;
    public Array<Player> Players { get; set; }

    public RandomNumberGenerator Rng { get; set; }
    private bool _rngInit;

    public CardFactory CardFactory { get; set; }
    public DeckManager DeckManager { get; set; }
    public StackManager StackManager { get; set; }
    public TurnManager TurnManager { get; set; }

    private GameBoard _gameBoard;

    public override void _EnterTree() {
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

        CreateCardFactory();
        CreateDeckManager();
        CreateStackManager();
        CreateTurnManager();
        
        CreateGameBoard();

        CreatePlayers();

        InitDone();
    }

	[Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected virtual void InitDone(int peerId = -1) {}

    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void StartGame() {
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
        _gameBoard = Assets.ME.GameBoardScene.Instantiate() as GameBoard;
        _gameBoard.ChangeParent(this);
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
    }

    protected void CreateMainPlayer() {
        var createdPlayer = Assets.ME.MainPlayerScene.Instantiate() as MainPlayer; 
        OnPlayerCreated(createdPlayer);
    }

    protected void CreateOnlinePlayer() {
        var createdPlayer = Assets.ME.OnlinePlayerScene.Instantiate() as OnlinePlayer; 
        OnPlayerCreated(createdPlayer);
    }

    protected void OnPlayerCreated(Player player) {
        player.ChangeParent(_playersContainer);
        player.Name = "Player" + (Players.Count + 1);

        var playerNumber = Players.Count;
        player.Init(playerNumber, _gameBoard.PlayerLocations[playerNumber]);

        Players.Add(player);
    }

    // --- Network ---
    private void OnServerDisconnected() {
		SceneManager.ME.GotoMainMenu();
	}
}