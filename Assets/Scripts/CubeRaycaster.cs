using System;
using UnityEngine;

public class CubeRaycaster : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Raycast Settings")]
    [SerializeField, Min(1f)] private float _maxRayDistance = 100f;

    public event Action<SplittableCube> OnCubeHit;

    private void OnEnable() => _input.OnPointerDown += HandlePointerDown;
    private void OnDisable() => _input.OnPointerDown -= HandlePointerDown;

    private void HandlePointerDown(Vector3 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance, _targetLayer))
        {
            if (hit.collider.TryGetComponent(out SplittableCube cube))
            {
                OnCubeHit?.Invoke(cube);
            }
        }
    }
}