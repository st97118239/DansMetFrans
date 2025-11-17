using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public float maxDistance;

    public float CheckDistance(Vector3 givenPos)
    {
        return Vector3.Distance(transform.position, givenPos);
    }
}
