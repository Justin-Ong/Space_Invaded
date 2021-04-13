using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BuildManager : MonoBehaviour
{
	public static BuildManager instance;
	public static bool buildModeFlag = true;
	public static bool pauseFlag = false;
	public static bool victoryFlag = false;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		instance = this;
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
    private string[] descriptionArray;

	public Sprite TurretImage1;
	public Sprite TurretImage2;
	public Sprite TurretImage3;
	public Sprite TurretImage4;

	public GameObject UI;
	public int maxBuildModeCount = 60;
	private float remainingTime;
	private int buildModeCount;
	private Text buildModeCounter;

	// Audio Prefabs
	public GameObject audioEnterBuildingPhase;
	public GameObject audioEnterGamePhase;

	// Pathing updates
	List<EnemySpawnerBehaviour> spawners = new List<EnemySpawnerBehaviour>();
	List<Vector3> pos = new List<Vector3>();
	Thread thread;
	bool threadRunning;

	void Start()
	{
		turretArray = new GameObject[numTurrets];
		turretImagesArray = new Sprite[numTurrets];

		rangeArray = new string[numStatTypes];
		fireRateArray = new string[numStatTypes];
		damageArray = new string[numStatTypes];
		costArray = new string[numStatTypes];
		moneyArray = new int[numStatTypes];
        descriptionArray = new string[numStatTypes];

		rangeArray[0] = "Range: 8";
		rangeArray[1] = "Range: 15";
		rangeArray[2] = "Range: 20";
		rangeArray[3] = "Range: 10";

		fireRateArray[0] = "Fire Rate: Constant";
		fireRateArray[1] = "Fire Rate: 6 per sec";
		fireRateArray[2] = "Fire Rate: 1 per sec";
		fireRateArray[3] = "Fire Rate: Constant";

		damageArray[0] = "Damage: 5 per second";
		damageArray[1] = "Damage: 1 per shot";
		damageArray[2] = "Damage: 10 per shot";
		damageArray[3] = "Damage: 0";

		costArray[0] = "Cost: 50";
		costArray[1] = "Cost: 100";
		costArray[2] = "Cost: 200";
		costArray[3] = "Cost: 100";

		moneyArray[0] = 50;
		moneyArray[1] = 100;
		moneyArray[2] = 200;
		moneyArray[3] = 100;

        descriptionArray[0] = "A short-ranged laser turret.";
        descriptionArray[1] = "A fast-firing gun tower.";
        descriptionArray[2] = "A powerful, slow-firing missile tower.";
        descriptionArray[3] = "A slowing laser.";

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
		buildModeCount = maxBuildModeCount;

		buildModeCounter = UI.transform.Find("BuildModeTimer").GetComponent<Text>();

		GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
		foreach (GameObject enemySpawner in enemySpawners)
		{
			spawners.Add(enemySpawner.GetComponent<EnemySpawnerBehaviour>());
			pos.Add(enemySpawner.transform.position);
		}
		threadRunning = true;
		thread = new Thread(() => PathingRefresh(spawners, pos));
		thread.Start();
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
                GameObject.Find("Description").GetComponent<Text>().text = descriptionArray[index];

                turretToBuild = turretArray[index];
				moneyToBuild = moneyArray[index];
			}
		}
		if (victoryFlag)
		{
			Time.timeScale = 0;
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
			if (buildModeCount <= 0)
			{
				buildModeCounter.text = "";
				buildModeCount = maxBuildModeCount;
				Instantiate(audioEnterBuildingPhase);
				buildModeFlag = false;
				remainingTime = 1f;
				EnemySpawnerBehaviour.TriggerBuildMode = false;
				EnemySpawnerBehaviour.waveOver = false;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				buildModeCounter.text = "";
				buildModeCount = maxBuildModeCount;
				Instantiate(audioEnterGamePhase);
				buildModeFlag = false;
				remainingTime = 1f;
				EnemySpawnerBehaviour.TriggerBuildMode = false;
				EnemySpawnerBehaviour.waveOver = false;
			}
		}
		if (buildModeFlag) 
		{
			buildModeCounter.text = buildModeCount.ToString();
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0)
                {
				buildModeCount -= 1;
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

	private void PathingRefresh(List<EnemySpawnerBehaviour> spawners, List<Vector3> pos)
	{
		while (threadRunning)
		{
			for (int i = 0; i < spawners.Count; i++)
			{
				spawners[i].GetWaypoints(pos[i]);
			}
			Thread.Sleep(500);
		}
	}

    private void OnDisable()
    {
		threadRunning = false;
		thread.Join();
    }
}
