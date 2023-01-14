using UnityEngine;

[CreateAssetMenu(fileName = "New WorthRandomPoints", menuName = "WorthRandomPoints")]

public class WorthRandomPoints : WorthPoints
{
    public int randomRangeMinInclusive, randomRangeMaxExclusive;

    public override void SetUp () // Called by HittableBehaviour on Awake.
    {
        pointsOnDead = Random.Range(randomRangeMinInclusive, randomRangeMaxExclusive);

        Debug.Log(pointsOnDead);
    }
}
