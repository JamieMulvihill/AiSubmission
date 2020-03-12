using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Spawn spawner;
    GameObject gO;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public Rigidbody2D carRigidbody;
    public float speed;
    public float carTorque;
    public float deathCheckTime = 0;
    public float speedMultiplier = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        backTire.AddTorque(-speed * Time.fixedDeltaTime * speedMultiplier);
        frontTire.AddTorque(-speed * Time.fixedDeltaTime * speedMultiplier);
    }

    public void DeathCheck(float time) {
        deathCheckTime++;
        if (deathCheckTime > 120 && gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
            gameObject.transform.rotation = Quaternion.identity;
            spawner.deadCars++;
        }
    }
}
