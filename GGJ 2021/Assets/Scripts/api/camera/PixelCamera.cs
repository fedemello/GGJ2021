using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main camera component. Sets the camera resolution and movement to be pixel perfect.
/// </summary>
public class PixelCamera : MonoBehaviour
{
    //Things to improve in this class: 
    // -Add the posibility of custom resolutions with black bars;
    // -Change Unity screen width and screen height from code

    public static PixelCamera Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("PixelCamera").AddComponent<PixelCamera>();
            return _inst;
        }
    }

    private static PixelCamera _inst;

    [Space]
    [Header("Basic Settings")]
    [HideInInspector]
    public Camera myCamera;
    public Vector2 aspectRatio;
    public int realWidht;
    public int intendedWidth;
    [HideInInspector]
    public int height;
    [HideInInspector]
    public int gameZoom = 1;
    [Space]

    [Space]
    [Header("Pixel Settings")]
    public bool pixelArt;
    public int pixelsPerUnity = 1;
    [HideInInspector]
    public int pixelWidht;
    [HideInInspector]
    public int pixelHeight;
    [HideInInspector]
    public float currentOrthographicSize;
    [HideInInspector]
    public float nativeOrthographicSize;
    [Space]

    [Space]
    [Header("SubPixel Settings")]
    public bool subPixelSnap;
    [HideInInspector]
    public int currentZoom;
    [HideInInspector]
    public float snapGrid = 1f;

    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _inst = this;

        myCamera = gameObject.GetComponent<Camera>();
        cameraInicialSetting();
    }

    //One time settings

    public void cameraInicialSetting()
    {
        //Basic Settings

        gameZoom = realWidht / intendedWidth;

        height = Mathf.RoundToInt((realWidht / aspectRatio.x) * aspectRatio.y);

        pixelWidht = realWidht / gameZoom;
        pixelHeight = height / gameZoom;

        currentZoom = gameZoom;

        //Projection type

        if (pixelArt == true)
        {
            myCamera.orthographic = true;
            setOrthographicSize();
        }
        else if (pixelArt == false)
        {
            myCamera.orthographic = false;
        }
    }

    public void setOrthographicSize()
    {
        zooming(gameZoom);

        nativeOrthographicSize = currentOrthographicSize;

        snapGrid = 1f / pixelsPerUnity / currentZoom;
    }

    //Reusable functions

    public void nativeZoom()
    {
        myCamera.orthographicSize = nativeOrthographicSize;
        currentZoom = gameZoom;
    }

    public void zooming(int zoom)
    {
        currentZoom = zoom;
        currentOrthographicSize = ((float)(height / zoom) / (float)pixelsPerUnity) / 2f;

        myCamera.orthographicSize = currentOrthographicSize;
    }

    //Snapping

    public void subPixelSnapping()
    {
        Vector3 InicialPos = transform.position;
        float Snapping = snapGrid;

        Vector3 SnappedPos = new Vector3(Mathf.Round((InicialPos.x)/Snapping)*Snapping,
            Mathf.Round((InicialPos.y) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.z) / Snapping) * Snapping);

        transform.position = SnappedPos;
    }
    public void pixelSnapping()
    {
        Vector3 InicialPos = transform.position;
        float Snapping = snapGrid*currentZoom;

        Vector3 SnappedPos = new Vector3(Mathf.Round((InicialPos.x) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.y) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.z) / Snapping) * Snapping);

        transform.position = SnappedPos;
    }
}