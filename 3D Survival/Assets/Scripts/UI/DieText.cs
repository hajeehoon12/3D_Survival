using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DieText : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowDieMessage()
    {
        // 초기 설정
        canvasGroup.alpha = 0;
        rectTransform.localScale = Vector3.one;
        tmpText.maxVisibleCharacters = 0;

        // DOTween을 이용해 텍스트를 1글자씩 보이게 하기
        tmpText.DOText(tmpText.text, 1.0f).SetEase(Ease.Linear);

        // 페이드 인 애니메이션과 스케일 애니메이션
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1, 1.0f)); // 페이드 인
        sequence.Join(rectTransform.DOScale(1.5f, 1.0f)); // 스케일 애니메이션

        // 일정 시간 후에 페이드 아웃 및 삭제
        sequence.AppendInterval(2.0f);
        sequence.Append(canvasGroup.DOFade(0, 1.0f)); // 페이드 아웃
        sequence.OnComplete(() => Destroy(gameObject)); // 삭제
    }
}