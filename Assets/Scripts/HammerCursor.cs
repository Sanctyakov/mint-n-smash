using UnityEngine;

public class HammerCursor : MonoBehaviour
{
    public Animator hammerAnimator;
    public AudioSource woosh;
    public float xOffset, yOffset, zOffset;
    public float speed;
    private float originalSpeed, speedBoostCounter, speedBoostDuration;
    private bool speedUp;

    private void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, Camera.main.nearClipPlane + zOffset));
        transform.position = Vector3.MoveTowards(transform.position, worldPosition, speed * Time.deltaTime);

        transform.position = new Vector3(
         Mathf.Clamp(transform.position.x, -2.5f, 2.5f),
         Mathf.Clamp(transform.position.y, -1.5f, 1.0f),
         transform.position.z);

        if (Input.GetMouseButtonDown(0))
        {
            hammerAnimator.SetTrigger("Smash");
            woosh.Play();
        }

        if(speedUp)
        {
            speedBoostCounter += Time.deltaTime;

            if (speedBoostCounter >= speedBoostDuration)
            {
                speedUp = false;
                speed = originalSpeed;
                speedBoostCounter = 0;
            }
        }
    }

    public void ChangeSpeed (float speedMultiplier, float duration)
    {
        speed *= speedMultiplier; // You can increase mouse speed all you want, it's never gonna be faster than the real mouse cursor.
        speedBoostDuration = duration;
        speedBoostCounter = 0;
        speedUp = true;
    }
}
