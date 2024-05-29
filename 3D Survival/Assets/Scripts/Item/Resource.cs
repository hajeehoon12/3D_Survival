using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1; // 한방에 얼마나 다는지
    public int capacity; // 내구도 용량

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;
            capacity -= 1;
            GameObject resources = Instantiate(itemToGive.dropPrefab, hitPoint - Vector3.forward * 1, Quaternion.LookRotation(hitNormal, Vector3.up));
            resources.GetComponent<Rigidbody>().AddForce((resources.transform.up + resources.transform.forward)*2 , ForceMode.Impulse);
        }
    }



}
