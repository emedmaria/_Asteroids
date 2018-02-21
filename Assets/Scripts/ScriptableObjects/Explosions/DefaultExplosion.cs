using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[CreateAssetMenu(fileName = "New Explosion", menuName = "__Asteroids/Explosion/DefaultExplosion")]
	public class DefaultExplosion : BaseExplosion
	{
		/*
		[SerializeField]
		[Range(1f,3f)]
		private float ParticlesLifeTime = 2f;
		*/

		public override void PlaySFX(SoundFxPair clip)
		{
            if(clip!=null)
			    AudioManager.Instance.PlaySFX(clip.Clip, clip.Volume);
		}

		public override void PlayParticleSys(ParticleSystem ps, Vector3 position)
		{
			ps.transform.position = position;           
			ps.Play();
		}

		public void DisablePs(ParticleSystem ps)
		{
			ps.Stop();
			ps.gameObject.SetActive(false);
		}

		IEnumerator ParticlesDie(float time, ParticleSystem ps)
		{
			yield return new WaitForSeconds(time);
			DisablePs(ps);
		}
	}
}
