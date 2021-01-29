/// ----------------------------------------------------------------------------------------------------------------------    
/// <summary>
/// CLocalData.
/// Class that manages local data, which is saved as localSave.xml in the persistentDataPath.
/// Data can be stored as strings, bools and ints.
/// Get the stored value by calling getStringValue(), getBoolValue() or getIntVale().
/// If the value hasn't been stored previously, it is added with the default value provided.
/// Set the stored value by calling setStringValue(), setBoolValue() or setIntValue().
/// ----------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using System;

// TODO: PlayerPrefs.DeleteAll(); HACER FUNCION PARA BORRAR EL SALVADO.

public class CLocalData
{
    // Reference to the instance of the class (singleton).
    private static CLocalData mInst = null;

    // The path to the local save.
#if UNITY_EDITOR
    private string mPath = "Assets/Data/localSave.xml";
#elif UNITY_TVOS
    private string mPath = Application.temporaryCachePath + "/localSave.xml";
#else
    private string mPath = Application.persistentDataPath + "/localSave.xml";
#endif

    // The reference to the XDocument.
    private XDocument mDocument;

    // Reference to the document elements to edit them separately.
    private Dictionary<string, XElement> mDocumentElements;

    // Used to keep cloud settings instead of default values if this is the first time creating the local file.
    private bool mLocalFileMissing = false;

    // The types of value stored.
    private const int TYPE_STRING = 0;
    private const int TYPE_BOOL = 1;
    private const int TYPE_INT = 2;

    // Dictionaries of the current local values.
    private Dictionary<string, string> mStringValues;
    private Dictionary<string, bool> mBoolValues;
    private Dictionary<string, int> mIntValues;

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Constructor. 
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    public CLocalData()
    {
        // Register the class as a singleton.
        registerSingleton();

        // Create each value dictionary.
        mStringValues = new Dictionary<string, string>();
        mBoolValues = new Dictionary<string, bool>();
        mIntValues = new Dictionary<string, int>();

        // Load the saved data.
        loadData();
    }

