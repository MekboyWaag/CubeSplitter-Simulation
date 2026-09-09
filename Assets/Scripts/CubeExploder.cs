using System.Collections.Generic;
using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private LayerMask _cubeLayer;

    private readonly Collider[] _collidersCache = new Collider[64];

    public void ApplyExplosion(IReadOnlyList<SplittableCube> targetCubes, Vector3 center, float force, float radius)
    {
        for (int i = 0; i < targetCubes.Count; i++)
        {
            if (targetCubes[i].Rigidbody == null)
                continue;

            targetCubes[i].Rigidbody.AddExplosionForce(force, center, radius, 1f, ForceMode.Impulse);
        }
    }

    public void ApplyAreaExplosion(Vector3 center, float force, float radius)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(center, radius, _collidersCache, _cubeLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (!_collidersCache[i].TryGetComponent(out Rigidbody rigidbody))
                continue;

            rigidbody.AddExplosionForce(force, center, radius, 1f, ForceMode.Impulse);
        }
    }
}