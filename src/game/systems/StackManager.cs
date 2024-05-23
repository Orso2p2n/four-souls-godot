using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum PriorityIntention {
    Deciding,
    NotActing,
    Acting
}

public partial class StackManager : Node
{
    public static StackManager ME;

    public Array<StackEffect> EffectsOnStack;

    private int _priorityCurNumber;
    private int _priorityStartNumber;

    public override void _Ready() {
        ME = this;

        Clear();
    }

    public void AddEffect(StackEffect effect, Player owner) {
        EffectsOnStack.Add(effect);
        _ = StartPriority(owner.PlayerNumber);
    }

    public void Clear() {
        EffectsOnStack = new Array<StackEffect>();
    }

    public void ResolveStack() {
        Console.Log("Resolving stack");
        var length = EffectsOnStack.Count;
        for (int i = length - 1; i >= 0 ; i--) {
            var stackEffect = EffectsOnStack[i];

            stackEffect.Resolve();
        }
    }
    
    public async Task StartPriority(int startPlayer) {
        var players = GameServer.ME.Players;
        var playersCount = players.Count;

        var max = startPlayer + playersCount;

        Console.Log("-----------------");

        // Iterate over all the players and ask for priority intention
        for (int i = startPlayer; i < max; i++) {
            var j = i;
            if (i >= playersCount) j -= playersCount;
            
            var player = GameServer.ME.Players[j];

            player.AskForPriorityIntention();
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
                bool hasActed = await player.GetPriority();

                if (hasActed) {
                    return;
                }
            }
        }

        ResolveStack();
    }
}
