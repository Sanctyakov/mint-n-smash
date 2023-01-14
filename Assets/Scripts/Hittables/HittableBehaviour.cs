using UnityEngine;

public class HittableBehaviour : MonoBehaviour
{
    public Hittable hittable;
    private int health;
    private AudioSource audioSource;
    private Rigidbody rigidBody;
    private GameController gc;

    private void Awake()
    {
        hittable.SetUp();

        health = hittable.health;

        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = hittable.audioClip;

        rigidBody = gameObject.AddComponent<Rigidbody>();

        rigidBody.angularVelocity = Random.insideUnitSphere;

        gc = FindObjectOfType<GameController>();

        if (gc == null) Debug.LogError("GameController not found");
    }

    public void Hit()
    {
        health--;

        audioSource.Play();

        rigidBody.angularVelocity = Random.insideUnitSphere * 10;

        gc.SpawnRandomSparks(transform);

        if (health <= 0)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        hittable.OnDead(gc, transform);

        rigidBody.rotation = Quaternion.identity;

        rigidBody.transform.localScale = new Vector3(rigidBody.transform.localScale.x, rigidBody.transform.localScale.y, rigidBody.transform.localScale.z / 5);

        Destroy(this);
    }

    public void OnMiss()
    {
        hittable.OnMiss(gc, transform);
    }
}
