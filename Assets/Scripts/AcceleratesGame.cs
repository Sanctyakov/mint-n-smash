public class AcceleratesGame : Smashable
{
    public float timeMultiplier, rewardMultiplier;

    public override void OnCompletelySmashed()
    {
        gc.AccelerateGame(timeMultiplier, rewardMultiplier, transform);
    }
}
