using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public RectTransform indicatorPanel; // Yuvarlak panelin RectTransform'u
    public Button[] buttons; // Butonlarınız

    public void MoveIndicator(int buttonIndex)
    {
        // Seçili butonun pozisyonuna taşı
        indicatorPanel.anchoredPosition = buttons[buttonIndex].GetComponent<RectTransform>().anchoredPosition;
    }
}
