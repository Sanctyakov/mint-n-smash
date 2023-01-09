using UnityEngine;

public class DestroyOnTriggerExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        LosePointsOnMiss lpom = other.GetComponent<LosePointsOnMiss>();

        if (lpom != null) lpom.LosePoints();

        Destroy(other.gameObject);
    }
}
