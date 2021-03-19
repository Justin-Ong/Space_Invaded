using UnityEngine;

public class Node : MonoBehaviour
{
	public Color hoverColor;
	public Vector3 positionOffset;
	public LayerMask obstacleMask;
	public string enemyTag = "EnemySpawner";

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
		RaycastHit[] hit;
		hit = Physics.BoxCastAll(transform.position - transform.forward.normalized, Vector3.one * 0.5f, transform.forward, transform.rotation, 1f, obstacleMask);
		if (hit.Length > 0)
		{
			return true;
		}
		else {
			GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag(enemyTag);
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