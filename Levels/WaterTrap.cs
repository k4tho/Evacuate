using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterTrap : MonoBehaviour
{
    public IEnumerator KillAllZombies()
    {
        yield return new WaitForSeconds(1.5f);

        Collider col = GetComponent<Collider>();
        Vector3 center = col.bounds.center;
        Vector3 halfExtents = col.bounds.extents;

        // Get all colliders currently intersecting the box
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity);
        Debug.Log(colliders.Length);

        foreach (Collider cols in colliders)
        {
            ZombieHealth zombie = cols.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(200); // Replace this with your real function (e.g., TakeDamage())
            }
        }
    }
}
