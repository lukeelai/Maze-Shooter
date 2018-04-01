using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject projectile;
    private GameObject bullets;

    public float speed = 1;
    public int lives = 3;
    public int ammo = 1;
    private Text ammoText, livesText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ammoText = GameObject.Find("HUD/Ammo").GetComponent<Text>();
        livesText = GameObject.Find("HUD/Lives").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && ammo > 0)
        {
            GameObject clone = Instantiate(projectile, transform.position + (transform.forward * 0.7f), transform.rotation) as GameObject;
            Rigidbody prb = clone.GetComponent<Rigidbody>();
            prb.velocity = transform.TransformDirection(Vector3.forward * 10);
            ammo--;
        }

        if (lives == 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        rotation();
        movement();
        ammoText.text = "Ammo: " + ammo.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet") lives--;
    }

    void movement()
    {
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

    void rotation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.LookAt(hit.point);
        }
    }
}