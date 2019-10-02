using System.Collections;
using System.Collections.Generic;
using System.Linq; //This using statement allows us to use the "orderby" logic below to sort.
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{


    //Maximum Y position in bobbing motion
    [SerializeField]
    private float MaxY;

    //Minimum Y position in bobbing motion
    [SerializeField]
    private float MinY;

    //speed of the bobbing motion.
    [SerializeField]
    private float BobSpeed;

    private GameObject[] destinations;

    private void Start()
    {
        //Retrieve a list of all the game objects tagged as a teleport destination
        //store it in the class variable "destinations"
        //i noticed whilst coding that i could move this here to do it once when the
        //object is created, instead of everytime a trigger event happens, and it
        //should work the same.
        destinations = GameObject.FindGameObjectsWithTag("TeleportDestination");
    }

    // Update is called once per frame
    private void Update()
    {
        //If the current Y is greaten than or less than MaxY or MinY
        if (transform.position.y >= MaxY ||
            transform.position.y <= MinY)
        {
            //Then invert the direction of travel
            BobSpeed *= -1.0f;
        }

        //Move the sphere/teleporter
        transform.position += new Vector3(0.0f, BobSpeed * Time.deltaTime, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
        //if it's not the player that's collided with the object - dont care
    {
        if (other.gameObject.tag != "Player")
        {
            Debug.Log("Colliding");
            return;
        }
        
        //Sort the array of destinations by which is closest to the current objects position
        //this could slow down logic if you have a lot of potential destinations (like hundreds)
        //if so, you may want to pick a fixed destination, or alter the logic to this sort 
        //(maybe if the player is teleported to a random destination?)
        destinations = destinations.OrderBy(p => Vector3.Distance(transform.position, p.transform.position)).ToArray();


        //Actually move the player objects position to match that sort of destinations position.
        other.gameObject.GetComponent<CharacterController>().enabled = false;
        other.gameObject.transform.position = destinations[0].transform.position;
        other.gameObject.GetComponent<CharacterController>().enabled = true;
        
    }
}
