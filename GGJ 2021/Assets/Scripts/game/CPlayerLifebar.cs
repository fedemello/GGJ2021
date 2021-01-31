using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que se encarga de presentar la vida actual del jugador.
/// </summary>
public class CPlayerLifebar : MonoBehaviour
{
    // Se usa para determinar si la barra de vida es la izquierda o la derecha.
    public bool mFirstPlayer;

    // Sprite de vida.
    public SpriteRenderer mLife;
    // Máscara que va tapando la barra de vida a medida de que la vida del jugador disminuye.
    public SpriteMask mLifeMask;

    // Life sprite width.
    public const int LIFEBAR_WIDTH = 162;

    // Percent values are between 0 - 1
    public float mCurrentPercent = 1.0f;

    // Used to lerp the spriteMask until the current percent is 0.
    private float mBarXPos;

    // Used to lerp between the initial mask x pos and mBarXPos;
    private float mInitialMaskXPos;

    // Used to lerp between the initial mask scale and 1.
    private float mInitialMaskScale;

    private bool mInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!mInitialized)
        {
            initialize();
        }
        

    }

    public void initialize()
    {
        // Calculate the initial values.
        mBarXPos = mLife.transform.localPosition.x;

        mInitialMaskXPos = mLifeMask.transform.localPosition.x;
        mInitialMaskScale = mLifeMask.transform.localScale.x;

        mInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aPercent"></param>
    public void setCurrentPercent(float aPercent)
    {
        if (!mInitialized)
        {
            initialize();
        }

        if (aPercent != mCurrentPercent)
        {
            mCurrentPercent = Mathf.Clamp(aPercent, 0, 1);

            rePositionAndScaleMask();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float getCurrentPercent()
    {
        return mCurrentPercent;
    }

    /// <summary>
    /// 
    /// </summary>
    private void rePositionAndScaleMask()
    {
        // Calculate the new xScale
        float xScale = CMath.lerp(1, 0, mInitialMaskScale, 1, mCurrentPercent);

        mLifeMask.transform.localScale = new Vector3(xScale, 1, 1);

        // Calculate the new pos.
        float xPos = CMath.lerp(1, 0, mInitialMaskXPos, mBarXPos, mCurrentPercent);

        Vector3 pos = mLifeMask.transform.localPosition;

        mLifeMask.transform.localPosition = new Vector3(xPos, pos.y, pos.z);
    }
}
