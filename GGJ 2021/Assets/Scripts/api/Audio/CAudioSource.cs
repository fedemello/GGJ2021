using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAudioSource
{
    private GameObject _gameObject;

    private string _id;
    private AudioSource _source;
    private AudioClip _clip;

    private float _volumen;

    private bool _playing = false;
    private bool _loops;

    private GameObject _parent;

    // Start is called before the first frame update
    public CAudioSource(string aId, float aVolume, GameObject aParent)
    {
        _id = aId;
        _gameObject = new GameObject(aId);

        

        _source = _gameObject.AddComponent<AudioSource>();
        _source.minDistance = 2690.9f;
        _source.maxDistance = 2717.8f;

        setVolume(aVolume);

        _parent = aParent;

        _gameObject.transform.SetParent(aParent.transform);
    }

    public string getId()
    {
        return _id;
    }

    public void playAudio(AudioClip aAudio, bool aLoop)
    {
        _clip = aAudio;

        _source.clip = _clip;

        _loops = aLoop;
        _source.loop = _loops;
        _source.Play();

        _playing = true;
    }

    public AudioClip getAudioClip()
    {
        return _clip;
    }

    public bool finishedPlaying()
    {
        if (_playing && !_loops)
        {
            if (!_source.isPlaying)
            {
                _playing = false;

                return true;
            }
        }

        return false;
    }

    public bool isPlaying()
    {
        return _playing;
    }

    public void setVolume(float aVolume)
    {
        _volumen = Mathf.Clamp(aVolume, 0, 1);
        _source.volume = _volumen;
    }

    public void destroy()
    {
        Debug.Log("destroying");
        _parent = null;

        _clip = null;
        _source = null;

        Object.Destroy(_gameObject);
    }
}
