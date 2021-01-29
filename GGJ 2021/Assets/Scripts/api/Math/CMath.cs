using UnityEngine;
using System.Collections;

public class CMath
{
    public const float INFINITY = float.MaxValue;
    public const float MIN_INFINITY = float.MinValue;

	public static int randomIntBetween(int aMin, int aMax)
	{
		return Random.Range (aMin, aMax + 1);
	}

	public static float randomFloatBetween(float aMin, float aMax)
	{
		return Random.Range (aMin, aMax);
	}

	public static float clampDeg(float aDeg)
	{
		aDeg = aDeg % 360.0f;
		if (aDeg < 0.0f) 
		{
			aDeg += 360.0f;
		}

		return aDeg;
	}

	public static bool pointInRect(float aX, float aY, float aRectX, float aRectY, float aRectWidth, float aRectHeight)
	{
		return (aX >= aRectX && aX <= aRectX + aRectWidth && aY >= aRectY && aY <= aRectY + aRectHeight);
	}

	public static float dist(float aX1, float aY1, float aX2, float aY2)
	{
		return Mathf.Sqrt((aX2 - aX1) * (aX2 - aX1) + (aY2 - aY1) * (aY2 - aY1));
	}

	public static float min(float aValue1, float aValue2)
	{
		if (aValue1 < aValue2)
		{
			return aValue1;
		}
		
		return aValue2;
	}
	
	// Convert from radians to degrees.
	public static float radToDeg(float aAngle)
	{
		return aAngle * 180 / Mathf.PI; 
		//TODO: return aAngle * Mathf.Rad2Deg;
	}
	
	// Convert from degrees to radians.
	public static float degToRad(float aAngle)
	{
		return aAngle * Mathf.PI / 180;
		// TODO: Optimizar pi/180.
	}

