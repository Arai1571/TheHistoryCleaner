using System.Collections.Generic;
using UnityEngine;
using System.Collections;
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
    [Header("BGM")]
    public AudioClip bgmInTitle; //タイトルBGM
    public AudioClip bgmInOpening; //オープニング
    public AudioClip bgmInGame; //ゲーム中
    public AudioClip bgmInBoss; //ボス
    public AudioClip bgmInEnding; //エンディング

    [Header("BGMVol")]
    [Range(0f, 1f)] public float bgmVolume = 1.0f;

    [Header("Opening：SE")]
    public AudioClip CatBot;
    public AudioClip CatBot1;
    public AudioClip CatBot2;
    public AudioClip GotBot1;
    public AudioClip GotBot2;
    public AudioClip GotBot3;
    public AudioClip RoboTalkingBug;
    public AudioClip BugNoise;

    [Header("Opening：SEVol")]
    [Range(0f, 2f)] public float seVolumeCatBot = 1.0f;
    [Range(0f, 2f)] public float seVolumeCatBot1 = 1.0f;
    [Range(0f, 2f)] public float seVolumeCatBot2 = 0.5f;
    [Range(0f, 2f)] public float seVolumeGotBot1 = 0.5f;
    [Range(0f, 2f)] public float seVolumeGotBot2 = 0.5f;
    [Range(0f, 2f)] public float seVolumeGotBot3 = 0.5f;
    [Range(0f, 2f)] public float seVolumeRoboTalkingBug = 0.5f;
    [Range(0f, 2f)] public float seVolumeBugNoise = 2.0f;

    [Header("Main：SE")]
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

    [Header("SEVol")]
    [Range(0f, 1f)] public float seVolumeShoot = 1.0f;
    [Range(0f, 1f)] public float seVolumeSpray = 1.0f;
    [Range(0f, 1f)] public float seVolumeAttack = 1.0f;
    [Range(0f, 1f)] public float seVolumeMoney = 1.0f;
    [Range(0f, 1f)] public float seVolumeDead = 1.0f;
    [Range(0f, 1f)] public float seVolumeHP0 = 1.0f;
    [Range(0f, 1f)] public float seVolumeOil = 1.0f;
    [Range(0f, 1f)] public float seVolumeGateOpen = 1.0f;
    [Range(0f, 1f)] public float seVolumeGateClosed = 1.0f;
    [Range(0f, 1f)] public float seVolumeWalk = 1.0f;
    [Range(0f, 1f)] public float seVolumeSmoke = 1.0f;
    [Range(0f, 1f)] public float seVolumePickup = 1.0f;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audio = GetComponent<AudioSource>();
    }
    
    // シーンが切り替わったら呼ばれるイベント
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log("🎵 Scene changed: " + scene.name);

    switch (scene.name)
    {
        case "Title":
            PlayBgm(BGMType.Title);
            break;

        case "Opening":
            PlayBgm(BGMType.Opening);
            break;

        case "Main":
            PlayBgm(BGMType.InGame);
            break;

        case "Boss":
            PlayBgm(BGMType.InBoss);
            break;

        case "Ending":
            PlayBgm(BGMType.ending);
            break;
    }
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
                    break;
                case BGMType.Opening:
                    audio.clip = bgmInOpening;
                    break;
                case BGMType.InGame:
                    audio.clip = bgmInGame;
                    break;
                case BGMType.InBoss:
                    audio.clip = bgmInBoss;
                    break;
                case BGMType.ending:
                    audio.clip = bgmInEnding;
                    break;
            }

            //ボリューム反映
            audio.volume = bgmVolume;
            audio.Play();
        }
    }

    //SE再生
    public void SEPlay(SEType type)
    {
        switch (type)
        {
            case SEType.Shoot:
                audio.PlayOneShot(seShoot, seVolumeShoot);
                break;
            case SEType.Spray:
                audio.PlayOneShot(seSpray, seVolumeSpray);
                break;
            case SEType.Attack:
                audio.PlayOneShot(seAttack, seVolumeAttack);
                break;
            case SEType.Money:
                audio.PlayOneShot(seMoney, seVolumeMoney);
                break;
            case SEType.Dead:
                audio.PlayOneShot(seDead, seVolumeDead);
                break;
            case SEType.HP0:
                audio.PlayOneShot(seHP0, seVolumeHP0);
                break;
            case SEType.Oil:
                audio.PlayOneShot(seOil, seVolumeOil);
                break;
            case SEType.GateOpen:
                audio.PlayOneShot(seGateOpen, seVolumeGateOpen);
                break;
            case SEType.GateClosed:
                audio.PlayOneShot(seGateClosed, seVolumeGateClosed);
                break;
            case SEType.Walk:
                audio.PlayOneShot(seWalk, seVolumeWalk);
                break;
            case SEType.Smoke:
                audio.PlayOneShot(seSmoke, seVolumeSmoke);
                break;
            case SEType.Pickup:
                audio.PlayOneShot(sePickup, seVolumePickup);
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