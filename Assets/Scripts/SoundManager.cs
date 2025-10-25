using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//BGMタイプ
public enum BGMType
{
    None,
    Title,
    Opening,
    InGame,
    InBoss,
    ending
}

//SEタイプ
public enum SEType
{
    Shoot,
    Spray,
    Attack,
    Money,
    Dead,
    HP0,
    Oil,
    GateOpen,
    GateClosed,
    Walk,
    Smoke,
    Pickup
}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle; //タイトルBGM
    public AudioClip bgmInOpening; //オープニング
    public AudioClip bgmInGame; //ゲーム中
    public AudioClip bgmInBoss; //ボス
    public AudioClip bgmInEnding; //エンディング

    public AudioClip seShoot;
    public AudioClip seSpray;
    public AudioClip seAttack;
    public AudioClip seMoney;
    public AudioClip seDead;
    public AudioClip seHP0;
    public AudioClip seOil;
    public AudioClip seGateOpen;
    public AudioClip seGateClosed;
    public AudioClip seWalk;
    public AudioClip seSmoke;
    public AudioClip sePickup;

    public static SoundManager instance; // シングルトンインスタンス
    public static BGMType playingBGM = BGMType.None; //再生中のBGM

    AudioSource audio;

    void Awake()
    {
        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンが切り替わっても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audio = GetComponent<AudioSource>();

    }

    //BGM再生
    public void PlayBgm(BGMType type)
    {
        if (type != playingBGM)
        {
            playingBGM = type;


            switch (type)
            {
                case BGMType.Title:
                    audio.clip = bgmInTitle;
                    audio.Play();
                    break;
                case BGMType.Opening:
                    audio.clip = bgmInOpening;
                    audio.Play();
                    break;
                case BGMType.InGame:
                    audio.clip = bgmInGame;
                    audio.Play();
                    break;
                case BGMType.InBoss:
                    audio.clip = bgmInBoss;
                    audio.Play();
                    break;
                case BGMType.ending:
                    audio.clip = bgmInEnding;
                    audio.Play();
                    break;
            }
        }
    }

    //SE再生
    public void SEPlay(SEType type)
    {
        switch (type)
        {
            case SEType.Shoot:
                audio.PlayOneShot(seShoot);
                break;
            case SEType.Spray:
                audio.PlayOneShot(seSpray);
                break;
            case SEType.Attack:
                audio.PlayOneShot(seAttack);
                break;
            case SEType.Money:
                audio.PlayOneShot(seMoney);
                break;
            case SEType.Dead:
                audio.PlayOneShot(seDead);
                break;
            case SEType.HP0:
                audio.PlayOneShot(seHP0);
                break;
            case SEType.Oil:
                audio.PlayOneShot(seOil);
                break;
            case SEType.GateOpen:
                audio.PlayOneShot(seGateOpen);
                break;
            case SEType.GateClosed:
                audio.PlayOneShot(seGateClosed);
                break;
            case SEType.Walk:
                audio.PlayOneShot(seWalk);
                break;
            case SEType.Smoke:
                audio.PlayOneShot(seSmoke);
                break;
            case SEType.Pickup:
                audio.PlayOneShot(sePickup);
                break;
        }
    }

    //停止メソッド
    public void StopBgm()
    {
        audio.Stop();
        playingBGM = BGMType.None;
    }

}