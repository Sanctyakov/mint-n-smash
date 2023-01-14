using UnityEngine;

[CreateAssetMenu(fileName = "New FixesNextSpawns", menuName = "FixesNextSpawns")]
public class FixesNextSpawns : Hittable
{
    public int amountOfFixedSpawns, pointsOnMiss;
    public GameObject hittableToSpawn;

    public override void OnDead(GameController gc, Transform t)
    {
        gc.FixNextSpawns(amountOfFixedSpawns, hittableToSpawn, t);
    }

    public override void OnMiss(GameController gc, Transform t)
    {
        if (pointsOnMiss < 0) gc.UpdatePoints(pointsOnMiss, t);
    }
}
