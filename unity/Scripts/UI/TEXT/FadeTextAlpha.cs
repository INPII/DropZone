using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class FadeTextAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CanvasGroup fadeText1Group;  // ù ��° �ؽ�Ʈ�� CanvasGroup
    public CanvasGroup fadeText2Group;  // �� ��° �ؽ�Ʈ�� CanvasGroup

    public float fadeDuration = 0.5f;  // ���İ��� ���ϴ� �ð� (��)

    private bool isAlphaLocked = false; // Alpha�� ���� �������� ���θ� üũ

    private void Start()
    {
        SetAlpha(fadeText1Group, 0);
        SetAlpha(fadeText2Group, 0);

    }

    // ���콺�� �÷��� �ؽ�Ʈ ���� �ö��� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Alpha ���� 1�� ������ ���°� �ƴϸ� ���̵� �� ����
        if (!isAlphaLocked)
        {
            StartCoroutine(FadeInText(fadeText1Group));
            StartCoroutine(FadeInText(fadeText2Group));
        }
    }

    // ���콺�� �÷��� �ؽ�Ʈ���� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        // Alpha ���� ���� ���°� �ƴϸ� ��� ���̵� �ƿ�
        if (!isAlphaLocked)
        {
            StopAllCoroutines(); // �ڷ�ƾ ���� (���̵� �� ���� �� ��� ���̵� �ƿ��ǵ���)
            fadeText1Group.alpha = 0;
            fadeText2Group.alpha = 0;

            fadeText1Group.gameObject.SetActive(false);
            fadeText2Group.gameObject.SetActive(false);
        }
    }

    // �÷��� �ؽ�Ʈ�� Ŭ������ �� alpha ���� 1�� �����ϰų� 0���� ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAlphaLocked)
        {
            // �̹� Alpha�� 1�� ������ ��� -> Alpha�� 0���� �����ϰ� ��Ȱ��ȭ
            SetAlpha(fadeText1Group, 0);
            SetAlpha(fadeText2Group, 0);
            isAlphaLocked = false;
        }
        else
        {
            // Alpha�� 1�� �����ϰ� Ȱ��ȭ
            SetAlpha(fadeText1Group, 1);
            SetAlpha(fadeText2Group, 1);
            isAlphaLocked = true;
        }
    }

    // Alpha ���� ������ ������Ű�� �Լ�
    private IEnumerator FadeInText(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1; // ���������� ���İ��� 1�� ����
    }

    // Alpha ���� �����ϰ� Ȱ��ȭ/��Ȱ��ȭ ���¸� �����ϴ� �Լ�
    private void SetAlpha(CanvasGroup canvasGroup, float alphaValue)
    {
        canvasGroup.alpha = alphaValue;
        canvasGroup.gameObject.SetActive(alphaValue > 0);
    }
}
