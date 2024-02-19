using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
    [Export] Node playersContainer;
    [Export] public HUD hud;

    [ExportCategory("Scenes")]
    [Export] PackedScene gameBoardScene;
    [Export] PackedScene mainPlayerScene;

    List<Player> players = new List<Player>();
    GameBoard gameBoard;

    public override void _Ready() {
        CreateGameBoard();
        CreateMainPlayer();
    }

    void CreateGameBoard() {
        gameBoard = gameBoardScene.Instantiate() as GameBoard;
        gameBoard.ChangeParent(this);
    }

    void CreateMainPlayer() {
        var createdPlayer = mainPlayerScene.Instantiate() as MainPlayer; 

        OnPlayerCreated(createdPlayer);
    }

    void OnPlayerCreated(Player player) {
        player.ChangeParent(playersContainer);

        var playerNumber = players.Count;
        player.Init(this, playerNumber, gameBoard.playerLocations[playerNumber]);

        players.Add(player);
    }
}
