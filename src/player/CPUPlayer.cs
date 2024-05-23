using Godot;
using System;
using System.Threading.Tasks;

public partial class CPUPlayer : OtherPlayer
{
    public override void AskForPriorityIntention() {
        base.AskForPriorityIntention();

        _ = RandomPriorityAfterRandomTime();

        
    }

    async Task RandomPriorityAfterRandomTime() {
        var rng = new RandomNumberGenerator();

        var time = rng.RandfRange(0f, 0f);
        var decision = rng.RandiRange(0, 1);

        await ToSignal(GetTree().CreateTimer(time), SceneTreeTimer.SignalName.Timeout);

        if (decision == 0) {
            PriorityIntentionNo();
        }
        else {
            PriorityIntentionYes();
        }
    }
}
