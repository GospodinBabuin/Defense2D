using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Buildings : MonoBehaviour
{
    public string buildingType;

    public float PositionX;
    public float PositionY;

    public float BuildingArea;
    public int Health;

    [SerializeField] private int _maxHealth;
    private bool _isDestroyed = false;
    public int BuildingLevel = 1;

    public int _nextLvlCost = 25;
    public int _repairCost;
    public int _moneyCollected;

    [SerializeField] private bool _canFarmMoney;
    [SerializeField] private float _addMoneyTime;
    [SerializeField] private int _moneyPerTime;

    public Chest _baseChest;
    [SerializeField] private GameObject _chest;
    [SerializeField] private GameObject _coin;

    private Rigidbody2D _rigidbody;

    public float RadiusOfInteraction;
    [SerializeField] private LayerMask _playerLayer;

    public GameObject _lvl1Objects;
    public GameObject _lvl2Objects;
    public GameObject _lvl3Objects;
    private GameObject _buttons;

    [SerializeField] private AudioSource spawnSound;
    [SerializeField] private AudioSource disappearanceSound;
    [SerializeField] private AudioSource repairSound;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _lvl1Objects = GetGameObjects("Lvl1"); if (_lvl1Objects != null) _lvl1Objects.SetActive(false);
        _lvl2Objects = GetGameObjects("Lvl2"); if (_lvl2Objects != null) _lvl2Objects.SetActive(false);
        _lvl3Objects = GetGameObjects("Lvl3"); if (_lvl3Objects != null) _lvl3Objects.SetActive(false);
        _buttons = GetGameObjects("CanvasForButtons"); if (_buttons != null) _buttons.SetActive(false);
        
        repairSound = gameObject.AddComponent<AudioSource>() as AudioSource;
        repairSound.clip = Resources.Load<AudioClip>("Audio/Effects/RepairBuilding");

        if (spawnSound != null)
            spawnSound.Play();
        
        SpawnBuildingObjects();

        if (buildingType != "Base")
            if (name != "BaseRoadLamp")
            {
                WorldManager.Instance.BuildingsList.Add(this);
                WorldManager.Instance.BuildingsPositions.Add(gameObject.transform);
            }
        if (buildingType != "RoadLamp")
            WorldManager.Instance.CheckBorders();

        if (name != "BaseRoadLamp")
            gameObject.name = buildingType;
        
        if(BuildingLevel > 1)
            _maxHealth = _maxHealth + (BuildingLevel * 5);
        Health = _maxHealth;
        _addMoneyTime = 5f;
        _moneyPerTime *= BuildingLevel;
    }
    private void FixedUpdate()
    {
        if (gameObject.CompareTag("Dead") == false)
        {
            if (_buttons != null)
            {
                if (Physics2D.OverlapCircleAll(transform.position, RadiusOfInteraction, _playerLayer).Length == 0)
                    _buttons.SetActive(false);
                else
                    _buttons.SetActive(true);
            }

            if (_canFarmMoney == true)
                FarmMoney();
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDestroyed != true)
        {
            Health -= damage;

            if (Health <= 0)
                DestroyBuilding();

            _repairCost = (_maxHealth - Health) * 5 * BuildingLevel;
        }
    }

    public void DestroyBuilding()
    {
        _isDestroyed = true;
        gameObject.tag = "Dead";
        
        if (disappearanceSound != null)
            disappearanceSound.Play();

        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        if (_buttons != null)
            _buttons.SetActive(false);
        if (gameObject.name == "Base")
        {
            WorldManager.Instance.baseDestroyedWindow.SetActive(true);
            WorldManager.Instance.ExitToMenu();
            return;
        }
        if (gameObject.name is "RoadLamp" or "RoadLamp(Clone)")
        {
            Destroy(gameObject.transform.Find("Light2D").GetComponentInChildren<Light2D>());
            Destroy(gameObject.transform.Find("Light2DSmall").GetComponentInChildren<Light2D>());
        }

        WorldManager.Instance.BuildingsPositions.Remove(gameObject.transform);
        WorldManager.Instance.BuildingsList.Remove(gameObject.GetComponent<Buildings>());
        WorldManager.Instance.CheckBorders();
        Destroy(gameObject, 10);
    }


    public void RepairBuilding()
    {
        if (Inventory.Instance.Money >= _repairCost && _repairCost != 0)
        {
            repairSound.Play();
            Health = _maxHealth;
            _repairCost = 0;
            Inventory.Instance.Money -= _repairCost;
        }
    }
    public void UpdateToNextLvl()
    {
        if (Inventory.Instance.Money >= _nextLvlCost && BuildingLevel < 3)
        {
            BuildingLevel++;
            _moneyPerTime *= BuildingLevel;
            _maxHealth += 5;
            SpawnBuildingObjects();
            if (spawnSound != null)
                spawnSound.Play();
            Inventory.Instance.Money -= _nextLvlCost;
            
            if (BuildingLevel == 3)
                _nextLvlCost = int.MaxValue;
            else
                _nextLvlCost *= 2;
        }
    }
    public void OpenUnitsMenu()
    {
        MenuUI.Instance.UnitSpawner = gameObject.GetComponent<Transform>();
        MenuUI.Instance._buildingLvl = BuildingLevel;
        MenuUI.Instance.ShowMenu(MenuUI.MenuState.UNITMENU);
    }
    public void OpenStatueMenu()
    {
        MenuUI.Instance._buildingLvl = BuildingLevel;
        MenuUI.Instance.ShowMenu(MenuUI.MenuState.STATUEMENU);
    }

    private void FarmMoney()
    {
        if (_addMoneyTime >= 0)
            _addMoneyTime -= Time.deltaTime;
        else
        {
            _moneyCollected += _moneyPerTime;
            _addMoneyTime = 5f;
        }
    }
    public void CollectMoney()
    {
        Debug.Log("Collect");
        if (_moneyCollected > 0)
        {
            while (_moneyCollected > 0)
            {
                if (!_chest)
                {
                    Instantiate(_coin, _baseChest.transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
                    _moneyCollected--;
                }
                else
                {
                    Instantiate(_coin, _chest.transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
                    _moneyCollected--;
                }
            }
        }
    }

    private void SpawnBuildingObjects()
    {
        if (BuildingLevel >= 1)
        {
            if (_lvl1Objects != null)
                _lvl1Objects.SetActive(true);
        }
        if (BuildingLevel >= 2)
        {
            if (_lvl2Objects != null)
                _lvl2Objects.SetActive(true);
        }
        if (BuildingLevel >= 3)
        {
            if (_lvl3Objects != null)
                _lvl3Objects.SetActive(true);
        }
    }

    //gets children from this gameobject
    private GameObject GetGameObjects(string name)
    {
        Transform[] childrens = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in childrens)
        {
            if (t.gameObject.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
    public void GetInfo()
    {
        InfoOnClick.instance.SelectTarget(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Spike"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().TakeDamage(collision.GetComponent<Enemy>().Health);
                DestroyBuilding();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, BuildingArea);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, RadiusOfInteraction);
    }
}
