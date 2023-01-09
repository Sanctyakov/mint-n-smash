public class LosePointsOnHit : Smashable
{
    public int pointsLostOnHit;

    public override void OnCompletelySmashed()
    {
        gc.LosePoints(pointsLostOnHit, transform);
    }
}
