using UnityEngine;

public class Node : MonoBehaviour
{
	public Color hoverColor;
	public Vector3 positionOffset;
	public LayerMask obstacleMask;
	public string enemySpawnerTag = "EnemySpawner";

	private GameObject turret;
	private Renderer rend;
	private MeshRenderer meshRend;
	private Color startColor;
	private bool isBlocked;

	void Start()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
		meshRend = gameObject.GetComponent<MeshRenderer>();
		meshRend.enabled = false;
		isBlocked = CheckIfBlocked();
	}

    void OnMouseEnter()
	{
		rend.material.color = hoverColor;
	}

	void OnMouseExit()
	{
		rend.material.color = startColor;
	}

	public void BuildTurret()
	{
		if (turret != null || !meshRend.enabled || isBlocked)
		{
			Debug.Log("Can't build there! - TODO: Display on screen.");
			return;
		}

		GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
		turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
	}

	private bool CheckIfBlocked()
	{
		Collider[] hit;
		hit = Physics.OverlapBox(transform.position, Vector3.one * 0.5f, Quaternion.identity, obstacleMask, QueryTriggerInteraction.Ignore);
		if (hit.Length > 0)
		{
			return true;
		}
		else {
			GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag(enemySpawnerTag);
			foreach (GameObject enemySpawner in enemySpawners)
			{
				if ((transform.position - enemySpawner.transform.position).magnitude < 10) {
					return true;
				}
			}
		}
		return false;
	}

	private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Player"))
		{
			meshRend.enabled = true;
			other.gameObject.GetComponent<PlayerMovement3D>().SetCurrNode(this);
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