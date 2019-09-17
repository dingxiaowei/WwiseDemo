using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventSounds : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public AK.Wwise.Event OnPointerDownSound;
    public AK.Wwise.Event OnPointerUpSound;
    public AK.Wwise.Event OnPointerEnterSound;
    public AK.Wwise.Event OnPointerExitSound;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownSound != null)
            OnPointerDownSound.Post(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterSound != null)
            OnPointerEnterSound.Post(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitSound != null)
            OnPointerExitSound.Post(gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnPointerUpSound != null)
            OnPointerUpSound.Post(gameObject);
    }
}
