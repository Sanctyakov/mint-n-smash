public class BlocksClicks : Smashable
{
    //public float secondsBlocked;

    public override void OnCompletelySmashed()
    {
        gc.BlockClicks(/*secondsBlocked,*/ transform);
    }
}
