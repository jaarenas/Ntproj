using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{

    // store the reference to premade objects
    [SerializeField]
    private GameObject afterImagePrefab;

    // store inactive objects
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void GrowPool()
    {
        for (int i = 0; i< 10; i++)
        {
             var InstanceToAdd = Instantiate(afterImagePrefab);
    InstanceToAdd.transform.SetParent(transform);
             AddToPool(InstanceToAdd);
}
    }

    public void AddToPool(GameObject instance)
{
    instance.SetActive(false);
    availableObjects.Enqueue(instance);
}


public GameObject GetFromPool()
{
    if (availableObjects.Count == 0)
    {
        GrowPool();

    }

    var instance = availableObjects.Dequeue();
    instance.SetActive(true);
    return instance;
}




}

