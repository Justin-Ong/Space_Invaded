using UnityEngine;

public class BuildManager : MonoBehaviour
{

	public static BuildManager instance;

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
	public int numTurrets = 3;

	private GameObject[] turretArray;
	private int index = 0;

	void Start()
	{
		turretArray = new GameObject[numTurrets];
		turretArray[0] = standardTurretPrefab1;
		turretArray[1] = standardTurretPrefab2;
		turretArray[2] = standardTurretPrefab3;
		turretToBuild = standardTurretPrefab1;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
        {
            index = (index + 1) % numTurrets;
			turretToBuild = turretArray[index];
        }
	}

	private GameObject turretToBuild;

	public GameObject GetTurretToBuild()
	{
		return turretToBuild;
	}

}