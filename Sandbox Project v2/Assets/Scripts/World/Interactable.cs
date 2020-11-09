using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Interactable : MonoBehaviour
{
    public InteractableObject interactableObject;
    public MonoBehaviour reciever;

    [Header("Color Properties")]
    public AnimationCurve animCurve;
    public virtual void Interact()
    {
        ColorFilterManager c = reciever as ColorFilterManager;
        if(c != null)
            SendCurve();

        else
        {
            SendMessage();
        }
    }
    private void SendCurve()
    {
        reciever.SendMessage("UpdateColorFilter", animCurve);
    }

    private void SendMessage()
    {
        print("Sent message");
    }
}
