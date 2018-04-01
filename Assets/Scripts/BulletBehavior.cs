using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    int counter = 0;

    // Use this for initialization
    void Start () {

    }

// Update is called once per frame
void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            counter++;
        }
        if (counter > 4) {
            Destroy(this.gameObject);
            counter = 0;
        }

        if (collision.gameObject.tag == "Player") Destroy(this.gameObject);
    }
}
