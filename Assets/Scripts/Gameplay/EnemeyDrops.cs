using UnityEngine;

public class EnemeyDrops : MonoBehaviour
{
    public enum State { ON, OFF, RUNNING }
    private State _state = State.OFF;

    public Sprite[] Sprites;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Inventory _playerResources;
    private Collider2D _collider;
    private readonly float _noBehaviourTime = 3f;
    private float _behaviourCountdown;

    private void Start()
    {
        _playerResources = GameObject.FindGameObjectWithTag("PlayerResources").GetComponent<Inventory>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _behaviourCountdown = _noBehaviourTime;
        if (Sprites.Length > 0)
        {
            _spriteRenderer.sprite = ChangeSprite();
        }
        _state = State.ON;
    }

    private void Update()
    {
        if (_state == State.ON)
        {
            if (_behaviourCountdown <= 0)
            {
                _rigidbody.gravityScale = 0;
                _collider.isTrigger = true;
                _state = State.RUNNING;
            }
            else
                _behaviourCountdown -= Time.deltaTime;
        }
        if (_state == State.RUNNING)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _playerResources.transform.position, 5 * Time.deltaTime);
        }
    }

    private Sprite ChangeSprite()
    {
        if (Sprites.Length > 0)
        {
            int spritePos = Random.Range(0, Sprites.Length - 1);
            return Sprites[spritePos];
        }
        else
            return null;
    }
}
