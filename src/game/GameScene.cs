using Godot;
using System;

public partial class GameScene : Node3D
{	
	private Game _game;

    public override void _EnterTree() {
        CreateGame();
    }

    private void CreateGame() {
        if (NetworkManager.ME.Status == NetworkStatus.Host) {
            _game = new GameServer() { Name = "Game" };
        }
        else {
            _game = new GameClient() { Name = "Game" };
        }

        _game.ChangeParent(this);
    }
}
