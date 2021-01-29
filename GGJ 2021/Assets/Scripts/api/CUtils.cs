/// ----------------------------------------------------------------------------------------------------------------------    
/// <summary>
/// CUtils.
/// A utility class with static functions to convert between data types.
/// </summary>
/// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public static class CUtils
{
    // Provides information about a specific culture (called a locale for unmanaged code development). 
    // The information includes the names for the culture, the writing system, the calendar used, the sort order of strings,
    // and formatting for dates and numbers.
    private static CultureInfo mCulture;

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the string with the hexadecimal number corresponding to the color aColor. For example "FF0077".
    /// </summary>
    /// <param name="aColor">The color to be converted to hexadecimal string.</param>
    /// <returns>
    /// The string with the hexadecimal number corresponding to the color aColor. For example "FF0077".
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static string colorToHex(Color32 aColor)
    {
        string hex = aColor.r.ToString("X2") + aColor.g.ToString("X2") + aColor.b.ToString("X2");
        return hex;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the color corresponding to the string with the hexadecimal number aHex. 
    /// </summary>
    /// <param name="aHex">The string with the hexadecimal number. For example "FF0077".</param>
    /// <returns>
    /// The color corresponding to the string with the hexadecimal number aHex.
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static Color32 hexToColor(string aHex)
    {
        byte r = byte.Parse(aHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(aHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(aHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the int corresponding to converting the string aNumberString to int. Returns 0 if the string is not a valid number.
    /// </summary>
    /// <param name="aNumberString">The string with the number to be converted to int.</param>
    /// <returns>
    /// The int corresponding to converting the string aNumberString to int. Returns 0 if the string is not a valid number.
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static int stringToInt(string aNumberString)
    {
        int x = 0;
        Int32.TryParse(aNumberString, out x);
        return x;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the float corresponding to converting the string aNumberString to float. 
    /// Example of valid strings: "17", "-17.4", "3.5", ".1".
    /// </summary>
    /// <param name="aNumberString">The string with the number to be converted to float.</param>
    /// <returns>
    /// The float corresponding to converting the string aNumberString to float.
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float stringToFloat(string aNumberString)
    {
        if (mCulture == null)
        {
            // Creates and initializes the CultureInfo using USA.
            mCulture = new CultureInfo("en-US");
        }
        return Convert.ToSingle(aNumberString, mCulture);
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns the bool corresponding to converting the string aBooleanString to bool.
    /// Example of valid strings: "true" or "false";
    /// </summary>
    /// <param name="aBooleanString">The string with the boolean to be converted to bool.</param>
    /// <returns>
    /// The bool corresponding to converting the string aBooleanString to bool.
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static bool stringToBoolean(string aBooleanString)
    {
        if (mCulture == null)
        {
            mCulture = new CultureInfo("en-US");
        }
        return Convert.ToBoolean(aBooleanString, mCulture);
    }

    
}

/// <summary>
/// Used to get random numbers
/// </summary>
public static class ThreadSafeRandom
{
    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}

/// <summary>
/// Class used to be able to shuffle lists.
/// </summary>
static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

