using UnityEngine;

public class CubeSplitFlow : MonoBehaviour
{
    private const float MinSafeScale = 0.01f;
    private const float MaxExplosionMultiplier = 100f;

    [Header("Dependencies")]
    [SerializeField] private CubeSplitConfigSO _config;
    [SerializeField] private CubeRaycaster _raycaster;
    [SerializeField] private CubeSpawner _spawner;
    [SerializeField] private CubeExploder _exploder;

    [Header("Level Design")]
    [SerializeField] private Transform[] _initialSpawnPoints;

    private void Start()
    {
        if (_initialSpawnPoints == null || _initialSpawnPoints.Length == 0)
        {
            _spawner.SpawnInitialCube(transform.position);
            return;
        }

        foreach (var spawnPoint in _initialSpawnPoints)
        {
            _spawner.SpawnInitialCube(spawnPoint.position);
        }
    }

    private void OnEnable() => _raycaster.OnCubeHit += ProcessCubeHit;
    private void OnDisable() => _raycaster.OnCubeHit -= ProcessCubeHit;

    private void ProcessCubeHit(SplittableCube targetCube)
    {
        if (targetCube == null) return;

        if (Random.value <= targetCube.CurrentSplitChance)
        {
            var newCubes = _spawner.SpawnSplitCubes(targetCube, _config);
            _exploder.ApplyExplosion(newCubes, targetCube.transform.position, _config.ExplosionForce, _config.ExplosionRadius);
        }
        else
        {
            float scaleX = targetCube.transform.localScale.x;
            float inverseScaleMultiplier = scaleX > MinSafeScale ? (1f / scaleX) : MaxExplosionMultiplier;
            float finalForce = _config.ExplosionForce * inverseScaleMultiplier;
            float finalRadius = _config.ExplosionRadius * inverseScaleMultiplier;

            _exploder.ApplyAreaExplosion(targetCube.transform.position, finalForce, finalRadius);
        }

        targetCube.Interact();
    }
}