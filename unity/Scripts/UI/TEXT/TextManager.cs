using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextManager : MonoBehaviour
{
    // Unity �����Ϳ��� �巡�� �� ������� ������ �� �ִ� public ����Ʈ
    public List<TextMeshProUGUI> textObjects;

    // Hover �� ����� ũ�� ����
    public Color hoverColor = Color.red;
    public float hoverScaleMultiplier = 1.2f;

    private Dictionary<TextMeshProUGUI, Vector3> originalScales = new Dictionary<TextMeshProUGUI, Vector3>();
    private Dictionary<TextMeshProUGUI, Color> originalColors = new Dictionary<TextMeshProUGUI, Color>();

    void Start()
    {
        // �� �ؽ�Ʈ�� ���� ũ��� ���� ����
        foreach (var textObj in textObjects)
        {
            if (textObj != null)
            {
                originalScales[textObj] = textObj.transform.localScale;
                originalColors[textObj] = textObj.color;
            }
        }
    }

    void Update()
    {
        // �� �����Ӹ��� ���콺 ��ġ�� �浹�ϴ� �ؽ�Ʈ�� üũ
        foreach (var textObj in textObjects)
        {
            if (textObj != null)
            {
                RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

                // ���콺�� �ؽ�Ʈ ���� �ִ��� Ȯ��
                if (rectTransform.rect.Contains(localMousePosition))
                {
                    OnMouseEnter(textObj);
                }
                else
                {
                    OnMouseExit(textObj);
                }
            }
        }
    }

    // ���콺�� �ؽ�Ʈ ���� ���� ��
    void OnMouseEnter(TextMeshProUGUI textObj)
    {
        textObj.color = hoverColor;
        textObj.transform.localScale = originalScales[textObj] * hoverScaleMultiplier;
    }

    // ���콺�� �ؽ�Ʈ���� ����� ��
    void OnMouseExit(TextMeshProUGUI textObj)
    {
        textObj.color = originalColors[textObj];
        textObj.transform.localScale = originalScales[textObj];
    }
}
