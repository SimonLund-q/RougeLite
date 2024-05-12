using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool canOpen = false;
    private bool hasBeenOpened = false;

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            canOpen = true;
        }
        else
        {
            canOpen = false;
        }
        if(hasBeenOpened == true && canOpen == false)
        {
            Close();
        }
        else if (hasBeenOpened == true)
        {
            Open();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canOpen == true)
        {
            Open();
        }
    }
    void Open()
    {
        hasBeenOpened = true;
        Destroy (gameObject.GetComponent<BoxCollider2D>());
    }
    void Close() 
    {
        gameObject.AddComponent<BoxCollider2D>();
    }
}
