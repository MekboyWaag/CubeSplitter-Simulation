using UnityEngine;

public class CubeSplitFlow : MonoBehaviour
{
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
        if (Random.value <= targetCube.CurrentSplitChance)
        {
            var newCubes = _spawner.SpawnSplitCubes(targetCube, _config);

            _exploder.ApplyExplosion(newCubes, targetCube.transform.position, _config.ExplosionForce, _config.ExplosionRadius);
        }

        targetCube.Interact();
    }
}