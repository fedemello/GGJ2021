using UnityEngine;
using System.Collections;

public class CGameState 
{
	private int mState = 0;
	private float mTimeState = 0.0f;

	public CGameState ()
	{
	}

	virtual public void init()
	{
	}

	virtual public void update()
	{
		mTimeState = mTimeState + Time.deltaTime;
	}

	virtual public void render()
	{
	}

	virtual public void destroy()
	{
	}

	virtual public void setState(int aState)
	{
		mState = aState;
		mTimeState = 0.0f;
	}

	public int getState()
	{
		return mState;
	}

	public void setTimeState(float aTimeState)
	{
		mTimeState = aTimeState;
	}

	public float getTimeState()
	{
		return mTimeState;
	}
}
