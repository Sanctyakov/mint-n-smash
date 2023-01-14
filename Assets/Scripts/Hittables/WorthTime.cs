using UnityEngine;

[CreateAssetMenu(fileName = "New WorthTime", menuName = "WorthTime")]

public class WorthTime : Hittable
{
    public float timeOnDead, extraTime, extraTimeRate;
    public override void OnDead(GameController gc, Transform t)
    {
        gc.UpdateTimer(timeOnDead, extraTime, extraTimeRate, t);
    }
}
