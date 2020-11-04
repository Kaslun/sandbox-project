using Manager;
using UnityEngine;

public class Breakable : MonoBehaviour
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
