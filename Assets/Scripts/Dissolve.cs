using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private GameObject go;
    public Color _color;
    [SerializeField] private SpriteRenderer[] _spriteRenderer;
    public Material _material;
    public Material _originalMaterial;
    [SerializeField] private float _fade;
    [SerializeField] private bool _inProcess = true;
    [SerializeField] private bool _materialIsUsing = false;
    [SerializeField] private bool _IsSpawning = true;

    private void Awake()
    {
        go = this.gameObject;
        _material = Resources.Load<Material>("Tiles/Dissolve");
        if (go.GetComponentInChildren<SpriteRenderer>() != null)
        {
            _spriteRenderer = go.GetComponentsInChildren<SpriteRenderer>();
            _originalMaterial = go.GetComponentInChildren<SpriteRenderer>().material;
        }

        if (gameObject.CompareTag("Enemy"))
            _color = Color.red;
        if (gameObject.CompareTag("Allies"))
            _color = Color.magenta;
        if (gameObject.layer == LayerMask.NameToLayer("Allies"))
            _color = Color.yellow;
        if (gameObject.layer == LayerMask.NameToLayer("Building"))
            _color = Color.cyan;

        _material.SetColor("_Color", _color);
    }

    private void Update()
    {
        if (_spriteRenderer != null)
        {
            if (go.CompareTag("Dead") && _inProcess)
            {
                DissolveMethod();
            }

            if (_IsSpawning)
            {
                SpawnMethod();
            }
        }
    }

    private void DissolveMethod()
    {

        if (_materialIsUsing == false)
        {
            foreach (SpriteRenderer item in _spriteRenderer)
                item.material = _material;

            _fade = 1f;

            _materialIsUsing = true;
        }

        _fade -= Time.deltaTime / 5f;

        foreach (SpriteRenderer item in _spriteRenderer)
            item.material.SetFloat("_Fade", _fade);

        if (_fade <= 0f)
        {
            foreach (SpriteRenderer item in _spriteRenderer)
                item.material.SetFloat("_Fade", 0f);

            _inProcess = false;
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
