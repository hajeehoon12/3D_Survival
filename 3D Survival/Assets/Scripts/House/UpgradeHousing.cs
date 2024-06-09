using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHousing : MonoBehaviour
{
    public MeshRenderer[] childs;
    public Texture[] _textures;
    int num = 0;
    private bool isClicked = false;

    private void Start()
    {
        //StartCoroutine(ChangeTexture());
    }



    public void UpgradeHouse()
    {
        if (!isClicked)
        {
            isClicked = true;
            return;
        }
        else isClicked = false;

        if (GameManager.instance.CanConsumeItem("UpgradeScroll", 1))
        {
            GameManager.instance.ConsumeItem("UpgradeScroll", 1);
            AudioManager.instance.PlaySFX("Upgrade");
            //CharacterManager.Instance.Player.controller.uiInventory.UpdateUI();
        }
        else
        {
            AudioManager.instance.PlaySFX("Cant");
            return;
        }
        Debug.Log("Upgrade!!");
        num++;
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].material.SetTexture("_MainTex", _textures[num % _textures.Length]);
        }

        gameObject.GetComponent<Resource>().maxCapacity += 2;
        gameObject.GetComponent<Resource>().capacity += 2;
    }




    IEnumerator ChangeTexture()
    {
        
        while (true)
        {
            num++;
            yield return new WaitForSeconds(3f);

            for (int i = 0; i < childs.Length; i++)
            {
                childs[i].material.SetTexture("_MainTex", _textures[num%_textures.Length]);
            }
        }
    }


}
