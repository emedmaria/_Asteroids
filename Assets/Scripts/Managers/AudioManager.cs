using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class AudioManager : MonoBehaviourSingletonPersistent<AudioManager>
    {
		[SerializeField]
		private AudioSource sfxSource;          //  Reference (drag in inspector) to audio Source which play soundFX
        [SerializeField]
		private AudioSource musicSource;        //  Reference (drag in inspector) to audio Source which play music

        [SerializeField]
        private AudioCollection audioCollection;    //  Collection of SoundFX

        /// <summary>
        /// Plays the SFX.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="volume">The volume.</param>
        public void PlaySFX(AudioClip clip, float volume = 1.0f)
		{
			if (!sfxSource.isPlaying)
			{
				sfxSource.clip = clip;
				sfxSource.volume = volume;
				sfxSource.Play();
			}
			else {
				PlayDynamicSoundFX(clip, volume);
			}
		}

        public void PlaySFX(SoundFXType sfxType)
        {
            SoundFxPair pair = audioCollection.GetSfxPair(sfxType);
            if (pair == null) return;

            if (!sfxSource.isPlaying && pair != null)
            {
                sfxSource.clip = pair.Clip;
                sfxSource.volume = pair.Volume;
                sfxSource.Play();
            }
            else {
                PlayDynamicSoundFX(pair.Clip, pair.Volume);
			}
        }

        /// <summary>
        /// In case AudioSource is busy creates one dynamically 
        /// and plays the clip
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="volume">The volume.</param>
        private void PlayDynamicSoundFX(AudioClip clip, float volume = 1.0f)
        {
            //Create an empty game object
            GameObject sfxGO = new GameObject("DynamicSFX_" + clip.name);
            sfxGO.transform.SetParent(transform);

            //Create the source
            AudioSource source = sfxGO.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.Play();

            Destroy(sfxGO, clip.length);
        }


        /// <summary>
        /// Plays ambience music
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="volume">The volume.</param>
        public void PlayMusic(AudioClip clip, float volume = 1.0f)
		{
			if (musicSource.isPlaying && musicSource.clip == clip)
				return;

			musicSource.clip = clip;
			musicSource.loop = true;
			musicSource.volume = volume;
			musicSource.Play();
		}

        public void PlayMusic(SoundFXType sfxType)
        {
            SoundFxPair pair = audioCollection.GetMusicPair(sfxType);
            if (pair == null) return;

            AudioClip clip = pair.Clip; 

            if (musicSource.isPlaying && musicSource.clip == clip)
                return;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = pair.Volume;
            musicSource.Play();
        }

        public void StopMusic() { musicSource.Stop(); }


	}
}
