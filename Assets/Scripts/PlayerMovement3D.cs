using UnityEngine;

public class PlayerMovement3D : MonoBehaviour {
    public float speed = 5;
    public static PlayerMovement3D instance;
    public LayerMask grabMask;

    private Node currNode;
    private Vector3 left = new Vector3(0, 90, 0);
    private Vector3 right = new Vector3(0, -90, 0);

    void Start()
    {
        instance = this;
    }

    void Update() {
        if (Input.GetKey(KeyCode.R))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(left);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(right);
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