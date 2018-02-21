using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public enum ExplosionType
	{
		AsteroidBig,
		AsteroidSmall,
		Player,
		Enemy, 
        StarSystem
	}

    //  TODO: Separate SFX from Music
    public enum SoundFXType
    {
        MenuButton,
        LifeLost,
        LevelStart,
        LevelComplete,
        GameOver,
        Shot,
        StandardAmbience,
        SpaceAmbience,
        AsteroidBigExplosion,
        AsteroidSmallExplosion,
        PlayerCrash,
        None
    }
}
