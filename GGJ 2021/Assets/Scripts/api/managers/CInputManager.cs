using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CInputManager : MonoBehaviour
{
    private Dictionary<InputDevice, int> mDevices = new Dictionary<InputDevice, int>();

    private const int AVAILABLE = 0;
    private const int UNAVAILABLE = 1;

    private InputDevice mPlayerOne;
    private InputDevice mPlayerTwo;

    public static CInputManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Input Manager").AddComponent<CInputManager>();
            return _inst;
        }
    }
    private static CInputManager _inst;

    public void Start()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        _inst = this;

        //Debug.Log("creating new input manager");

        getDevices();

        // Suscribe to device attachment.
        InputManager.OnDeviceAttached += InputManager_OnDeviceAttached;
        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;
    }

    private void InputManager_OnDeviceDetached(InputDevice obj)
    {
        if (mDevices.ContainsKey(obj))
        {
            // If the device was currently being used.
            if (mDevices[obj] == UNAVAILABLE)
            {
                Debug.Log("player was left without controller.");

                if (obj == mPlayerOne)
                {
                    Debug.Log("playerOne left");
                    mPlayerOne = null;
                }
                else if (obj == mPlayerTwo)
                {
                    Debug.Log("playerTwo left");
                    mPlayerTwo = null;
                }
            }

            // Debug.Log("Removing detached device");
            mDevices.Remove(obj);

            CLevelManager.Inst.getCurrentState().notifyRemovedDevice(obj);
        }
        else
        {
           // Debug.Log("Device was never contained");
        }    
    }

    private void InputManager_OnDeviceAttached(InputDevice obj)
    {
        if (mDevices.ContainsKey(obj))
        {
            //Debug.Log("Device attached already contained!");
            return;
        }

        addDevice(obj);
    }

    /// <summary>
    /// Stores all devices.
    /// </summary>
    private void getDevices()
    {
        if (InputManager.Devices != null)
        {
            //Debug.Log("getting devices, count: " + InputManager.Devices.Count);

            for (int i = 0; i < InputManager.Devices.Count; i++)
            {
                InputDevice device = InputManager.Devices[i];

                // If previously added, continue.
                if (mDevices.ContainsKey(device))
                {
                    //Debug.Log("found the same device twice!!");
                    continue;
                }

                addDevice(device);
            }

            //Debug.Log("stored " + mDevices.Count + " devices.");
        }
    }

    public void addDevice(InputDevice aDevice)
    {
        Debug.Log("adding device: " + aDevice.Name);

        int availability = AVAILABLE;

        if (aDevice.IsAttached)
        {
            if (mPlayerOne == null)
            {
                mPlayerOne = aDevice;
                availability = UNAVAILABLE;
            }
            else if (mPlayerTwo == null)
            {
                mPlayerTwo = aDevice;
                availability = UNAVAILABLE;
            }
        }

        // Add the device.
        mDevices.Add(aDevice, availability);

        // If player found
        if (availability == UNAVAILABLE)
        {
            CLevelManager.Inst.setPlayerControllers(mPlayerOne, mPlayerTwo);
        }
    }

    private InputDevice getAvailableDevice()
    {
        List<InputDevice> keys = new List<InputDevice>(mDevices.Keys);

        for (int i = 0; i< keys.Count; i++)
        {
            // If the device is currently not being used.
            if (mDevices[keys[i]] == AVAILABLE && keys[i].IsAttached)
            {
                // Set to true.
                mDevices[keys[i]] = UNAVAILABLE;

                return keys[i];
            }
            else
            {
                //Debug.Log(keys[i] + "current being used");
            }
        }

        //Debug.Log("no available device, returning null");

        return null;
    }

    /// <summary>
    /// Get the player controller
    /// </summary>
    /// <param name="playerOne"></param>
    /// <returns></returns>
    public InputDevice getPlayerControl(bool playerOne = true)
    {
        if (playerOne)
        {
            if (mPlayerOne == null)
            {
                //Debug.Log("getting player one from available devices");

                getDevices();
            }

            return mPlayerOne;
        }
        else
        {
            if (mPlayerTwo == null)
            {
                //Debug.Log("getting player two from available devices");

                getDevices();
            }

            return mPlayerTwo;
        }
    }

    /// <summary>
    /// Returns true if any key was pressed.
    /// </summary>
    /// <returns></returns>
    public bool pressedAnything()
    {
        if (InputManager.ActiveDevice.AnyButtonIsPressed || Input.anyKeyDown)
        {
            return true;
        }


        return false;
    }

    /// <summary>
    /// Get any active device.
    /// </summary>
    /// <returns></returns>
    public InputDevice getActiveDevice()
    {
        return InputManager.ActiveDevice;
    }

    public void logDevices()
    {
        List<InputDevice> devices = new List<InputDevice>(mDevices.Keys);
        for (int i = 0; i < devices.Count; i++)
        {
            InputDevice device = devices[i];

            Debug.Log("device: " + device.Name + " isActive: " + device.IsActive + " isAttached: " + device.IsAttached + "available: " + mDevices[device].ToString());
        }
    }
}
