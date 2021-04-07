using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public float speed = 5;
    public static PlayerControls instance;
    public LayerMask grabMask;

    private Node currNode;

    void Start()
    {
        instance = this;
    }

    void Update() {
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(-1 * transform.up * speed * Time.deltaTime);
        }
        transform.Translate(transform.right * Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);
        transform.Translate(transform.forward * Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 0f, grabMask);
        if (hits.Length == 0)
        {
            currNode = null;
        }

        if (!Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0) && currNode && !BuildManager.pauseFlag)
        {
            currNode.BuildTurret();
        }
    }

    public void SetCurrNode(Node newNode)
    {
        currNode = newNode;
    }
}