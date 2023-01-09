public class AwardsPoints : Smashable
{
    public int pointsAwarded;

    public override void OnCompletelySmashed()
    {
        gc.AwardPoints(pointsAwarded, transform);
    }
}
