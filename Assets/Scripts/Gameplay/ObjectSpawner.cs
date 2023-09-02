using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject Prefab;
    public float BuildingArea;
    public int Cost;
    private GameObject _objectSpawner;
    [SerializeField] private Text _costText;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _buildingsMask;
    [SerializeField] private LayerMask _enemyBuildingsMask;
    [SerializeField] private LayerMask _roadLampMask;

    private void Start()
    {
        _enemyMask = LayerMask.GetMask("Enemy");
        _buildingsMask = LayerMask.GetMask("Building");
        _enemyBuildingsMask = LayerMask.GetMask("EnemyBuilding");
        _roadLampMask = LayerMask.GetMask("RoadLamp");

        _objectSpawner = GameObject.Find("objectSpawner");

        _costText.text = Cost.ToString();
    }

    public void SpawnUnit()
    {
        if (Inventory.Instance.Money >= Cost)
        {
            Instantiate(Prefab, MenuUI.Instance.UnitSpawner.position, Quaternion.identity);
            Inventory.Instance.Money -= Cost;
        }
    }

    public void SpawnObject()
    { 
        Vector3 spawnPos = _objectSpawner.transform.position;
        spawnPos.z = 0f;
        if (Prefab.name == "RoadLamp" && Inventory.Instance.Money >= Cost && Physics2D.OverlapCircleAll(_objectSpawner.transform.position, BuildingArea, _roadLampMask).Length == 0)
        {
            Instantiate(Prefab, spawnPos, Quaternion.identity);
            Inventory.Instance.Money -= Cost;
            return;
        }

        if (Prefab.name != "RoadLamp" && Physics2D.OverlapCircleAll(_objectSpawner.transform.position, BuildingArea, _buildingsMask).Length == 0 &&
            Physics2D.OverlapCircleAll(_objectSpawner.transform.position, BuildingArea, _enemyMask).Length == 0 &&
            Physics2D.OverlapCircleAll(_objectSpawner.transform.position, BuildingArea, _enemyBuildingsMask).Length == 0 &&
            Inventory.Instance.Money >= Cost)
        {
            Instantiate(Prefab, spawnPos, Quaternion.identity);
            Inventory.Instance.Money -= Cost;
        }
    }
}
