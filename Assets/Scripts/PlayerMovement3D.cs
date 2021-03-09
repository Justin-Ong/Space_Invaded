using UnityEngine;

public class PlayerMovement3D : MonoBehaviour {
    public float speed = 5;
    private Vector3 motion;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();

    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Translate(Vector3.up * 1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.Translate(Vector3.down * 1);
        }

        motion = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rb.velocity = motion * speed;
    }
}