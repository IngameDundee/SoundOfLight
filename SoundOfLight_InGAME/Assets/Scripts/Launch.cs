using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
This script is to be attached to game objects that will be launched immediately after spawning
It adds a one time relative force to the GOs rigidbody, so the position the object is spawned 
in at will determine its direction of travel.
It then adds a soft gravity value to it consistently, to allow it to fall slower than default 
gravity acceleration.
 */
public class Launch : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasLaunched = false;
    // Start is called before the first frame update
    void Awake()
    {
        // Get rigidbody of the GO this is attached to
        rb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(launchSelf());

    }

    IEnumerator launchSelf()
    {
        yield return new WaitForSeconds(0.05f);// Small delay fixed occasionally not launching the GO
        rb.AddRelativeForce(-300f, 0f, 0f);
        hasLaunched = true;
    }

    private void FixedUpdate()
    {
        if(hasLaunched == true)
        {
            // Add custom low gravity force 
            rb.AddForce(0f, -1f, 0f);
        }
    }
}
