using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinumumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgMusic;

    private bool canPlaySfx;

    public bool playBgm;
    private int bgmIndex;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
    }


    private void Update()
    {
        if(!playBgm)
            StopAllBGMusic();
        else
        {
            if (!bgMusic[bgmIndex].isPlaying)
                PlayBGMusic(bgmIndex);
        }
    }
    public void PlaySFX(int _sfxIndex,Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //    return;

        if (canPlaySfx == false)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinumumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();


    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));
    IEnumerator DecreaseVolume(AudioSource _auido)
    {
        float defaultVolume = _auido.volume;

        while (_auido.volume > .1f)
        {
            _auido.volume -= _auido.volume * .2f;
            yield return new WaitForSeconds(.4f);

            if (_auido.volume <= .1f)
            {
                _auido.Stop();
                _auido.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex=Random.Range(0, bgMusic.Length);
        PlayBGMusic(bgmIndex);
    }

    public void PlayBGMusic(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGMusic();
        bgMusic[bgmIndex].Play();
    }

    public void StopAllBGMusic()
    {
        for (int i = 0; i < bgMusic.Length; i++)
        {
            bgMusic[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySfx = true;
}
