using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAudioManager : MonoBehaviour
{
    public static CAudioManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject go = new GameObject("Audio Manager");
                DontDestroyOnLoad(go);
                return go.AddComponent<CAudioManager>();
            }
            return _inst;
        }
    }
    private static CAudioManager _inst;

    private CAudioSource _music;

    private List<CAudioSource> _sfxs = new List<CAudioSource>();
    //private List<string> _sfxKeys;
    //private CAudioSource _sfx;

    private float _volumeMusic = 0.3f;
    private float _volumeSfx = 0.5f;

    public void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        //_sfxKeys = new List<string>();

        //Setup music.
        _music = new CAudioSource("music", _volumeMusic, gameObject);
        //_sfx = new CAudioSource(_volumeSfx, gameObject);
        
    }

    public void Update()
    {

        for (int i = _sfxs.Count - 1; i >= 0; i--)
        {
            CAudioSource source = _sfxs[i];

            if (source.finishedPlaying())
            {
                source.destroy();
                _sfxs.Remove(source);
            }
        }

        if (_music.isPlaying() && _music.finishedPlaying())
        {
            //_music.
            Debug.Log("music ended!");
        }
    }


    public void startMusic(AudioClip aAudioClip, bool aLoop = true)
    {
        _music.playAudio(aAudioClip, aLoop);
    }

    public bool isMusicPlaying()
    {
        return _music.isPlaying();
    }
    public bool isMusicPlaying(AudioClip aClip)
    {
        return _music.getAudioClip() == aClip;
    }
    

    public void playSfx(string id, AudioClip aSfx, bool onlyOnce = false)
    {
        if (onlyOnce && isSfxPlaying(id))
        {
            Debug.Log(string.Format("sfx {0} already being played", id));
            return;
        }

        createSfx(id, aSfx);
    }

    private void createSfx(string aId, AudioClip aSfx)
    {
        string id = aId;
        int i = 0;
        while(isSfxPlaying(id))
        {
            i += 1;
            id = aId + i.ToString();
        }

        CAudioSource source = new CAudioSource(id, _volumeSfx, gameObject);

        source.playAudio(aSfx, false);
        _sfxs.Add(source);
    }

    public bool isSfxPlaying(string aId)
    {
        for (int i = 0; i < _sfxs.Count; i++)
        {
            if (_sfxs[i].getId() == aId)
            {
                return true;
            }
        }

        return false;
    }

    public void stopSfx(string aId)
    {
        for (int i = _sfxs.Count -1; i >= 0 ; i--)
        {
            if (_sfxs[i].getId() == aId)
            {
                _sfxs[i].destroy();
                _sfxs.RemoveAt(i);
            }
        }
    }

    public void stopAllSfx()
    {
        for (int i = _sfxs.Count - 1; i >= 0; i--)
        {
            _sfxs[i].destroy();
            _sfxs.RemoveAt(i);
        }
    }


}
