using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public Color hoverColor;
	public Vector3 positionOffset;
	public LayerMask obstacleMask;
	public string enemySpawnerTag = "EnemySpawner";
	public GameObject errorMessage;

	private Color originalColor;
	private GameObject turret;
	private Renderer rend;
	private MeshRenderer meshRend;
	private bool isBlocked;

	void Start()
	{
		rend = GetComponent<Renderer>();
		meshRend = gameObject.GetComponent<MeshRenderer>();
		meshRend.enabled = false;
		isBlocked = CheckIfBlocked();
		originalColor = rend.material.color;
		if (isBlocked)
		{
			rend.material.color = hoverColor;
		}
	}

	public void BuildTurret()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		if (turret != null || !meshRend.enabled || isBlocked)
		{
			ShowErrorMessage("Can't build here");
			return;
		}
		if (!BuildManager.buildModeFlag)
		{
			ShowErrorMessage("Can't build right now");
			return;
		}
        if (ResourceSystem.money < BuildManager.moneyToBuild)
		{
			ShowErrorMessage("Not enough money");
			return;
		}

		ResourceSystem.money -= BuildManager.moneyToBuild;

		GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
		turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
		turret.GetComponentInChildren<TurretLogic>().node = this;
		References.levelGrid.searchGrid.SetWalkableAt((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y), (int)Mathf.Floor(transform.position.z), false);

		GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
		rend.material.color = hoverColor;
	}

	public void RemoveTurret()
	{
		Destroy(turret);
		turret = null;
		References.levelGrid.searchGrid.SetWalkableAt((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y), (int)Mathf.Floor(transform.position.z), true);
		rend.material.color = originalColor;
	}

	private void ShowErrorMessage(string errMessage)
	{
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameObject newErr = Instantiate(errorMessage, transform.position + Vector3.up * 2, transform.rotation);
		ErrorMessage err = newErr.GetComponent<ErrorMessage>();
		err.text = errMessage;
		err.timeToLive = 2;
	}

	private bool CheckIfBlocked()
	{
		Collider[] hit;
		hit = Physics.OverlapBox(transform.position, Vector3.one * 0.5f, Quaternion.identity, obstacleMask, QueryTriggerInteraction.Ignore);
		if (hit.Length > 0)
		{
			return true;
		}
		else
		{
			GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag(enemySpawnerTag);
			foreach (GameObject enemySpawner in enemySpawners)
			{
				if ((transform.position - enemySpawner.transform.position).magnitude < 10)
				{
					return true;
				}
			}
			GameObject defencePoint = GameObject.Find("DefencePoint");
			if ((transform.position - defencePoint.transform.position).magnitude < 5)
			{
				return true;
			}
		}
		return false;
	}

	private void OnTriggerEnter(Collider other)
	{
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

		if (other.gameObject.CompareTag("Player"))
		{
			meshRend.enabled = true;
			other.gameObject.GetComponent<PlayerControls>().SetCurrNode(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			meshRend.enabled = false;
		}
	}
}