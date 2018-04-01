using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;
    private Rigidbody rb;
    public GameObject projectile;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject clone = Instantiate(projectile, transform.position + (transform.forward * 0.5f), transform.rotation) as GameObject;
            Rigidbody prb = clone.GetComponent<Rigidbody>();
            prb.velocity = transform.TransformDirection(Vector3.forward * 10);
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.LookAt(hit.point);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.position -= transform.forward * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.position -= transform.right * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.position += transform.right * Time.deltaTime * speed;
        }
    }
}