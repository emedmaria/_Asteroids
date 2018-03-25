using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[CreateAssetMenu(fileName ="Ship Settings", menuName ="__Asteroids/Shared Data/ Ship Movement Template")]
	public class BaseShipSettings : ScriptableObject {

		//	Parameters to define Basic Ship movement
		public float Thrust = 1000f;
		public float Rotation = 500f;
		public float MaxSpeed = 20f;

		//public AudioSource CollisionAudio;
		//public AudioSource EngineAudio;
		//public AudioSource OffBoundaries; 
	}
}
