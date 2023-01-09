public class MultipliesMouseSpeed : Smashable
{
    public float mouseSpeedMultiplier, speedBoostDuration;

    public override void OnCompletelySmashed()
    {
        gc.MultiplyMouseSpeed(mouseSpeedMultiplier, speedBoostDuration, transform);
    }
}
