using UnityEngine;
using UnityEngine.Pool;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private CubeSplitConfigSO _config;
    [SerializeField] private SplittableCube _cubePrefab;
    [SerializeField] private int _initialPoolCapacity = 100;

    private ObjectPool<SplittableCube> _pool;

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
                    cube.OnClicked -= HandleCubeClicked;
                    Destroy(cube.gameObject);
                }
            },
            defaultCapacity: _initialPoolCapacity
        );
    }

    private void Start()
    {
        SpawnCube(transform.position, Vector3.one, 1.0f);
    }

    private void OnDestroy()
    {
        _pool?.Dispose();
    }

    private SplittableCube CreateCube()
    {
        SplittableCube cube = Instantiate(_cubePrefab);
        cube.OnClicked += HandleCubeClicked;
        return cube;
    }

    private void HandleCubeClicked(SplittableCube clickedCube)
    {
        Vector3 originPosition = clickedCube.transform.position;
        Vector3 newScale = clickedCube.transform.localScale * _config.ScaleMultiplier;
        float nextSplitChance = clickedCube.CurrentSplitChance * _config.SplitChanceDecay;

        _pool.Release(clickedCube);

        if (UnityEngine.Random.value <= clickedCube.CurrentSplitChance)
        {
            int spawnCount = UnityEngine.Random.Range(_config.MinCubes, _config.MaxCubes + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * _config.SpawnScatterRadius;
                SplittableCube newCube = SpawnCube(originPosition + randomOffset, newScale, nextSplitChance);
                newCube.AddExplosiveForce(originPosition, _config.ExplosionForce, _config.ExplosionRadius);
            }
        }
    }

    private SplittableCube SpawnCube(Vector3 position, Vector3 scale, float splitChance)
    {
        SplittableCube cube = _pool.Get();
        Color cubeColor = Color.white;

        if (_config.AvailableColors != null && _config.AvailableColors.Count > 0)
        {
            cubeColor = _config.AvailableColors[UnityEngine.Random.Range(0, _config.AvailableColors.Count)];
        }
        else
        {
            Debug.LogWarning("CubeManager: Цвета не настроены в CubeSplitConfigSO!");
        }

        cube.Initialize(position, scale, cubeColor, splitChance);
        return cube;
    }
}