using UnityEngine;
using UnityEngine.UI;

public class InfoOnClick : MonoBehaviour
{
    public static InfoOnClick instance;

    public GameObject _infoWindow;
    private Buildings _building;
    private Unit _unit;

    [SerializeField] private GameObject _closeButton;
    [SerializeField] private GameObject _forBuildings;
    [SerializeField] private GameObject _borderButtons;

    [SerializeField] private Text TargetsName;
    [SerializeField] private Text Hp;
    [SerializeField] private Text Repair;
    [SerializeField] private Text CollectedMoney;
    [SerializeField] private Text NextLvlCost;
    [SerializeField] private Text NextLvlText;


    private bool Maxlvl = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _borderButtons.SetActive(false);
        _infoWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _infoWindow.activeInHierarchy)
            CloseWindow();
    }
    private void FixedUpdate()
    {
        if (_unit && _infoWindow.activeInHierarchy)
        {
            Hp.text = _unit.Health.ToString();
        }
        if (_building && _infoWindow.activeInHierarchy)
        {
            Hp.text = _building.Health.ToString();

            _forBuildings.SetActive(true);
            Repair.text = _building._repairCost.ToString();
            CollectedMoney.text = _building._moneyCollected.ToString();
            if (Maxlvl == false)
            {
                if (_building.BuildingLevel == 3)
                {
                    Maxlvl = true;
                    NextLvlText.text = "Maximum level";
                    NextLvlCost.text = "";
                }
                else
                    NextLvlCost.text = _building._nextLvlCost.ToString();
            }
        }
    }

    public void SelectTarget(Buildings target)
    {
        if (!_infoWindow.activeInHierarchy)
        {
            _building = target;
            _unit = null;
            //Maxlvl = false;
            _infoWindow.SetActive(true);
            _borderButtons.SetActive(false);
            _forBuildings.SetActive(true);
            TargetsName.text = ((Object)_building).name;
            if (Maxlvl == true)
            {
                NextLvlText.text = "Maximum level";
                NextLvlCost.text = "";
            }
            else
                NextLvlText.text = "Next Lvl Cost";
        }
    }
    public void SelectTarget(Unit target)
    {
        if (!_infoWindow.activeInHierarchy)
        {
            _unit = target;
            _building = null;
            Maxlvl = false;
            _infoWindow.SetActive(true);
            if (_unit.gameObject.CompareTag("Allies") || _unit.gameObject.layer == LayerMask.NameToLayer("GoesToPosition"))
                _borderButtons.SetActive(true);
            _forBuildings.SetActive(false);
            TargetsName.text = _unit.name;
        }
    }

    public void SelectLeftBorder()
    {
        if (_unit != null && _unit.GetComponent<AlliesSolders>() != null)
        {
            Debug.Log(WorldManager.Instance.LeftBorder);
            _unit.GetComponent<Unit>();
            _unit.Border = WorldManager.Instance.LeftBorder;
            _unit.onBorder = Unit.OnBorder.LEFT;
            _unit.IsOnBorder = false;
        }
    }
    public void SelectRightBorder()
    {
        if (_unit != null && _unit.GetComponent<AlliesSolders>() != null)
        {
            Debug.Log(WorldManager.Instance.RightBorder);
            _unit.GetComponent<Unit>();
            _unit.Border = WorldManager.Instance.RightBorder;
            _unit.onBorder = Unit.OnBorder.RIGHT;
            _unit.IsOnBorder = false;
        }
    }

    public void CloseWindow()
    {
        _unit = null;
        _building = null;
        Maxlvl = false;
        _infoWindow.SetActive(false);
    }

}
