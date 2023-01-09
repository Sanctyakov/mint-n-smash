public class FixesNextSpawns : Smashable
{
    public int spawnsAffected;
    public Smashable smashableToSpawn;

    public override void OnCompletelySmashed()
    {
        gc.FixNextSpawns(spawnsAffected, smashableToSpawn, transform);
    }
}
