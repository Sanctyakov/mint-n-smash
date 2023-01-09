public class AwardsTime : Smashable
{
    public float timeAwarded, extraTime, extraTimeRate;
    public override void OnCompletelySmashed()
    {
        gc.AwardTime(timeAwarded, extraTime, extraTimeRate, transform);
    }
}
