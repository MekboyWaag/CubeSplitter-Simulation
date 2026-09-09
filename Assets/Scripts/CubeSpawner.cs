using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private SplittableCube _cubePrefab;
    [SerializeField] private int _initialPoolCapacity = 100;

    private ObjectPool<SplittableCube> _pool;

    private readonly List<SplittableCube> _spawnedCubesCache = new List<SplittableCube>(10);

    private void Awake()
    {
        _pool = new ObjectPool<SplittableCube>(
            createFunc: CreateCube,
            actionOnGet: cube => cube.gameObject.SetActive(true),
            actionOnRelease: cube => cube.gameObject.SetActive(false),
            actionOnDestroy: cube =>
            {
                if (cube != null)
                {
                    cube.OnInteracted -= ReturnToPool;
                    Destroy(cube.gameObject);
                }
            },
            defaultCapacity: _initialPoolCapacity
        );
    }

    private void OnDestroy()
    {
        _pool?.Dispose();
    }

    public SplittableCube SpawnInitialCube(Vector3 position)
    {
        SplittableCube cube = _pool.Get();
        cube.Initialize(position, Vector3.one, Color.white, 1.0f);
        return cube;
    }

    public IReadOnlyList<SplittableCube> SpawnSplitCubes(SplittableCube parentCube, CubeSplitConfigSO config)
    {
        _spawnedCubesCache.Clear();

        int spawnCount = Random.Range(config.MinCubes, config.MaxCubes + 1);
        Vector3 newScale = parentCube.transform.localScale * config.ScaleMultiplier;
        float nextSplitChance = parentCube.CurrentSplitChance * config.SplitChanceDecay;

        for (int i = 0; i < spawnCount; i++)
        {
            SplittableCube newCube = _pool.Get();
            Vector3 offset = Random.insideUnitSphere * config.SpawnScatterRadius;

            Color randomColor = Color.white;
            if (config.AvailableColors != null && config.AvailableColors.Count > 0)
                randomColor = config.AvailableColors[Random.Range(0, config.AvailableColors.Count)];

            newCube.Initialize(parentCube.transform.position + offset, newScale, randomColor, nextSplitChance);
            _spawnedCubesCache.Add(newCube);
        }

        return _spawnedCubesCache;
    }

    private SplittableCube CreateCube()
    {
        SplittableCube cube = Instantiate(_cubePrefab);
        cube.OnInteracted += ReturnToPool;
        return cube;
    }

    private void ReturnToPool(SplittableCube cube)
    {
        _pool.Release(cube);
    }
}