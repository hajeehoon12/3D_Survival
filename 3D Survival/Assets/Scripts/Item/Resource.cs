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
    public Material[] _materials;
    //public float tempY;

    private void Start()
    {
        capacity = maxCapacity;

        
        _materials = GetComponent<MeshRenderer>().materials;
        
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
        Invoke("ResourceInit", 5f);

    }

    private void ResourceInit()
    {
        gameObject.SetActive(true);



        foreach (Material _material in _materials)
        {
            _material.DOFloat(1f, "_Cutoff",0f ).OnComplete(()=> _material.DOFloat(0.3f, "_Cutoff", 10f).SetEase(Ease.InExpo));
        }

        //tempY = gameObject.transform.localScale.y;
        //Debug.Log(tempY);
        //gameObject.transform.DOScaleY(0, 0f);
        //gameObject.transform.DOScaleY(tempY,10f);
        

        capacity = maxCapacity;
    }




}
