using UnityEngine;
using UnityEngine.UI;

public class ToastModalWindow : ModalWindow<ToastModalWindow>
{
    [SerializeField] private Image m_IconImage;

    // Tempo di permanenza del toast window sullo schermo
    private float m_Delay = 3f;
    public float Delay => m_Delay;

    public void SetIcon(Sprite icon)
    {
        if (m_IconImage != null)
        {
            m_IconImage.sprite = icon;
        }
    }

    public void SetDelay(float seconds)
    {
        m_Delay = seconds;
    }

    public override void Show()
    {
        base.Show();

        if (Delay > 0)
        {
            Invoke(nameof(Close), Delay);
        }
    }
}