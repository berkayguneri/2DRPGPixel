using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newxOffset = 0;
        float newyOffset = 0;

        if (mousePosition.x > xLimit)
            newxOffset = -xOffset;
        else
            newxOffset = xOffset;

        if (mousePosition.y > yLimit)
            newyOffset = -yOffset;
        else
            newyOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newxOffset, mousePosition.y + newyOffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * .8f;
    }
}
