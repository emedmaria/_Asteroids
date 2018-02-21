using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public abstract class BaseExplosion : ScriptableObject
	{
		[SerializeField]
		private ExplosionType explosionType;
		public ExplosionType ExplosionType { get { return explosionType; } }

		[SerializeField]
		public GameObject ParticleSysPrefab;
        public SoundFxPair SoundFX;
        //public AudioClip SoundFX;

		public Action ExplosionComplete; 

		public event EventHandler ExplosionFinished;
		protected virtual void OnExplosionFinished(object sender, EventArgs e )
		{
			if (ExplosionFinished != null)
				ExplosionFinished(this, e);
		}

		public virtual void Explode(SoundFxPair clip, ParticleSystem ps, Vector3 position)
		{
			PlaySFX(clip);
			PlayParticleSys(ps, position);

			OnExplosionFinished(this, EventArgs.Empty);

			if (ExplosionComplete != null)
				ExplosionComplete(); 
		}

		public abstract void PlaySFX(SoundFxPair clip);
		public abstract void PlayParticleSys(ParticleSystem ps, Vector3 position);

		void OnEnable()
		{
			//this.hideFlags = HideFlags.DontSave;
		}
	}
}
