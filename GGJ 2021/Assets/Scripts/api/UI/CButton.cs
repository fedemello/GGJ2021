using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using System;

public enum EButtonEvents { BUTTON_SELECT, BUTTON_DESELECT };

public class CButton : MonoBehaviour
{
    private List<Image> mImages = new List<Image>();
    protected TextMeshProUGUI mMainText;
    protected string mId;

    private Dictionary<EButtonEvents, Action> mEvents = new Dictionary<EButtonEvents, Action>();

    //public delegate void onButtonSelectedDelegate();
    //public static event onButtonSelectedDelegate onButtonSelected;

    //public delegate void onButtonDeselectedDelegate();
    //public static event onButtonDeselectedDelegate onButtonDeselected;

    private bool mHidden = false;

    /// <summary>
    /// Setup the button and its initial values.
    /// </summary>
    /// <param name="aId"></param>
    /// <param name="aImage"></param>
    /// <param name="aText"></param>

    public void setup(string aText, Sprite aImage, Action aOnClick = null, Action aOnUnclick = null)
    {

        mId = aText;

        mMainText = GetComponentInChildren<TextMeshProUGUI>();

        mImages = GetComponentsInChildren<Image>().ToList();

        if (mImages[0].sprite == null)
        {
            mImages[0].sprite = aImage;
        }
        //mImages[0].sprite = aImage;
        mMainText.text = aText;

        if (aOnClick != null)
        {
            addEvent(EButtonEvents.BUTTON_SELECT, aOnClick);
        }
        if (aOnUnclick != null)
        {
            addEvent(EButtonEvents.BUTTON_DESELECT, aOnUnclick);
        }

        //action act = new action(aAction);

        //mActions += aAction;
    }

    public string getId()
    {
        return mId;
    }

    public void addEvent(EButtonEvents aType, Action aAction)
    {
        if (!mEvents.ContainsKey(aType))
        {
            
            //Debug.Log("adding event for the first time");
            mEvents.Add(aType, aAction);
        }
        else
        {
            Debug.Log("adding aditional event");
            mEvents[aType] += aAction;
        }
    }

    public void removeEvent(EButtonEvents aType, Action aAction)
    {
        if (mEvents[aType] != null)
        {
            mEvents[aType] -= aAction;
        }
        if (mEvents[aType] == null)
        {
            mEvents.Remove(aType);
        }
    }

    public Image getImageByIndex(int aIndex)
    {
        if (aIndex >= 0 && aIndex < mImages.Count)
        {
            return mImages[aIndex];
        }

        Debug.Log("error, index out of bounds");
        return null;
    }

    public void setHidden(bool aHide)
    {
        if (aHide)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        mHidden = aHide;
    }

    public bool isHidden()
    {
        return mHidden;
    }

    public void onButtonClicked()
    {
        //Debug.Log("button clicked!");
        if (mEvents.ContainsKey(EButtonEvents.BUTTON_SELECT))
        {
            mEvents[EButtonEvents.BUTTON_SELECT]();
        }
        else
        {
            Debug.Log("button does not have a select action");
        }
    }

    public void onButtonUnclicked()
    {
        if (mEvents.ContainsKey(EButtonEvents.BUTTON_DESELECT))
        {
            mEvents[EButtonEvents.BUTTON_DESELECT]();
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}