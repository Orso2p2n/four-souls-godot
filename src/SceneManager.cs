using Godot;
using System;

public partial class SceneManager : Node
{
    public static SceneManager ME;

    [Export] private PackedScene mainMenuScene;
    [Export] private PackedScene lobbyScene;
    [Export] private PackedScene gameScene;

    public Node CurrentScene { get; set; }

    public override void _EnterTree() {
        ME = this;

        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void GotoScene(PackedScene scene) {
        CallDeferred(MethodName.DeferredGotoScene, scene);
    }

    public void DeferredGotoScene(PackedScene scene) {
        CurrentScene.Free();

        CurrentScene = scene.Instantiate();

        GetTree().Root.AddChild(CurrentScene);
        GetTree().CurrentScene = CurrentScene;
    }

    public void GotoMainMenu() {
        GotoScene(mainMenuScene);
    }

    public void GotoGame() {
        GotoScene(gameScene);
    }

    public void GotoLobby() {
        GotoScene(lobbyScene);
    }
}
