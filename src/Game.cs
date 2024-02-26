using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
    public static Game ME { get; private set; }

    [Export] private Node _playersContainer;
    [Export] public HUD Hud;
    [Export] public StackManager StackManager { get; private set; }

    [ExportCategory("Scenes")]
    [Export] private PackedScene _gameBoardScene;
    [Export] private PackedScene _mainPlayerScene;
    [Export] private PackedScene _otherPlayerScene;

    public Array<Player> Players { get; set; }
    private GameBoard _gameBoard;

    public override void _Ready() {
        ME = this;

        Players = new Array<Player>();

        CreateGameBoard();

        CreateMainPlayer();
        CreateOtherPlayer();
        CreateOtherPlayer();
        CreateOtherPlayer();
    }

    void CreateGameBoard() {
        _gameBoard = _gameBoardScene.Instantiate() as GameBoard;
        _gameBoard.ChangeParent(this);
    }

    void CreateMainPlayer() {
        var createdPlayer = _mainPlayerScene.Instantiate() as MainPlayer; 

        OnPlayerCreated(createdPlayer);
    }

    void CreateOtherPlayer() {
        var createdPlayer = _otherPlayerScene.Instantiate() as OtherPlayer; 

        OnPlayerCreated(createdPlayer);
    }

    void OnPlayerCreated(Player player) {
        player.ChangeParent(_playersContainer);

        var playerNumber = Players.Count;
        player.Init(this, playerNumber, _gameBoard.PlayerLocations[playerNumber]);

        Players.Add(player);
    }
}
