using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameScreen : MonoBehaviour
{
    protected RectTransform rect;
    protected bool isOpen;
    public float startX;

    public virtual void Initialize()
    {
        rect = GetComponent<RectTransform>();
        startX = rect.localPosition.x;
    }

    public virtual void Open()
    {
        isOpen = true;
        rect.localPosition = new Vector3(0, rect.localPosition.y, rect.localPosition.z);
    }

    public virtual void Close()
    {
        isOpen = false;
        rect.localPosition = new Vector3(startX, rect.localPosition.y, rect.localPosition.z);
    }

    public virtual void Toggle()
    {
        if (isOpen == true)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}