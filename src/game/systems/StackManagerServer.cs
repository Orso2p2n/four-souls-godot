using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class StackManagerServer : StackManager
{
    public Array<StackEffect> EffectsOnStack;

    private int _priorityCurNumber;
    private int _priorityStartNumber;

    private bool _actingPlayerHasActed;

    public override void _Ready() {
        base._Ready();

        Clear();
    }

    public override void AddEffect(StackEffect effect, Player owner) {
        if (StackIsEmpty) {
            Rpc(StackManager.MethodName.SetStackIsEmpty, false);
        }

        owner.EmitSignal(Player.SignalName.PriorityAction, true);

        EffectsOnStack.Add(effect);
        _ = StartPriority(owner.PlayerNumber);
    }

    public void Clear() {
        Rpc(StackManager.MethodName.SetStackIsEmpty, true);
        EffectsOnStack = new Array<StackEffect>();
    }

    public void ResolveStack() {
        Console.Log("Resolving stack");
        var length = EffectsOnStack.Count;
        for (int i = length - 1; i >= 0 ; i--) {
            var stackEffect = EffectsOnStack[i];

            stackEffect.Resolve();
        }

        Clear();
    }
    
    public override async Task StartPriority(int startPlayer) {
        var players = GameServer.ME.Players;
        var playersCount = players.Count;

        var max = startPlayer + playersCount;

        Console.Log("-----------------");

        // Iterate over all the players and ask for priority intention
        for (int i = startPlayer; i < max; i++) {
            var j = i;
            if (i >= playersCount) j -= playersCount;
            
            var player = GameServer.ME.Players[j];

            player.Rpc(Player.MethodName.RemovePriority);
            player.Rpc(Player.MethodName.AskForPriorityIntention);
        }

        // Iterate again and check the intention
        for (int i = startPlayer; i < max; i++) {
            var j = i;
            if (i >= playersCount) j -= playersCount;
            
            var player = GameServer.ME.Players[j];

            if (player.PriorityIntention == PriorityIntention.Deciding) {
                await ToSignal(player, Player.SignalName.PriorityIntentionChosen);
            }

            if (player.PriorityIntention == PriorityIntention.Acting) {
                var hasActed = false;

                player.Rpc(Player.MethodName.GetPriority);

                void OnPlayerAction(bool passesPriority) { hasActed = passesPriority; }

                player.PriorityAction += OnPlayerAction;
                await ToSignal(player, Player.SignalName.PriorityAction);
                player.PriorityAction -= OnPlayerAction;

                if (hasActed) {
                    return;
                }

                player.Rpc(Player.MethodName.RemovePriority);
            }
        }

        ResolveStack();
    }
}
