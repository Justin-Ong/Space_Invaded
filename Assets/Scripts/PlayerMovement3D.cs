using UnityEngine;

public class PlayerMovement3D : MonoBehaviour {
    public float speed = 5;
    public static PlayerMovement3D instance;
    public LayerMask grabMask;

    private Node currNode;

    void Start()
    {
        instance = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Translate(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.Translate(Vector3.down);
        }

        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 0f, grabMask);
        if (hits.Length == 0)
        {
            currNode = null;
        }

        if (Input.GetMouseButtonDown(0) && currNode)
        {
            currNode.BuildTurret();
        }
    }

    public void SetCurrNode(Node newNode)
    {
        currNode = newNode;
    }
}