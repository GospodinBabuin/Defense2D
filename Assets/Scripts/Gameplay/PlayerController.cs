public class PlayerController : Unit
{
    [SerializeField] private float movementDirectory;
    public GameObject ObjSpanwer;
    public GameObject prefab;
    private HealthBar _healthIcon;
    public int maxHeath;

    private InputActions _input;

    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _healthIcon = GameObject.Find("PlayerHealth").GetComponent<HealthBar>();
        _healthIcon.SetMaxHealth(Health);
    }

    private void Update()
    {
        if (IsDead == true)
            return;

        movementDirectory = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movementDirectory, 0, 0) * Time.deltaTime * Speed;

        Animator.SetFloat("Speed", Mathf.Abs(movementDirectory));

        if (!Mathf.Approximately(0, movementDirectory))
            transform.rotation = movementDirectory < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        if (Input.GetKeyUp(KeyCode.Space) && Time.time >= NextAttackTime)
        {
            Attack();
            NextAttackTime = Time.time + 1f / AttackRate;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            MenuUI.Instance.ShowMenu(MenuUI.MenuState.BUILDINGMENU);
    }

    public void RestoreHealth(int health)
    {
        Health += health;
        if (Health > maxHeath)
            Health = maxHeath;
        _healthIcon.SetHealth(Health);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHeath = newMaxHealth;
        _healthIcon.SetMaxHealth(maxHeath);
    }
    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }
    public void SetDamage(int newDamage)
    {
        AttackDamage = newDamage;
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
