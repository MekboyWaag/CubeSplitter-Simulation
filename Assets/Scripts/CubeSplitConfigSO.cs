using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeSplitConfig", menuName = "Systems/Cube Split Config")]
public class CubeSplitConfigSO : ScriptableObject
{
    [field: Header("Spawn Settings")]
    [field: SerializeField] public int MinCubes { get; private set; } = 2;
    [field: SerializeField] public int MaxCubes { get; private set; } = 6;
    [field: SerializeField] public float ScaleMultiplier { get; private set; } = 0.5f;
    [field: SerializeField] public float SplitChanceDecay { get; private set; } = 0.5f;

    [field: Header("Physics Settings")]
    [field: SerializeField] public float ExplosionForce { get; private set; } = 500f;
    [field: SerializeField] public float ExplosionRadius { get; private set; } = 5f;
    [field: SerializeField] public float SpawnScatterRadius { get; private set; } = 0.25f;

    [Header("Colors")]
    [SerializeField] private Color[] _availableColors;

    public IReadOnlyList<Color> AvailableColors => _availableColors;
}