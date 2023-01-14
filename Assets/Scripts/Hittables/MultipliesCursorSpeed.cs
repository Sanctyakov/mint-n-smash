using UnityEngine;

[CreateAssetMenu(fileName = "New MultipliesCursorSpeed", menuName = "MultipliesCursorSpeed")]

public class MultipliesCursorSpeed : Hittable
{
    public float mouseSpeedMultiplier, speedBoostDuration;

    public override void OnDead(GameController gc, Transform t)
    {
        gc.MultiplyMouseSpeed(mouseSpeedMultiplier, speedBoostDuration, t);
    }
}
