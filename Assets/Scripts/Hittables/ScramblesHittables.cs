using UnityEngine;

[CreateAssetMenu(fileName = "New ScramblesHittables", menuName = "ScramblesHittables")]

public class ScramblesHittables : Hittable
{
    //public float scrambleProportion; // Currently scrambles all hittables.

    public override void OnDead(GameController gc, Transform t)
    {
        gc.ScrambleHittables(t);
    }
}
