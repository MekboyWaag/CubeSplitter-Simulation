using System.Collections.Generic;
using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    public void ApplyExplosion(IReadOnlyList<SplittableCube> targetCubes, Vector3 center, float force, float radius)
    {
        for (int i = 0; i < targetCubes.Count; i++)
        {
            targetCubes[i].Rigidbody.AddExplosionForce(force, center, radius, 1f, ForceMode.Impulse);
        }
    }
}