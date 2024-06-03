using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1; // 한방에 얼마나 다는지
    public int capacity; // 내구도 용량
    public int maxCapacity;
    //public Material _material;

    private void Start()
    {
        capacity = maxCapacity;

        
        //_material = GetComponent<MeshRenderer>().material;
        
    }


    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;
            capacity -= 1;
            GameObject resources = Instantiate(itemToGive.dropPrefab, hitPoint - Vector3.forward * 1, Quaternion.LookRotation(hitNormal, Vector3.up));
            resources.GetComponent<Rigidbody>().AddForce((resources.transform.up + resources.transform.forward)*2 , ForceMode.Impulse);

            if (capacity == 0)
            {
                ReGroath();
            }
        }
    }

    private void ReGroath()
    { 
        gameObject.SetActive(false);
        Invoke("ResourceInit", 10f);

    }

    private void ResourceInit()
    {
        gameObject.SetActive(true);

        
        //_material.DOFade(0, 0f);
        //_material.DOFade(1, 2f);
        

        capacity = maxCapacity;
    }




}
