using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGrid : MonoBehaviour
{
    private GridLayoutGroup _grComponent;

    private List<CButton> mButtons;

    private int mColumnCount = 1;

    public void init()
    {
        //Debug.Log("grid init!");

        getGrid();
        

        mButtons = new List<CButton>();
    }

    private void getGrid()
    {
        if (_grComponent == null)
        {
            _grComponent = gameObject.GetComponent<GridLayoutGroup>();
        }
    }
    public Vector2 getCellSize()
    {
        getGrid();

        return _grComponent.cellSize;
    }

    public void setColumnCount(int aCount)
    {
        //Debug.Log("Setting column count: " + aCount);
        mColumnCount = aCount;
    }

    public int getColumnCount()
    {
        return mColumnCount;
    }

    public void addButton(CButton aButton, bool mHidden = false)
    {
        //aButton.transform.SetParent(transform);
        //aButton.transform.SetAsLastSibling();
        //aButton.transform.localScale = Vector3.one;

        if (mHidden)
        {
            aButton.setHidden(true);
        }

        mButtons.Add(aButton);
    }

    public CButton getButton(int aIndex)
    {
        if (aIndex >= 0 && aIndex < mButtons.Count)
        {
            return mButtons[aIndex];
        }

        Debug.Log("index out of bounds!!");
        return null;
    }

    public void setButtonHidden(int aIndex, bool aHide = true)
    {
        if (aIndex >= 0 && aIndex < mButtons.Count)
        {
            mButtons[aIndex].setHidden(aHide);
        }
    }

    public int getIndexOfButton(string aId)
    {
        for (int i = 0; i < mButtons.Count; i++)
        {
            if (mButtons[i].getId() == aId)
            {
                return i;
            }
        }

        return -1;
    }

    public int getButtonCount()
    {
        return mButtons.Count;
    }

    public void clear()
    {
        for (int i = mButtons.Count - 1; i >= 0; i--)
        {
            if (mButtons[i] != null)
            {
                mButtons[i].destroy();
                mButtons[i] = null;
            }
        }

        mButtons.Clear();
    }

    public void destroy()
    {
        clear();
    }
}
