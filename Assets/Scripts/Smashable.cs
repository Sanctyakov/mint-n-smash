using UnityEngine;

public abstract class Smashable : MonoBehaviour
{
    public int health;
    public AudioSource clink;
    public Animator animator;
    private Rigidbody rb;
    [HideInInspector] public GameController gc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.angularVelocity = Random.insideUnitSphere;

        gc = FindObjectOfType<GameController>();
    }

    public void Smashed()
    {
        health--;

        clink.Play();

        if (health <= 0)
        {
            OnCompletelySmashed();

            LosePointsOnMiss lpom = GetComponent<LosePointsOnMiss>();

            if (lpom != null) Destroy(lpom);

            Destroy(this);

            //rb.rotation = Quaternion.identity;

            rb.transform.localScale = new Vector3 (rb.transform.localScale.x, rb.transform.localScale.y, rb.transform.localScale.z / 5);
        }

        rb.angularVelocity = Random.insideUnitSphere * 10;
    }

    public abstract void OnCompletelySmashed();
}
