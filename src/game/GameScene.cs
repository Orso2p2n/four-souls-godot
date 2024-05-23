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
            _game = new GameServer();
        }
        else {
            _game = new GameClient();
        }

        _game.ChangeParent(this);
        _game.Name = "Game";
    }
}
