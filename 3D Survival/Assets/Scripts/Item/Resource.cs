using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum ResourceType
{ 
    Wood,
    Bush,
    Rock,
    Ores
}

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1; // 한방에 얼마나 다는지
    public int capacity; // 내구도 용량
    public int maxCapacity;
    public Material[] _materials;
    public float tempY;

    [Header("Regeneration")]
    public float reGroathTime;
    public ResourceType resourceType;
    

    private void Start()
    {
        capacity = maxCapacity;


        if (resourceType == ResourceType.Wood || resourceType == ResourceType.Bush)
        {
            _materials = GetComponent<MeshRenderer>().materials;
        }
        
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
                if ((resourceType == ResourceType.Wood || resourceType == ResourceType.Bush))
                {
                    PlantReGroath();
                    return;
                }
                gameObject.SetActive(false);
            }
            
        }
    }

    private void PlantReGroath() // wood && bush regroath
    { 
        gameObject.SetActive(false);
        Invoke("ResourceInit", reGroathTime);

    }

    private void ResourceInit()
    {
        gameObject.SetActive(true);



        foreach (Material _material in _materials)
        {
            _material.DOFloat(1f, "_Cutoff",0f ).OnComplete(()=> _material.DOFloat(0.3f, "_Cutoff", 10f).SetEase(Ease.InExpo));
        }

        tempY = gameObject.transform.localScale.y;
        //Debug.Log(tempY);
        gameObject.transform.DOScaleY(0, 0f);
        gameObject.transform.DOScaleY(tempY,2f);
        

        capacity = maxCapacity;
    }




}
