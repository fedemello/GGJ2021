// TODO: Revisar cuando retornar el vector...

using UnityEngine;
using System.Collections;

public class CVector
{
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	// Constructor.
	public CVector()
	{
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;
	}
	
	// Constructor.
	public CVector(float aX, float aY)
	{
		x = aX;
		y = aY;
		z = 0.0f;
	}
	
	// Constructor.
	public CVector(float aX, float aY, float aZ)
	{
		x = aX;
		y = aY;
		z = aZ;
	}
	
	public static CVector operator *(CVector aVector, float aScalar)
	{
		return new CVector(aVector.x * aScalar, aVector.y * aScalar, aVector.z * aScalar);
	}
	
	public static CVector operator +(CVector aVector1, CVector aVector2)
	{
		return new CVector(aVector1.x + aVector2.x, aVector1.y + aVector2.y, aVector1.z + aVector2.z);
	}
	
	public static CVector operator -(CVector aVector1, CVector aVector2)
	{
		return new CVector(aVector1.x - aVector2.x, aVector1.y - aVector2.y, aVector1.z - aVector2.z);
	}
	
	// Adds a vector to this vector.
	public void add(CVector aVector)
	{
		x += aVector.x;
		y += aVector.y;
		z += aVector.z;
	}
	
	// Substract a vector to this vector.
	public void subtract(CVector aVector)
	{
		x -= aVector.x;
		y -= aVector.y;
		z -= aVector.z;
	}
	
	// Multiply this vector by a value.
	public void mul(float aScalar)
	{
		x *= aScalar;
		y *= aScalar;
		z *= aScalar;
	}
	
	// Divides this vector by a value.
	public void div(float aScalar)
	{
		x /= aScalar;
		y /= aScalar;
		z /= aScalar;
	}
	
	// Generates a copy of this vector.
	public CVector clone()
	{
		return new CVector(x, y, z);
	}
	
	// Setea la x y la y a cero, por lo tanto, pone su largo en cero.
	public CVector zero()
	{
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;
		return this;
	}
	
	// Returna true si el vector es cero (si la x, la y, y por lo tanto su largo es cero).
	public bool isZero()
	{
		return x == 0.0f && y == 0.0f && z == 0.0f;
	}
	
	// Set the lenght or magnitude of this vector. 2D
	// Changing the lenght will change the x and the y, but not the angle of this vector.
	public void setLenght(float aLenght)
	{
		float angle = getAngle();
		x = Mathf.Cos(angle) * aLenght;
		y = Mathf.Sin(angle) * aLenght;
	}
	
	public float getLenght()
	{
		return Mathf.Sqrt(getLenghtSquared());
	}
	
	// Get the lenght of this vector, squared.
	public float getLenghtSquared()
	{
		return x * x + y * y;
	}
	
	// Sets the angle of the vector (angle in radians). 2D.
	// Changing the angle changes the x and y but retains the same lenght.
	public void setAngle(float aAngle)
	{
		float lenght = getLenght();
		x = Mathf.Cos(aAngle) * lenght;
		y = Mathf.Sin(aAngle) * lenght;
	}
	
	// Gets the angle of the vector in radians. 
	public float getAngle()
	{
		return Mathf.Atan2(y, x);
	}
	
	// Normalizes the vector. Equivalent to setting the lenght to one, but more efficient.
	// Returns a reference to this vector.
	public CVector normalize()
	{
		if (getLenght() == 0.0f)
		{
			x = 1.0f;
			return this;
		}
		
		float lenght = getLenght();
		x /= lenght;
		y /= lenght;
		return this;		
	}
	
	// Ensures the lenght of the vector is no longer than the given value.
	// Parameter: The maximum value this vector should be. If lenght is larger than max, it will be truncated to this value.
	// Returns: A reference to this vector.
	public CVector truncate(float aMax)
	{
		setLenght(CMath.min(aMax, getLenght()));
		return this;
	}
	
	// Reverses the direction of this vector.
	// Returns a reference to this vector.
	public CVector reverse()
	{
		x = -x;
		y = -y;
		return this;
	}
	
	// Whether or not this vector is normalized (if its lenght is equal to one).
	// Returns true if lenght is one, otherwise false.
	public bool isNormalized()
	{
		return getLenght() == 1.0f;
	}
	
	// Calculates the dot product of this vector and another given vector.
	// Returns the dot product of this vector and the one passed in as a parameter.
	public float dotProd(CVector aVector)
	{
		return x * aVector.x + y * aVector.y;
	}
	
	// Calculates the angle between two vectors.
	// Returns: The angle between the two given vectors.
	public static float getAngleBetween(CVector aVector1, CVector aVector2)
	{
		if (!aVector1.isNormalized())
			aVector1.clone().normalize();
			
		if (!aVector2.isNormalized())
			aVector2.clone().normalize();
			
		return Mathf.Acos(aVector1.dotProd(aVector2));
	}
	
	// Finds a vector that is perpendicular to this vector. 2D.
	// Returns: The perpendicular vector to this vector.
	public CVector getPerpendicularVector()
	{
		return new CVector(-y, x);
	}
	
	// Determines if a given vector is to the right or left of this vector.
	// Returns: -1 if to the left. +1 if to the right.
	public float sign(CVector aVector)
	{
		CVector p = getPerpendicularVector();
		return p.dotProd(aVector) < 0 ? -1 : 1;
	}
	
	// Calculates the distance from this vector to another given vector.
	// Returns: The distance from this vector to the vector passed as a parameter.
	public float dist(CVector aVector)
	{
		return Mathf.Sqrt(distSquared(aVector));
	}
	
	// Calculates the distance squared from this vector to another given vector.
	// Returns: The distance squared from this vector to the vector passed as a parameter.
	public float distSquared(CVector aVector)
	{
		float dx = aVector.x - x;
		float dy = aVector.y - y;
		return dx * dx + dy * dy;
	}
	
	// Indicates wheather this vector and another CVector instance are equal in value.
	// Returns: true if the other vector is equal in value, false if not.
	public bool equals(CVector aVector)
	{
		return x == aVector.x && y == aVector.y;
	}
	
	// Generates a string representation of this vector.
	// Returns: a string with the description of the vector.
	public string toString()
	{
		return "[CVector (x:" + x + " y:" + y + " z:" + z + "]";
	}

	public void destroy()
	{
	}
}
