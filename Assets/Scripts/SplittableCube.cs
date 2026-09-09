using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer), typeof(BoxCollider))]
public class SplittableCube : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _propBlock;

    public event Action<SplittableCube> OnInteracted;

    public Rigidbody Rigidbody { get; private set; }
    public float CurrentSplitChance { get; private set; } = 1.0f;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    public void Initialize(Vector3 position, Vector3 scale, Color color, float splitChance)
    {
        transform.position = position;
        transform.localScale = scale;
        CurrentSplitChance = splitChance;

        _meshRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(ColorProperty, color);
        _meshRenderer.SetPropertyBlock(_propBlock);

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    public void Interact()
    {
        OnInteracted?.Invoke(this);
    }
}