	//--------------------------------------------------------------------------------------------------------------------
    // INTERPOLATION FUNCTIONS.
    //--------------------------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Linear interpolation.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// https://en.wikipedia.org/wiki/Linear_interpolation 
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float lerp(float aStart, float aEnd, float aValue)
    {
        // Precise method which guarantees v = v1 when t = 1.
        return ((1 - aValue) * aStart) + (aValue * aEnd);

        // Imprecise method, which does not guarantee v = v1 when t = 1, due to floating-point arithmetic error.
        // return aStart + (aValue * (aEnd - aStart));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Linear interpolation.
    /// Returns the interpolation between the values (aDestStart, aDestEnd) for the parameter (aValue) in the closed unit 
    /// interval [aOriginStart, aOriginEnd].
    /// </summary>
    /// <param name="aOriginStart">The start value of the origin range.</param>
    /// <param name="aOriginEnd">The end value of the origin range.</param>
    /// <param name="aDestStart">The start value of the destination range.</param>
    /// <param name="aDestEnd">The end value of the destination range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [aOriginStart,aOriginEnd].</param>
    /// <returns>
    /// The interpolation between the values (aDestStart, aDestEnd) for the parameter (aValue) in the closed unit interval 
    /// [aOriginStart, aOriginEnd].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float lerp(float aOriginStart, float aOriginEnd, float aDestStart, float aDestEnd, float aValue)
    {
        //if both values are equal, aValue is returned because it is not necessary lerp (it gives indeterminancy)
        if (aOriginEnd == aOriginStart)
        {
            return aDestStart;
        }

        // Parse value to [0, 1]
        float parsedValue = (aValue - aOriginStart) / (aOriginEnd - aOriginStart);
        // Precise method which guarantees v = v1 when t = 1.
        return ((1 - parsedValue) * aDestStart) + (parsedValue * aDestEnd);
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Linear interpolation between two vectors.
    /// Returns the vector interpolation between two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector lerp(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(lerp(aStart.x, aEnd.x, aValue), lerp(aStart.y, aEnd.y, aValue), lerp(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Calculates the linear parameter t (between 0 and 1) that produces the interpolant value within the range [aStart, aEnd].
    /// https://docs.unity3d.com/ScriptReference/Mathf.InverseLerp.html
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The interpolant value to find t.</param>
    /// <returns>
    /// The linear parameter t (between 0 and 1) that produces the interpolant value within the range [aStart, aEnd]. 
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float inverseLerp(float aStart, float aEnd, float aValue)
    {
        return Mathf.InverseLerp(aStart, aEnd, aValue);
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Linear interpolation for angles.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// https://en.wikipedia.org/wiki/Linear_interpolation 
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1]. The result is given between 0 and 360.
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float lerpAngle(float aStart, float aEnd, float aValue)
    {
        float res = 0;
        if (Mathf.Abs(aEnd - aStart) >= 180)
        {
            if (aEnd > aStart)
            {
                aEnd -= 360;
            }
            else
            {
                aStart -= 360;
            }
        }
        // Precise method which guarantees v = v1 when t = 1.
        res = ((1 - aValue) * aStart) + (aValue * aEnd);
        while (res < 0)
        {
            res += 360;
        }
        while (res > 360)
        {
            res -= 360;
        }
        return res;
    }

    //--------------------------------------------------------------------------------------------------------------------
    // Functions taken from Mathfx interpolation library.
    // http://wiki.unity3d.com/index.php?title=Mathfx
    // Some functions have been modified to work with CVector class.
    //--------------------------------------------------------------------------------------------------------------------

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation while easing in and out at the limits.
    /// Has ease-in and ease-out of the values.	The interpolation will gradually speed up from the start and slow down toward the end.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float hermite(float aStart, float aEnd, float aValue)
    {
        return Mathf.Lerp(aStart, aEnd, aValue * aValue * (3.0f - 2.0f * aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation while easing in and out at the limits.
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector hermite(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(hermite(aStart.x, aEnd.x, aValue), hermite(aStart.y, aEnd.y, aValue), hermite(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation while easing around the end, when value is near one (ease out).
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float sinerp(float aStart, float aEnd, float aValue)
    {
        return Mathf.Lerp(aStart, aEnd, Mathf.Sin(aValue * Mathf.PI * 0.5f));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation while easing around the end, when value is near one (ease out).
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector sinerp(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(sinerp(aStart.x, aEnd.x, aValue), sinerp(aStart.y, aEnd.y, aValue), sinerp(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation similar to sinerp(), except it eases in, when value is near zero, instead of easing out (and uses cosine instead of sine).
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float coserp(float aStart, float aEnd, float aValue)
    {
        return Mathf.Lerp(aStart, aEnd, 1.0f - Mathf.Cos(aValue * Mathf.PI * 0.5f));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation similar to sinerp(), except it eases in, when value is near zero, instead of easing out (and uses cosine instead of sine).
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector coserp(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(coserp(aStart.x, aEnd.x, aValue), coserp(aStart.y, aEnd.y, aValue), coserp(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float berp(float aStart, float aEnd, float aValue)
    {
        aValue = Mathf.Clamp01(aValue);
        aValue = (Mathf.Sin(aValue * Mathf.PI * (0.2f + 2.5f * aValue * aValue * aValue)) * Mathf.Pow(1f - aValue, 2.2f) + aValue) * (1f + (1.2f * (1f - aValue)));
        return aStart + (aEnd - aStart) * aValue;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector berp(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(berp(aStart.x, aEnd.x, aValue), berp(aStart.y, aEnd.y, aValue), berp(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// A version of berp() with a more exagerating curve (better for UI animation). Check berp() documentation.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float berpCarloncho(float aStart, float aEnd, float aValue)
    {
        // Made by Carlos Molina.
        aValue = Mathf.Clamp01(aValue);
        float c = (-1 * ((Mathf.PI * Mathf.PI * aValue) - Mathf.PI));
        float b = (-1 * ((Mathf.PI * aValue) - Mathf.PI));
        aValue = ((Mathf.Cos(c) * b) + Mathf.PI) / Mathf.PI;
        return aStart + (aEnd - aStart) * aValue;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// A version of berp() with a more exagerating curve (better for UI animation). Check berp() documentation.
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector berpCarloncho(CVector aStart, CVector aEnd, float aValue)
    {
        return new CVector(berpCarloncho(aStart.x, aEnd.x, aValue), berpCarloncho(aStart.y, aEnd.y, aValue), berpCarloncho(aStart.z, aEnd.z, aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Returns a value between 0 and 1 that can be used to easily make bouncing GUI items (a la OS X's Dock).
    /// Returns the interpolation between the values 0 and 1 for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values 0 and 1 for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float bounce(float aValue)
    {
        return Mathf.Abs(Mathf.Sin(6.28f * (aValue + 1f) * (aValue + 1f)) * (1f - aValue));
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation that is a mix of Sinerp and Berp with multipliers.
    /// Returns the interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start value of the range.</param>
    /// <param name="aEnd">The end value of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The interpolation between the values (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static float sinerpXBerp(float aStart, float aEnd, float aValue, float aSinerpMultiplier = 1.0f, float aBerpMultiplier = 1.0f)
    {
        float sinerpValue = sinerp(aStart, aEnd, aValue) * aSinerpMultiplier;
        float berpValue = berp(aStart, aEnd, aValue) * aBerpMultiplier;
        return (sinerpValue + berpValue) / (aSinerpMultiplier + aBerpMultiplier);
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Interpolation that is a mix of Sinerp and Berp with multipliers.
    /// Returns the vector interpolation between the two vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </summary>
    /// <param name="aStart">The start vector of the range.</param>
    /// <param name="aEnd">The end vector of the range.</param>
    /// <param name="aValue">The value to interpolate, in the closed interval [0,1].</param>
    /// <returns>
    /// The vector interpolation between the vectors (aStart, aEnd) for the parameter (aValue) in the closed unit interval [0,1].
    /// </returns>
    /// ------------------------------------------------------------------------------------------------------------------
    public static CVector sinerpXBerp(CVector aStart, CVector aEnd, float aValue, float aSinerpMultiplier = 1.0f, float aBerpMultiplier = 1.0f)
    {
        return new CVector(sinerpXBerp(aStart.x, aEnd.x, aValue, aSinerpMultiplier, aBerpMultiplier),
                           sinerpXBerp(aStart.y, aEnd.y, aValue, aSinerpMultiplier, aBerpMultiplier),
                           sinerpXBerp(aStart.z, aEnd.z, aValue, aSinerpMultiplier, aBerpMultiplier));
    }
}
