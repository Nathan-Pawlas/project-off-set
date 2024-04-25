using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class Interactable : MonoBehaviour
{
    public bool currentlyActive = false;
    public void interact()
    {
        gameObject.GetComponent<Interaction>().interact();
        currentlyActive = true;
    }

    public void stopInteract()
    {
        gameObject.GetComponent<Interaction>().stopInteract();
        currentlyActive = false;
    }

}
