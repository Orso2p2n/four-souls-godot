using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
    public static Game ME;

    [Export] Node playersContainer;
    [Export] public HUD hud;

    [ExportCategory("Scenes")]
    [Export] PackedScene gameBoardScene;
    [Export] PackedScene mainPlayerScene;
    [Export] PackedScene otherPlayerScene;

    public List<Player> players = new List<Player>();
    GameBoard gameBoard;

    public override void _Ready() {
        ME = this;

        CreateGameBoard();
        
        CreateMainPlayer();
        CreateOtherPlayer();
        CreateOtherPlayer();
        CreateOtherPlayer();
    }

    void CreateGameBoard() {
        gameBoard = gameBoardScene.Instantiate() as GameBoard;
        gameBoard.ChangeParent(this);
    }

    void CreateMainPlayer() {
        var createdPlayer = mainPlayerScene.Instantiate() as MainPlayer; 

        OnPlayerCreated(createdPlayer);
    }

    void CreateOtherPlayer() {
        var createdPlayer = otherPlayerScene.Instantiate() as OtherPlayer; 

        OnPlayerCreated(createdPlayer);
    }

    void OnPlayerCreated(Player player) {
        player.ChangeParent(playersContainer);

        var playerNumber = players.Count;
        player.Init(this, playerNumber, gameBoard.playerLocations[playerNumber]);

        players.Add(player);
    }
}
