
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public float radius = 3f;
     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
    