    //--------------------------------------------------------------------------------------------------------------------
    // SINGLETON.
    //--------------------------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Register the class as a singleton. Called from the constructor.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    private void registerSingleton()
    {
        // If the instance (static) is null, it means that this is the first object of this class we created.
        if (mInst == null)
        {
            // Save the reference to be returned by inst().
            mInst = this;
        }
        else
        {
            // If the instance is not null, there is already created a instace of this class. Don't let another.
            throw new UnityException("ERROR: Cannot create another instance of singleton class CLocalData.");
        }
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the only instance of this class (singleton).
    /// </summary>
    /// <returns>
    /// The only instance of this class (singleton).
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CLocalData inst()
    {
        return mInst;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Load the saved data.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    public void loadData()
    {
        // Try to load the information from the xml, if there is any.
        if (!File.Exists(mPath))
        {
            mLocalFileMissing = true;

            mDocument = new XDocument(new XDeclaration("1.0", "utf8", "yes"));
            XElement elem = new XElement("localData");

            mDocument.Add(elem);
            mDocument.Save(mPath);
        }
        else
        {
            StreamReader reader = new StreamReader(mPath);
            mDocument = XDocument.Parse(reader.ReadToEnd());
            reader.Close();

            //Debug.Log("**** Loading save from the Local Save **** mDocument: " + mDocument.ToString());
        }

        loadLocalDataFromDocument(mDocument);
    }

    /// <summary>
    /// Load the local save document.
    /// </summary>
    /// <param name="aDocument">The document to load.</param>
    private void loadLocalDataFromDocument(XDocument aDocument)
    {
        // Save the document as the current version.
        mDocument = aDocument;

        // Reset the document element's dictionary.
        mDocumentElements = new Dictionary<string, XElement>();

        List<XElement> localData = aDocument.Root.Elements().ToList();

        // For each value stored on the document.
        for (int i = 0; i < localData.Count; i++)
        {
            // Get the data key and value type.
            string key = localData[i].Attribute("key").Value;
            int valueType = CUtils.stringToInt(localData[i].Attribute("type").Value);

            // Save the element on the dictionary.
            // Note: Used to update elements separately without having to go through the hole document.
            mDocumentElements.Add(key, localData[i]);

            // If it's a string.
            if (valueType == TYPE_STRING)
            {
                // Get the value as string.
                string value = localData[i].Attribute("value").Value;

                // Add if not previously added.
                if (!mStringValues.ContainsKey(key))
                {
                    mStringValues.Add(key, value);
                }
                // Set if already contained.
                else
                {
                    mStringValues[key] = value;
                }
            }
            // If it's a bool.
            else if (valueType == TYPE_BOOL)
            {
                // Get the value as bool.
                bool value = CUtils.stringToBoolean(localData[i].Attribute("value").Value);

                // Add if not previously added.
                if (!mBoolValues.ContainsKey(key))
                {
                    mBoolValues.Add(key, value);
                }
                // Set if already contained.
                else
                {
                    mBoolValues[key] = value;
                }
            }
            // If it's an int.
            else if (valueType == TYPE_INT)
            {
                // Get the value as int.
                int value = CUtils.stringToInt(localData[i].Attribute("value").Value);

                // Add if not previously added.
                if (!mIntValues.ContainsKey(key))
                {
                    mIntValues.Add(key, value);
                }
                // Set if already contained.
                else
                {
                    mIntValues[key] = value;
                }
            }
        }
    }

    /// <summary>
    /// Update the value stored on the document. If the value changes, the document gets saved.
    /// </summary>
    /// <param name="aKey"></param>
    /// <param name="aValue"></param>
    /// <param name="aType"></param>
    private void updateOrCreateValueForKey(string aKey, string aValue, int aType)
    {
        // Bool to check if saving is needed.
        bool shouldSave = false;

        // If the element is already contained, update its value.
        if (mDocumentElements.ContainsKey(aKey))
        {
            XElement element = mDocumentElements[aKey];

            // Change the value if it's different to the current one.
            if (element.Attribute("value").Value != aValue)
            {
                element.Attribute("value").SetValue(aValue);

                // Save the document at the end.
                shouldSave = true;
            }
            // Sound and music changed type from bool to int so we need to update if necessary.
            if (element.Attribute("type").Value != aType.ToString())
            {
                element.Attribute("type").SetValue(aType.ToString());

                // Save the document at the end.
                shouldSave = true;
            }

        }
        // Else, create the element and add it to the document.
        else
        {
            // Create attributes.
            XAttribute key = new XAttribute("key", aKey);
            XAttribute type = new XAttribute("type", aType.ToString());
            XAttribute value = new XAttribute("value", aValue);

            // Create the element.
            XElement element = new XElement("data", key, type, value);

            // Add it to the document.
            mDocument.Root.Add("\n", element, "\n");

            // Add the reference to the dictionary.
            mDocumentElements.Add(aKey, element);

            // Save the document at the end.
            shouldSave = true;
        }

        if (shouldSave)
        {
            // Save the document locally.
            mDocument.Save(mPath);
        }

        // TODO: update cloud?
    }

    // -----------------------------------------------------------------------------------------------
    // SETTERS
    // -----------------------------------------------------------------------------------------------

    /// <summary>
    /// Save a string locally.
    /// </summary>
    /// <param name="aKey">The key stored on the file.</param>
    /// <param name="aValue">The value to store.</param>
    public void setStringValue(string aKey, string aValue)
    {
        if (aValue == null)
        {
            return;
        }

        Debug.Log("saving string for key: " + aKey + " value: " + aValue);

        if (mStringValues.ContainsKey(aKey))
        {
            mStringValues[aKey] = aValue;
        }
        else
        {
            mStringValues.Add(aKey, aValue);
        }

        updateOrCreateValueForKey(aKey, aValue, TYPE_STRING);
    }

    /// <summary>
    /// Save a bool locally.
    /// </summary>
    /// <param name="aKey">The key stored on the file.</param>
    /// <param name="aValue">The value to store.</param>
    public void setBoolValue(string aKey, bool aValue)
    {
        string valueString = aValue.ToString();

        Debug.Log("saving bool for key: " + aKey + " value: " + valueString);

        if (mBoolValues.ContainsKey(aKey))
        {
            mBoolValues[aKey] = aValue;
        }
        else
        {
            mBoolValues.Add(aKey, aValue);
        }

        updateOrCreateValueForKey(aKey, valueString, TYPE_BOOL);
    }

    /// <summary>
    /// Save an int locally.
    /// </summary>
    /// <param name="aKey">The key stored on the file.</param>
    /// <param name="aValue">The value to store.</param>
    public void setIntValue(string aKey, int aValue)
    {
        string valueString = aValue.ToString();

        Debug.Log("saving bool for key: " + aKey + " value: " + valueString);

        if (mIntValues.ContainsKey(aKey))
        {
            mIntValues[aKey] = aValue;
        }
        else
        {
            mIntValues.Add(aKey, aValue);
        }

        updateOrCreateValueForKey(aKey, valueString, TYPE_INT);
    }

    // -----------------------------------------------------------------------------------------------
    // GETTERS
    // -----------------------------------------------------------------------------------------------

    /// <summary>
    /// Get the string value stored. If not contained, the value is added to the local save with the default value.
    /// </summary>
    /// <param name="aKey"></param>
    /// <param name="aDefaultValue"></param>
    /// <returns></returns>
    public string getStringValue(string aKey, string aDefaultValue="")
    {
        // If the value isn't contained.
        if (!mStringValues.ContainsKey(aKey))
        {
            // Add the value to the local save with the default value.
            setStringValue(aKey, aDefaultValue);
        }

        return mStringValues[aKey];
    }

    /// <summary>
    /// Get the bool value stored. If not contained, the value is added to the local save with the default value.
    /// </summary>
    /// <param name="aKey"></param>
    /// <param name="aDefaultValue"></param>
    /// <returns></returns>
    public bool getBoolValue(string aKey, bool aDefaultValue)
    {
        // If the value isn't contained.
        if (!mBoolValues.ContainsKey(aKey))
        {
            // Add the value to the local save with the default value.
            setBoolValue(aKey, aDefaultValue);
        }

        return mBoolValues[aKey];
    }

    /// <summary>
    /// Get the int value stored. If not contained, the key is added to the local save with the default value.
    /// </summary>
    /// <param name="aKey"></param>
    /// <param name="aDefaultValue"></param>
    /// <returns></returns>
    public int getIntValue(string aKey, int aDefaultValue)
    {
        // If the value isn't contained.
        if (!mIntValues.ContainsKey(aKey))
        {
            // Add the value to the local save with the default value.
            setIntValue(aKey, aDefaultValue);
        }

        return mIntValues[aKey];
    }
}