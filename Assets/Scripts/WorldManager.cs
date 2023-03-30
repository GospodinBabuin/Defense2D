using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    private Storage storage;
    private GameData gameData;
    private PlayerController player;
    private GameObject _saveGameWindow;
    public GameObject baseDestroyedWindow;

    public enum DayState { DAY, EVENING, NIGHT }
    public DayState dayState;

    public int DayCount;
    public float CurrentTime;
    public Text DayCountText;

    [SerializeField] private Buildings _base;
    private enum DayCounterState { SHOW, HIDE, OFF };
    [SerializeField] private DayCounterState dayCounterState;
    public List<Transform> BuildingsPositions = new List<Transform>();
    public GameObject LeftBorder;
    public GameObject RightBorder;
    public Vector3 MiddleOfTheBorders;

    public SpriteRenderer[] _beams;
    public Light2D GlobalLight;
    private bool _globalLightIntensityOnPosition = false;

    public List<Buildings> BuildingsList;
    public List<AlliesSolders> AlliesSoldersList;
    public List<Enemy> EnemyList;
    public bool CanCheckIsMonstersDead = false;

    private void Awake()
    {
        Instance = this;

        LeftBorder = GameObject.Find("GreenZoneBorderL");
        RightBorder = GameObject.Find("GreenZoneBorderR");
        
        BuildingsList = new List<Buildings>();
        AlliesSoldersList = new List<AlliesSolders>();
        EnemyList = new List<Enemy>();
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        _saveGameWindow = GameObject.Find("GameSaveWindow");
        baseDestroyedWindow = GameObject.Find("BaseDestroyedWindow");
        _saveGameWindow.SetActive(false);
        baseDestroyedWindow.SetActive(false);
        storage = new Storage();
        Load();
        
        DailyChest.SpawnChest();
        
        DayCountText.text = DayCount.ToString();

        CurrentTime = 0f;
        dayState = DayState.DAY;
        dayCounterState = DayCounterState.SHOW;
        StartCoroutine(ShowDayCount());
    }

    private void FixedUpdate()
    {
        ChangeDayTime();
    }

    private void Save()
    {
        gameData.day = DayCount;
        gameData.maxHealth = player.maxHeath;
        gameData.health = player.Health;
        gameData.speed = player.Speed;
        gameData.damage = player.AttackDamage;
        gameData.money = GameObject.Find("PlayerResouces").GetComponent<Inventory>().Money;
        gameData.buildings = BuildingsList;
        gameData.solders = AlliesSoldersList;
        gameData.waveType = WaveSpawner.Instance.waveType;

        storage.SaveGame(gameData);
        StartCoroutine(ShowSaveGameWindow());
        Debug.Log("GameSaved");
    }

    private IEnumerator ShowSaveGameWindow()
    {
        _saveGameWindow.SetActive(true);
        yield return new WaitForSeconds(3);
        _saveGameWindow.SetActive(false);
        yield break;
    }
    public void ExitToMenu()
    {
        StartCoroutine(ExitToMenuCoroutine());
    }
    private IEnumerator ExitToMenuCoroutine()
    {
        yield return new WaitForSeconds(10);
        if (File.Exists(Application.persistentDataPath + "/GameSave.save"))
            File.Delete(Application.persistentDataPath + "/GameSave.save");
        GameObject.Find("LoadScene").GetComponent<LoadScenes>().LoadScene("Main menu");
    }

    private void Load()
    {
        gameData = (GameData)storage.LoadGame(new GameData());

        player.maxHeath = gameData.maxHealth;
        player.Health = gameData.health;
        player.Speed = gameData.speed;
        player.AttackDamage = gameData.damage;
        GameObject.Find("PlayerResouces").GetComponent<Inventory>().Money = gameData.money;
        WaveSpawner.Instance.waveType = gameData.waveType;
        DayCount = gameData.day;

        if (gameData.solders != null && gameData.solders.Count > 0)
        {
            for (int i = 0; i < gameData.solders.Count; i++)
            {
                switch (gameData.solders[i].Name)
                {
                    case "WarriorLvl1":
                        Instantiate(Resources.Load<AlliesSolders>("WarriorLvl1")).Health = gameData.solders[i].Health;
                        break;
                    case "WarriorLvl2":
                        Instantiate(Resources.Load<AlliesSolders>("WarriorLvl2")).Health = gameData.solders[i].Health;
                        break;
                    case "WarriorLvl3":
                        Instantiate(Resources.Load<AlliesSolders>("WarriorLvl3")).Health = gameData.solders[i].Health;
                        break;
                }
            }
        }

        if (gameData.buildings != null && gameData.buildings.Count > 0)
        {
            for (int i = 0; i < gameData.buildings.Count; i++)
            {
                switch (gameData.buildings[i].buildingType)
                {
                    case "Barricade":
                        Instantiate(Resources.Load<Buildings>("Buildings/Barricade"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "RoadLamp":
                        Instantiate(Resources.Load<Buildings>("Buildings/RoadLamp"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "Scarecrow":
                        Instantiate(Resources.Load<Buildings>("Buildings/Scarecrow"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "Spike":
                        Instantiate(Resources.Load<Buildings>("Buildings/Spike"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "Stall":
                        Instantiate(Resources.Load<Buildings>("Buildings/Stall"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "Statue":
                        Instantiate(Resources.Load<Buildings>("Buildings/Statue"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "WeaponRack":
                        Instantiate(Resources.Load<Buildings>("Buildings/WeaponRack"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                    case "Well":
                        Instantiate(Resources.Load<Buildings>("Buildings/Well"),new Vector3(gameData.buildings[i].PositionX, gameData.buildings[i].PositionY), quaternion.identity).BuildingLevel = gameData.buildings[i].BuildingLevel;
                        break;
                }
            }
        }
        Debug.Log("Load");
    }

    private void ChangeDayTime()
    {
        CurrentTime += Time.deltaTime;

        if (CurrentTime >= 60 * 1 && dayState == DayState.DAY)
        {
            _globalLightIntensityOnPosition = false;
            if (!_globalLightIntensityOnPosition)
            {
                GlobalLight.intensity -= Time.deltaTime / 20;

                if (GlobalLight.intensity <= 0.65f)
                {
                    GlobalLight.intensity = 0.65f;
                    _globalLightIntensityOnPosition = true;
                    dayState = DayState.EVENING;
                }
            }
        }

        if (CurrentTime >= 60 * 2 && dayState == DayState.EVENING)
        {
            _globalLightIntensityOnPosition = false;
            if (!_globalLightIntensityOnPosition)
            {
                GlobalLight.intensity -= Time.deltaTime / 15;

                if (GlobalLight.intensity <= 0)
                {
                    GlobalLight.intensity = 0;
                    _globalLightIntensityOnPosition = true;
                    dayState = DayState.NIGHT;
                    BackgroundMusic.instance.PlayNighttimeMusic();
                }
            }
        }

        if (CurrentTime >= 60 * 5 && dayState == DayState.NIGHT || WaveSpawner.Instance.IsMonstersDead())
        {
            _globalLightIntensityOnPosition = false;
            if (!_globalLightIntensityOnPosition)
            {
                GlobalLight.intensity += Time.deltaTime / 15;

                if (GlobalLight.intensity >= 1)
                {
                    CanCheckIsMonstersDead = false;
                    GlobalLight.intensity = 1;
                    _globalLightIntensityOnPosition = true;
                    dayState = DayState.DAY;
                    CurrentTime = 0;
                    DayCount++;
                    Reward();
                    Save();
                    DailyChest.SpawnChest();
                    DayCountText.text = DayCount.ToString();
                    dayCounterState = DayCounterState.SHOW;
                    StartCoroutine(ShowDayCount());
                    BackgroundMusic.instance.PlayDaytimeMusic();
                }
            }
        }
    }

    private System.Collections.IEnumerator ShowDayCount()
    {
        while (DayCountText.color != Color.black && dayCounterState == DayCounterState.SHOW)
        {
            DayCountText.color = Color.Lerp(DayCountText.color, Color.black, Time.deltaTime);
            yield return null;
        }
        dayCounterState = DayCounterState.HIDE;

        yield return null;

        while (DayCountText.color != Color.clear && dayCounterState == DayCounterState.HIDE)
        {
            DayCountText.color = Color.Lerp(DayCountText.color, Color.clear, Time.deltaTime);
            yield return null;
        }
        dayCounterState = DayCounterState.OFF;
        yield break;
    }

    private void Reward()
    {
        _base._moneyCollected = 58 + (2 * DayCount);
        _base._baseChest.ChestOpen();
        _base.CollectMoney();
    }
    public void CheckBorders()
    {
        LeftBorder.transform.position = new Vector2(-1, -3);
        RightBorder.transform.position = new Vector2(1, -3);

        if (BuildingsPositions.Count != 0)
        {
            foreach (Transform item in BuildingsPositions)
            {
                if (item.position.x < LeftBorder.transform.position.x && item.name != "RoadLamp")
                {
                    LeftBorder.transform.position = item.position + new Vector3(3, 0);
                    LeftBorder.transform.position = new Vector3(LeftBorder.transform.position.x, -3);
                    Debug.Log($"New left border position: {LeftBorder.transform.position.x}");
                }

                if (item.position.x > RightBorder.transform.position.x && item.name != "RoadLamp")
                {
                    RightBorder.transform.position = item.position + new Vector3(-3, 0);
                    RightBorder.transform.position = new Vector3(RightBorder.transform.position.x, -3);
                    Debug.Log($"New Right border position: {RightBorder.transform.position.x}");
                }
            }

            MiddleOfTheBorders = RightBorder.transform.position + LeftBorder.transform.position;
        }
        else
        {
            RightBorder.transform.position = new Vector2(1, -3);
            LeftBorder.transform.position = new Vector2(-1, -3);
            MiddleOfTheBorders = RightBorder.transform.position + LeftBorder.transform.position;
        }
    }
}
