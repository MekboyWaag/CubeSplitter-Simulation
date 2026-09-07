using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _cubeLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
    }

    private void HandleClick(Vector3 mousePosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _cubeLayer))
        {
            if (hit.collider.TryGetComponent(out SplittableCube cube))
            {
                cube.TriggerClick();
            }
        }
    }
}