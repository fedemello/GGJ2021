using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpriteLoader
{
    private static Sprite mButtonSprite;
    private static Sprite mTournamentButtonSprite;


    public static Sprite getButtonSprite()
    {
        if (mButtonSprite == null)
        {
            mButtonSprite = Resources.Load<Sprite>("Sprites/UI/boton");
        }

        return mButtonSprite;
    }

    public static Sprite getTournamentButtonSprite()
    {
        if (mTournamentButtonSprite == null)
        {
            mTournamentButtonSprite = Resources.Load<Sprite>("Sprites/UI/tournamentButton");
        }

        return mTournamentButtonSprite;
    }
}
