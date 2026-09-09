using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> OnPointerDown;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        OnPointerDown?.Invoke(Input.mousePosition);
    }
}