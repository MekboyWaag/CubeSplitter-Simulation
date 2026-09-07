using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer), typeof(BoxCollider))]
public class SplittableCube : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _propBlock;

    public event Action<SplittableCube> OnClicked;

    public float CurrentSplitChance { get; private set; } = 1.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void TriggerClick()
    {
        OnClicked?.Invoke(this);
    }

    public void AddExplosiveForce(Vector3 explosionCenter, float force, float radius)
    {
        _rigidbody.AddExplosionForce(force, explosionCenter, radius, 1f, ForceMode.Impulse);
    }
}