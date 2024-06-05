using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGreen : MonoBehaviour
{
    public LayerMask _constructMask;


    [Header("Renderer")]
    public MeshRenderer[] _Renderers;


    private void Start()
    {
        CharacterManager.Instance.Player.controller.canConstruct = true;

        foreach (MeshRenderer meshes in _Renderers)
        {
            meshes.material.color = Color.green;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != _constructMask)
        {
            Debug.Log("Can't Build");
            CharacterManager.Instance.Player.controller.canConstruct = false;

            foreach (MeshRenderer meshes in _Renderers)
            {
                meshes.material.color = Color.red;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.layer != _constructMask)
        {
            Debug.Log("Can Build");
            CharacterManager.Instance.Player.controller.canConstruct = true;

            foreach (MeshRenderer meshes in _Renderers)
            {
                meshes.material.color = Color.green;
            }
        }
    }


}
