using UnityEngine;

[CreateAssetMenu(fileName = "New BlocksClicks", menuName = "BlocksClicks")]
public class BlocksClicks : Hittable
{
    //public float secondsBlocked; // Currently set by animation.
    public override void OnDead(GameController gc, Transform t)
    {
        gc.BlockClicks(/*secondsBlocked,*/t);
    }
}
