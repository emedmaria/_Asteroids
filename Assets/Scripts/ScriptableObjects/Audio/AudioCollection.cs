using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsteroidsClone
{
    [CreateAssetMenu(fileName = "Audio Collection", menuName = "__Asteroids/Audio/Audio Collection")]
    public class AudioCollection : ScriptableObject
    {
        [Header("General Sound Effects")]
        public List<SoundFxPair> AudioClips;

        [Header("Ambience music")]
        public List<SoundFxPair> AmbienceClips;

        public AudioClip GetSfx(SoundFXType type)
        {
            SoundFxPair pair = AudioClips.Find(x => x.Type == type);
            return (pair!=null ? pair.Clip : null);
        }

        public AudioClip GetMusic(SoundFXType type)
        {
            SoundFxPair pair = AmbienceClips.Find(x => x.Type == type);
            return (pair != null ? pair.Clip : null);
        }

        public SoundFxPair GetSfxPair(SoundFXType type) { return AudioClips.Find(x => x.Type == type); }
        public SoundFxPair GetMusicPair(SoundFXType type) { return AmbienceClips.Find(x => x.Type == type); }
    }

    [Serializable]
    public class SoundFxPair
    {
        public SoundFXType Type;
        public AudioClip Clip;
        public float Volume = 1.0f;
    }   
}

