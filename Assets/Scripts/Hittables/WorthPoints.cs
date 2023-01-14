using UnityEngine;

[CreateAssetMenu(fileName = "New WorthPoints", menuName = "WorthPoints")]
public class WorthPoints : Hittable
{
    public int pointsOnDead, pointsOnMiss;

    public override void OnDead(GameController gc, Transform t)
    {
        gc.UpdatePoints(pointsOnDead, t);
    }

    public override void OnMiss(GameController gc, Transform t)
    {
        if (pointsOnMiss < 0) gc.UpdatePoints(pointsOnMiss, t);
    }
}
