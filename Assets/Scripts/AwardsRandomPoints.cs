using UnityEngine;

public class AwardsRandomPoints : AwardsPoints
{
    public int randomRangeMinInclusive, randomRangeMaxExclusive;

    private void Start ()
    {
        pointsAwarded = Random.Range(randomRangeMinInclusive, randomRangeMaxExclusive);
    }
}
