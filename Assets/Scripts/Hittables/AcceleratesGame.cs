using UnityEngine;

[CreateAssetMenu(fileName = "New AcceleratesGame", menuName = "AcceleratesGame")]

public class AcceleratesGame : Hittable
{
    public float timeMultiplier, rewardMultiplier;

    public override void OnDead(GameController gc, Transform t)
    {
        gc.AccelerateGame(timeMultiplier, rewardMultiplier, t);
    }
}
