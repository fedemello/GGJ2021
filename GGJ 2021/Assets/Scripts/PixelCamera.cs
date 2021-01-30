using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
    public Camera MyCamera;
    public Vector2 AspectRatio;
    private int RealWidth = Screen.width;
    private int RealHeight = Screen.height;
    public int PxToPxRatio;
    private int IntendedWidth;
    [Space]

    //PX settings ---------------------------

    [Space]
    [Header("Pixel Settings")]
    public bool PixelClap;
    public int PixelsPerUnity = 1;
    private float pixelsPerUnitScale = 1;
    [HideInInspector]
    public int PixelWidth;
    [HideInInspector]
    public int PixelHeight;
    [HideInInspector]
    public float CurrentOrthographicSize;
    [HideInInspector]
    public float NativeOrthographicSize;
    [Space]

    [Space]
    [Header("SubPixel Settings")]
    public bool SubPixelSnap;
    [HideInInspector]
    public int CurrentZoom;
    [HideInInspector]
    public float SnapGrid = 1f;

    //Zoom variables -------------------------------------------------------------------------

    [Space]
    [Header("Zoom Settings")]

    [SerializeField]
    private bool smoovZoom = true;    
    
    [SerializeField]
    private float zoomScaleMax = 10f;
   
    [SerializeField]
    private float smoovZoomDuration = 0.5f; // In seconds
    private float zoomStartTime = 0f;
    private float zoomScaleMin = 1f;
    private float zoomCurrentValue = 1f;
    private float zoomNextValue = 1f;
    private float zoomInterpolation = 1f;
    private float zoomStart;

    //Utility
    private Coroutine _activeCoroutine;

    //Loop -------------------------------------------------------------------------------

    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _inst = this;

        MyCamera = gameObject.GetComponent<Camera>();
        CameraInicialSetting();
    }

    public void Update()
    {        
        if (RealHeight != Screen.height)
        {
            RealHeight = Screen.height;
            UpdateCameraScale();
        } 
        if (midZoom)
        {
            if (smoovZoom)
            {
                zoomInterpolation = (Time.time - zoomStartTime) / smoovZoomDuration;
            }
            else
            {
                zoomInterpolation = 1f; // express to the end
            }
            pixelsPerUnitScale = Mathf.Lerp(zoomCurrentValue, zoomNextValue, zoomInterpolation);
            UpdateCameraScale();
        }
    }

    //Settings ---------------------------------------------------------------------------

    public void CameraInicialSetting()
    {
        //Basic Settings

        IntendedWidth = RealWidth/PxToPxRatio;

        PixelWidth = RealWidth / PxToPxRatio;
        PixelHeight = RealHeight / PxToPxRatio;

        CurrentZoom = PxToPxRatio;

        MyCamera.orthographic = true;
        SetOrthographicSize();
    }

    public void SetOrthographicSize()
    {
        SetZoomImmediate(PxToPxRatio);

        NativeOrthographicSize = CurrentOrthographicSize;

        SnapGrid = 1f / PixelsPerUnity / CurrentZoom;
    }


    //Zoom functions -----------------------------------------------------------------------------------------------

    private void UpdateCameraScale()
    {
        float cameraSize = (RealHeight / (pixelsPerUnitScale * PixelsPerUnity)) * 0.5f;;
        // The magic formular from teh Unity Docs
        MyCamera.orthographicSize = cameraSize;
    }

    private bool midZoom { get { return zoomInterpolation < 1; } }

    private void SetUpSmoovZoom()
    {
        zoomStartTime = Time.time;
        zoomCurrentValue = pixelsPerUnitScale;
        zoomInterpolation = 0f;
    }

    public void SetPixelsPerUnit(int pixelsPerUnitValue)
    {
        PixelsPerUnity = pixelsPerUnitValue;
        UpdateCameraScale();
    }

    // Has to be >= zoomScaleMin
    public void SetZoomScaleMax(int zoomScaleMaxValue)
    {
        zoomScaleMax = Mathf.Max(zoomScaleMaxValue, zoomScaleMin);
    }

    public void SetSmoovZoomDuration(float smoovZoomDurationValue)
    {
        smoovZoomDuration = Mathf.Max(smoovZoomDurationValue, 0.0333f); // 1/30th of a second sounds small enough
    }

    // Clamped to the range [1, zoomScaleMax], Integer values will be pixel-perfect
    public void SetZoom(float scale)
    {
        SetUpSmoovZoom();
        zoomNextValue = Mathf.Max(Mathf.Min(scale, zoomScaleMax), zoomScaleMin);
    }

    // Clamped to the range [1, zoomScaleMax], Integer values will be pixel-perfect
    public void SetZoomImmediate(float scale)
    {
        pixelsPerUnitScale = Mathf.Max(Mathf.Min(scale, zoomScaleMax), zoomScaleMin);
        UpdateCameraScale();
    }

    public void ZoomIn()
    {
        if (!midZoom)
        {
            SetUpSmoovZoom();
            zoomNextValue = Mathf.Min(pixelsPerUnitScale + 1, zoomScaleMax);
        }
    }

    public void ZoomOut()
    {
        SetUpSmoovZoom();
        zoomNextValue = Mathf.Max(pixelsPerUnitScale - 1, zoomScaleMin);
    }


    //Utility------------------------------------------------------------------------------------------------------

    //Snapping------------------------------------------------------------------------------------------------------

    public Vector3 SubPixelSnapping(Vector3 InicialPos)
    {
        float Snapping = SnapGrid;

        Vector3 SnappedPos = new Vector3(Mathf.Round((InicialPos.x)/Snapping)*Snapping,
            Mathf.Round((InicialPos.y) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.z) / Snapping) * Snapping);

        return SnappedPos;
    }
    public Vector3 PixelSnapping(Vector3 InicialPos)
    {
        float Snapping = SnapGrid*CurrentZoom;

        Vector3 SnappedPos = new Vector3(Mathf.Round((InicialPos.x) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.y) / Snapping) * Snapping, 
            Mathf.Round((InicialPos.z) / Snapping) * Snapping);

        return SnappedPos;
    }

    public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
    {
        float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
        return adjustedUnityUnits;
    }
}