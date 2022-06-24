using UnityEngine;
using UnityEngine.EventSystems;
public class NarrativeCloser: MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{


        private bool mouseIsOver = false;

private void OnEnable()
{
    EventSystem.current.SetSelectedGameObject(gameObject);
}

public void OnDeselect(BaseEventData eventData)
{
    //Close the Window on Deselect only if a click occurred outside this panel
    if (!mouseIsOver)
        gameObject.SetActive(false);
}

public void OnPointerEnter(PointerEventData eventData)
{
    mouseIsOver = true;
    EventSystem.current.SetSelectedGameObject(gameObject);
}

public void OnPointerExit(PointerEventData eventData)
{
    mouseIsOver = false;
    EventSystem.current.SetSelectedGameObject(gameObject);
}
 }