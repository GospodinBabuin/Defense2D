using UnityEngine;
public class MenuUI : MonoBehaviour
{
    public enum MenuState { UNITMENU, BUILDINGMENU, STATUEMENU, GAMEMENU, OFF }
    private MenuState _menuState = MenuState.OFF;

    public static MenuUI Instance { get; private set; }

    private GameObject _buildingMenu;
    private GameObject _unitMenu;
    private GameObject _gameMenu;
    private GameObject _statueMenu;
    
    public int _buildingLvl;
    public Transform UnitSpawner;
    private GameObject _slot2;
    private GameObject _slot3;
    private GameObject _healLvl2;
    private GameObject _healLvl3;
    private GameObject _speedLvl2;
    private GameObject _speedLvl3;
    private GameObject _damageLvl2;
    private GameObject _damageLvl3;

    private PlayerController player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _buildingMenu = GameObject.Find("BuildingMenu");
        _unitMenu = GameObject.Find("UnitMenu");
        _gameMenu = GameObject.Find("GameMenu");
        _statueMenu = GameObject.Find("StatueMenu");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        
        _healLvl2 = GameObject.Find("HealSlot2");
        _healLvl3 = GameObject.Find("HealSlot3");
        _speedLvl2 = GameObject.Find("SpeedSlot1");
        _speedLvl3 = GameObject.Find("SpeedSlot2");
        _damageLvl2 = GameObject.Find("DamageSlot1");
        _damageLvl3 = GameObject.Find("DamageSlot2");
        _healLvl2.SetActive(false);
        _healLvl3.SetActive(false);
        _speedLvl2.SetActive(false);
        _speedLvl3.SetActive(false);
        _damageLvl2.SetActive(false);
        _damageLvl3.SetActive(false);

        _slot2 = GameObject.Find("UnitSlot2");
        _slot3 = GameObject.Find("UnitSlot3");
        _slot2.SetActive(false);
        _slot3.SetActive(false);

        _buildingMenu.SetActive(false);
        _unitMenu.SetActive(false);
        _gameMenu.SetActive(false);
        _statueMenu.SetActive(false);
        _menuState = MenuState.OFF;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _menuState != MenuState.OFF && !InfoOnClick.instance._infoWindow.activeInHierarchy)
            CloseMenu();
    }
    public void ShowMenu(MenuState state)
    {
        switch (state)
        {
            case MenuState.BUILDINGMENU:
                ShowBuildingMenu();
                break;
            case MenuState.UNITMENU:
                ShowUnitsMenu();
                break;
            case MenuState.STATUEMENU:
                ShowStatueMenu();
                break;
            case MenuState.GAMEMENU:
                ShowGameMenu();
                break;
        }
    }
    public void ShowBuildingMenu()
    {
        if (_menuState != MenuState.BUILDINGMENU)
        {
            if (_unitMenu.activeInHierarchy)
                _unitMenu.SetActive(false);
            if (_statueMenu.activeInHierarchy)
                _statueMenu.SetActive(false);
            if (_gameMenu.activeInHierarchy)
                _gameMenu.SetActive(false);

            _buildingMenu.SetActive(true);
            _menuState = MenuState.BUILDINGMENU;

            if (Inventory.Instance.isClosed == true)
            {
                Inventory.Instance._bannerState = Inventory.BannerState.OFF;
                Inventory.Instance.ShowOrHideResources();
            }
        }
        else
            CloseMenu();
    }
    public void ShowUnitsMenu()
    {
        if (_menuState != MenuState.UNITMENU)
        {
            if (_buildingMenu.activeInHierarchy)
                _buildingMenu.SetActive(false);
            if (_statueMenu.activeInHierarchy)
                _statueMenu.SetActive(false);
            if (_gameMenu.activeInHierarchy)
                _gameMenu.SetActive(false);

            _unitMenu.SetActive(true);
            _menuState = MenuState.UNITMENU;

            if (_buildingLvl >= 2)
                _slot2.SetActive(true);
            if (_buildingLvl == 3)
                _slot3.SetActive(true);

            if (Inventory.Instance.isClosed == true)
            {
                Inventory.Instance._bannerState = Inventory.BannerState.OFF;
                Inventory.Instance.ShowOrHideResources();
            }
        }
        else
            CloseMenu();
    }
    public void ShowStatueMenu()
    {
        if (_menuState != MenuState.STATUEMENU)
        {
            if (_buildingMenu.activeInHierarchy)
                _buildingMenu.SetActive(false);
            if (_unitMenu.activeInHierarchy)
                _unitMenu.SetActive(false);
            if (_gameMenu.activeInHierarchy)
                _gameMenu.SetActive(false);

            _statueMenu.SetActive(true);
            _menuState = MenuState.STATUEMENU;

            if (_buildingLvl >= 2)
            {
                if (player.maxHeath < 15)
                    _healLvl2.SetActive(true);
                if (player.Speed < 3.75f)
                    _speedLvl2.SetActive(true);
                if (player.AttackDamage < 2)
                    _damageLvl2.SetActive(true);
            }

            if (_buildingLvl == 3)
            {
                if (player.maxHeath < 20)
                    _healLvl3.SetActive(true);
                if (player.Speed < 4.5f)
                    _speedLvl3.SetActive(true);
                if (player.AttackDamage < 3)
                    _damageLvl3.SetActive(true);
            }
            if (Inventory.Instance.isClosed == true)
            {
                Inventory.Instance._bannerState = Inventory.BannerState.OFF;
                Inventory.Instance.ShowOrHideResources();
            }
        }
        else
            CloseMenu();
    }
    public void ShowGameMenu()
    {
        if (_menuState != MenuState.GAMEMENU)
        {
            if (_buildingMenu.activeInHierarchy)
                _buildingMenu.SetActive(false);
            if (_unitMenu.activeInHierarchy)
                _unitMenu.SetActive(false);
            if (_statueMenu.activeInHierarchy)
                _statueMenu.SetActive(false);

            _gameMenu.SetActive(true);
            _menuState = MenuState.GAMEMENU;

            if (Inventory.Instance.isClosed == true)
            {
                Inventory.Instance._bannerState = Inventory.BannerState.OFF;
                Inventory.Instance.ShowOrHideResources();
            }
        }
        else
            CloseMenu();
    }
    public void CloseMenu()
    {
        if (_buildingMenu.activeInHierarchy)
            _buildingMenu.SetActive(false);
        
        if (_unitMenu.activeInHierarchy)
        {
            _slot2.SetActive(false);
            _slot3.SetActive(false);
            _unitMenu.SetActive(false);
            UnitSpawner = null;
            _buildingLvl = 1;
        }

        if (_statueMenu.activeInHierarchy)
        {
            _healLvl2.SetActive(false);
            _healLvl3.SetActive(false);
            _speedLvl2.SetActive(false);
            _speedLvl3.SetActive(false);
            _damageLvl2.SetActive(false);
            _damageLvl3.SetActive(false);
            _statueMenu.SetActive(false);
            _buildingLvl = 1;
        }

        if (_gameMenu.activeInHierarchy)
            _gameMenu.SetActive(false);
        
        _menuState = MenuState.OFF;
            
        if (Inventory.Instance.isClosed == false)
        {
            Inventory.Instance._bannerState = Inventory.BannerState.ON;
            Inventory.Instance.ShowOrHideResources();
        }
    }
}
