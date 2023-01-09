public class ScramblesSmashables : Smashable
{
    //public float scrambleProportion;

    public override void OnCompletelySmashed()
    {
        gc.ScrambleSmashables(transform);
    }
}
