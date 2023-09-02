using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildingInfo", menuName = "ScriptableObjects/New buildingInfo")]

public class BuildingInfo : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private float _buildingArea;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private int _health;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _buildingMask;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _spriteIcon;

    public string id => this._id;
    public int health => this._health;
    public GameObject prefab => this._prefab;
    public Sprite spriteIcon => this._spriteIcon;


}
