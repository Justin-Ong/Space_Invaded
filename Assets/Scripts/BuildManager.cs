using UnityEngine;
using UnityEngine.UI;


public class BuildManager : MonoBehaviour
{
	//private float fixedDeltaTime;
	public static BuildManager instance;
	public static bool buildModeFlag = false;
	public static bool pauseFlag = false;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		instance = this;
		//fixedDeltaTime = Time.fixedDeltaTime;
	}

	public GameObject standardTurretPrefab1;
	public GameObject standardTurretPrefab2;
	public GameObject standardTurretPrefab3;
	public GameObject standardTurretPrefab4;

	public int numTurrets = 4;
	private int numStatTypes = 4;

	private GameObject[] turretArray;
	private Sprite[] turretImagesArray;
	private int index = 0;

	private string[] rangeArray;
	private string[] fireRateArray;
	private string[] damageArray;
	private string[] costArray;
	private int[] moneyArray;

	public Sprite TurretImage1;
	public Sprite TurretImage2;
	public Sprite TurretImage3;
	public Sprite TurretImage4;

	private float remainingTime;
	private int count;

	void Start()
	{
		turretArray = new GameObject[numTurrets];
		turretImagesArray = new Sprite[numTurrets];

		rangeArray = new string[numStatTypes];
		fireRateArray = new string[numStatTypes];
		damageArray = new string[numStatTypes];
		costArray = new string[numStatTypes];
		moneyArray = new int[numStatTypes];

		rangeArray[0] = "Range: 9";
		rangeArray[1] = "Range: 15";
		rangeArray[2] = "Range: 15";
		rangeArray[3] = "Range: 8";

		fireRateArray[0] = "Fire Rate: 0.5";
		fireRateArray[1] = "Fire Rate: 2";
		fireRateArray[2] = "Fire Rate: 2";
		fireRateArray[3] = "Laser";

		damageArray[0] = "Damage: 10";
		damageArray[1] = "Damage: 1";
		damageArray[2] = "Damage: 5";
		damageArray[3] = "Damage: 0";

		costArray[0] = "Cost: 50";
		costArray[1] = "Cost: 100";
		costArray[2] = "Cost: 200";
		costArray[3] = "Cost: 150";

		moneyArray[0] = 50;
		moneyArray[1] = 100;
		moneyArray[2] = 200;
		moneyArray[3] = 150;

		turretArray[0] = standardTurretPrefab1;
		turretArray[1] = standardTurretPrefab2;
		turretArray[2] = standardTurretPrefab3;
		turretArray[3] = standardTurretPrefab4;

		turretImagesArray[0] = TurretImage1;
		turretImagesArray[1] = TurretImage2;
		turretImagesArray[2] = TurretImage3;
		turretImagesArray[3] = TurretImage4;

		turretToBuild = standardTurretPrefab1;
		moneyToBuild = 50;

		remainingTime = 1f;
		count = 30;
	}

	void Update()
	{
		if (!pauseFlag)
		{
			if (Input.GetMouseButtonDown(1))
			{
				index = (index + 1) % numTurrets;
				GameObject.Find("TurretDisplay").GetComponent<Button>().image.sprite = turretImagesArray[index];
				GameObject.Find("Range").GetComponent<Text>().text = rangeArray[index];
				GameObject.Find("Fire Rate").GetComponent<Text>().text = fireRateArray[index];
				GameObject.Find("Damage").GetComponent<Text>().text = damageArray[index];
				GameObject.Find("Cost").GetComponent<Text>().text = costArray[index];

				turretToBuild = turretArray[index];
				moneyToBuild = moneyArray[index];
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pauseFlag)
			{
				Time.timeScale = 1;
				pauseFlag = false;
			}
			else
			{
				Time.timeScale = 0;
				pauseFlag = true;
			}
		}
		if (EnemySpawnerBehaviour.TriggerBuildMode)
		{
			buildModeFlag = true;
			if (count == 0)
			{
				Debug.Log("You have no time left.");
				count = 30;
				buildModeFlag = false;
				remainingTime = 1f;
				EnemySpawnerBehaviour.TriggerBuildMode = false;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("You have exited Building mode.");
				count = 30;
				buildModeFlag = false;
				remainingTime = 1f;
				EnemySpawnerBehaviour.TriggerBuildMode = false;
			}
		}
		if (buildModeFlag) 
		{
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0)
                {
				Debug.Log(count);
				count -= 1;
				remainingTime = 1f;
			}
		}
	}

	private GameObject turretToBuild;
	public static int moneyToBuild;

	public GameObject GetTurretToBuild()
	{
		return turretToBuild;
	}
}
