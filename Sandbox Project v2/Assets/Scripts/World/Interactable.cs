using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableObject interactableObject;
    public GameObject[] itemPool;

    public void Interact()
    {
        if (interactableObject.isBreakable)
        {
            SpawnItem();
            Destroy(gameObject);
        }

        else
        {
            Debug.Log(name + " is interacted with");
        }
    }

    private void SpawnItem()
    {
        GameObject go = Instantiate(itemPool[Random.Range(0, itemPool.Length)]);
        go.transform.position = transform.position;
    }
}
