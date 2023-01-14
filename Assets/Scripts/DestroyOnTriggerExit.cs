using UnityEngine;

public class DestroyOnTriggerExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        HittableBehaviour hb = other.GetComponent<HittableBehaviour>();

        if (hb != null) hb.OnMiss();

        Destroy(other.gameObject);
    }
}
