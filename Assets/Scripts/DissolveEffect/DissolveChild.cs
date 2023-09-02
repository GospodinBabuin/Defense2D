using UnityEngine;

public class DissolveChild : MonoBehaviour
{
    private GameObject _go;
    private Color _color;
    private SpriteRenderer[] _spriteRenderer;

    private Material _material;
    private Material _originalMaterial;
    private float _fade;
    private bool _materialIsUsing = false;
    private bool _IsSpawning = true;

    private void Awake()
    {
        _go = this.gameObject;
        _color = GetComponentInParent<Dissolve>()._color;
        _material = Resources.Load<Material>("Tiles/Dissolve");
        _material.SetColor("_Color", _color);
        _spriteRenderer = _go.GetComponentsInChildren<SpriteRenderer>();
        _originalMaterial = _go.GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (_IsSpawning && transform.parent.CompareTag("Dead") == false)
        {
            SpawnMethod();
        }
    }

    private void SpawnMethod()
    {
        if (_materialIsUsing == false)
        {
            foreach (SpriteRenderer item in _spriteRenderer)
                item.material = _material;

            _fade = 0f;

            _materialIsUsing = true;
        }

        _fade += Time.deltaTime;

        foreach (SpriteRenderer item in _spriteRenderer)
            item.material.SetFloat("_Fade", _fade);

        if (_fade >= 1f)
        {
            foreach (SpriteRenderer item in _spriteRenderer)
                item.material.SetFloat("_Fade", 1f);

            _IsSpawning = false;

            foreach (SpriteRenderer item in _spriteRenderer)
                item.material = _originalMaterial;

            _materialIsUsing = false;
        }
    }
}
