using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using UnityEditor;

public class Interactable : MonoBehaviour
{
    public InteractableObject interactableObject;
    public MonoBehaviour reciever;

    public AnimationCurve colorCurve;
    public virtual void Interact()
    {
        if(colorCurve.length > 0)
            SendCurve();

        else
        {
            SendMessage();
        }
    }
    private void SendCurve()
    {
        reciever.SendMessage("UpdateColorFilter", colorCurve);
    }

    private void SendMessage()
    {
        print("Sent message");
    }
}
