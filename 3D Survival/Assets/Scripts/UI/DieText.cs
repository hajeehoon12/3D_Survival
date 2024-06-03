using UnityEngine;
using TMPro;
using System.Collections;

public class DieText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform rectTransform;

    void Awake()
    {
       
    }

    public void ShowDieMessage()
    {   
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        rectTransform.localScale = Vector3.one;
        tmpText.maxVisibleCharacters = 0;
        Color originalColor = tmpText.color;
        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float fadeInDuration = 0.5f;
        //float scaleUpDuration = 1.0f;
        float visibleDuration = 2.0f;
        float fadeOutDuration = 1.0f;
        float scale = 1.5f;

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            rectTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(scale, scale, scale), elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        rectTransform.localScale = new Vector3(scale, scale, scale);

        int totalVisibleCharacters = tmpText.text.Length;
        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            tmpText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(visibleDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            rectTransform.localScale = Vector3.Lerp(new Vector3(scale, scale, scale), new Vector3(scale * 1.2f, scale * 1.2f, scale * 1.2f), elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        rectTransform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, scale * 1.2f);

        gameObject.SetActive(false);
    }
}