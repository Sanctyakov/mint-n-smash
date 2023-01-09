using UnityEngine;

public class LosePointsOnMiss : MonoBehaviour
{
    public int pointsLostOnMiss;

    public void LosePoints()
    {
        Smashable s = this.GetComponent<Smashable>();

        s.gc.LosePoints(pointsLostOnMiss, transform);
    }
}
