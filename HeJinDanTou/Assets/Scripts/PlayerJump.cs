﻿using UnityEngine;
using System.Collections;
/// <summary>
/// 主角跳跃状态
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private GameObject hero;
    public Transform throwPos;
    public Transform shootPos_Up;
    public Transform shootPos_Down;
    public Transform shootPos_Hor;
    private float shootTime = 0;
    public GameObject projectile;
    public GameObject sld;
    public ShootDir shootDir = ShootDir.RIGHT;
    private shoot shoot;
    private PlayerMove playMove;
    public bool isShoot = false;
    public bool isThrow = false;
    //修正
    public SpriteRenderer srBody;//上身
    public SpriteRenderer srLeg;//下身

    public Sprite[] spBody_Up_Idle;//上身向上
    public Sprite[] spBody_Up_Shoot;//上身向上射击
    public Sprite[] spBody_Down_Shoot;//上身向下射击
    public Sprite[] spBody_Down_Idle;//上身向下
    public Sprite[] spBody_Hor_Shoot;//上身横向射击
    public Sprite[] spBody_Hor_Throw;//上身横向射击
    public Sprite[] spBody_Hor_Atk;//上身横向攻击
    public Sprite[] spBody_Hor_Idle;//上身横向静止
    public Sprite[] spLeg_Idle;//下身静止

    //动画速度
    public float animSpeedBody_Up_Idle = 30;
    public float animSpeedBody_Up_Shoot = 30;
    public float animSpeedBody_Down_Shoot = 5;
    public float animSpeedBody_Down_Idle = 5;
    public float animSpeedBody_Hor_Shoot = 5;
    public float animSpeedBody_Hor_Throw = 5;
    public float animSpeedBody_Hor_Atk = 5;
    public float animSpeedBody_Hor_Idle = 5;
    public float animSpeedLeg_Idle = 5;
    //动画间隔
    private float animIntervalBody_Up_Idle = 5;
    private float animIntervalBody_Up_Shoot = 5;
    private float animIntervalBody_Down_Shoot = 5;
    private float animIntervalBody_Down_Idle = 5;
    private float animIntervalBody_Hor_Shoot = 5;
    private float animIntervalBody_Hor_Throw = 5;
    private float animIntervalBody_Hor_Atk = 5;
    private float animIntervalBody_Hor_Idle = 5;
    private float animIntervalLeg_Idle = 5;
    //帧长度
    private int animLengthBody_Up_Idle = 5;
    private int animLengthBody_Up_Shoot = 5;
    private int animLengthBody_Down_Shoot = 5;
    private int animLengthBody_Down_Idle = 5;
    private int animLengthBody_Hor_Shoot = 5;
    private int animLengthBody_Hor_Throw = 5;
    private int animLengthBody_Hor_Atk = 5;
    private int animLengthBody_Hor_Idle = 5;
    private int animLengthLeg_Idle = 5;
    //计时器
    private float timerBody_Up_Idle = 0;
    private float timerBody_Up_Shoot = 0;
    private float timerBody_Down_Shoot = 0;
    private float timerBody_Down_Idle = 0;
    private float timerBody_Hor_Shoot = 0;
    private float timerBody_Hor_Throw = 0;
    private float timerBody_Hor_Atk = 0;
    private float timerBody_Hor_Idle = 0;
    private float timerLeg_Idle = 0;
    //帧
    private int animIndexBody_Up_Idle = 0;
    private int animIndexBody_Up_Shoot = 0;
    private int animIndexBody_Down_Shoot = 0;
    private int animIndexBody_Down_Idle = 0;
    private int animIndexBody_Hor_Shoot = 0;
    private int animIndexBody_Hor_Throw = 0;
    private int animIndexBody_Hor_Atk = 0;
    private int animIndexBody_Hor_Idle = 0;
    private int animIndexLeg_Idle = 0;
    //上身状态
    public BodyStatus bodyStatus = BodyStatus.Hor_Idle;
    public LegStatus legStatus = LegStatus.Idle;
    void Start()
    {
        hero = GameObject.Find("Player");
        animIntervalBody_Up_Idle = 1 / animSpeedBody_Up_Idle;
        animIntervalBody_Up_Shoot = 1 / animSpeedBody_Up_Shoot;
        animIntervalBody_Down_Shoot = 1 / animSpeedBody_Down_Shoot;
        animIntervalBody_Down_Idle = 1 / animSpeedBody_Down_Idle;
        animIntervalBody_Hor_Shoot = 1 / animSpeedBody_Hor_Shoot;
        animIntervalBody_Hor_Throw = 1 / animSpeedBody_Hor_Throw;
        animIntervalBody_Hor_Atk = 1 / animSpeedBody_Hor_Atk;
        animIntervalBody_Hor_Idle = 1 / animSpeedBody_Hor_Idle;
        animIntervalLeg_Idle = 1 / animSpeedLeg_Idle;

        animLengthBody_Up_Idle = spBody_Up_Idle.Length;
        animLengthBody_Up_Shoot = spBody_Up_Shoot.Length;
        animLengthBody_Down_Shoot = spBody_Down_Shoot.Length;
        animLengthBody_Down_Idle = spBody_Down_Idle.Length;
        animLengthBody_Hor_Shoot = spBody_Hor_Shoot.Length;
        animLengthBody_Hor_Throw = spBody_Hor_Throw.Length;
        animLengthBody_Hor_Atk = spBody_Hor_Atk.Length;
        animLengthBody_Hor_Idle = spBody_Hor_Idle.Length;
        animLengthLeg_Idle = spLeg_Idle.Length;

        shoot = hero.GetComponent<shoot>();
        playMove = hero.GetComponent<PlayerMove>();
    }
    // Update is called once per frame
    void Update()
    {
        //bodyStatus = BodyStatus.Hor_Throw;
        print(bodyStatus);
        //上身srBody= 向上射击、静止 横向射击、静止
        switch (bodyStatus)
        {
            case BodyStatus.Hor_Shoot:
                timerBody_Hor_Shoot += Time.deltaTime;
                if (timerBody_Hor_Shoot > animIntervalBody_Hor_Shoot)
                {
                    timerBody_Hor_Shoot -= animIntervalBody_Hor_Shoot;
                    animIndexBody_Hor_Shoot++;
                    animIndexBody_Hor_Shoot %= animLengthBody_Hor_Shoot;
                    srBody.sprite = spBody_Hor_Shoot[animIndexBody_Hor_Shoot];
                    if (animIndexBody_Hor_Shoot == 2)
                    {
                        Shoot();
                        Instantiate(projectile, new Vector3(shootPos.position.x, shootPos.position.y), Quaternion.Euler(0, 0, z_Rotation));
                        AudioSource.PlayClipAtPoint(playMove.soundShoot, new Vector3(0, 0, 0));
                    }
                    if (animIndexBody_Hor_Shoot == animLengthBody_Up_Shoot - 1)
                    {
                        isShoot = false;
                        bodyStatus = BodyStatus.Hor_Idle;
                    }
                }
                break;
            case BodyStatus.Hor_Atk:
                if (isShoot)
                {
                    timerBody_Hor_Atk += Time.deltaTime;
                    if (timerBody_Hor_Atk > animIntervalBody_Hor_Atk)
                    {
                        timerBody_Hor_Atk -= animIntervalBody_Hor_Atk;
                        animIndexBody_Hor_Atk++;
                        animIndexBody_Hor_Atk %= animLengthBody_Hor_Atk;
                        srBody.sprite = spBody_Hor_Atk[animIndexBody_Hor_Atk];
                        if (animIndexBody_Hor_Atk == 2)
                        {
                            AudioSource.PlayClipAtPoint(playMove.soundTieGuo, new Vector3(0, 0, 0));
                        }
                        if (animIndexBody_Hor_Atk == animLengthBody_Hor_Atk - 1)
                        {
                            isShoot = false;
                            bodyStatus = BodyStatus.Hor_Idle;
                        }
                    }
                }
                break;
            case BodyStatus.Hor_Idle:
                timerBody_Hor_Idle += Time.deltaTime;
                if (timerBody_Hor_Idle > animIntervalBody_Hor_Idle)
                {
                    timerBody_Hor_Idle -= animIntervalBody_Hor_Idle;
                    animIndexBody_Hor_Idle++;
                    animIndexBody_Hor_Idle %= animLengthBody_Hor_Idle;
                    srBody.sprite = spBody_Hor_Idle[animIndexBody_Hor_Idle];
                }
                break;
            case BodyStatus.Up_Idle:
                timerBody_Up_Idle += Time.deltaTime;
                if (timerBody_Up_Idle > animIntervalBody_Up_Idle)
                {
                    timerBody_Up_Idle -= animIntervalBody_Up_Idle;
                    animIndexBody_Up_Idle++;
                    animIndexBody_Up_Idle %= animLengthBody_Up_Idle;
                    srBody.sprite = spBody_Up_Idle[animIndexBody_Up_Idle];
                }
                break;
            case BodyStatus.Hor_Throw:
                timerBody_Hor_Throw += Time.deltaTime;
                if (timerBody_Hor_Throw > animIntervalBody_Hor_Throw)
                {
                    timerBody_Hor_Throw -= animIntervalBody_Hor_Throw;
                    animIndexBody_Hor_Throw++;
                    animIndexBody_Hor_Throw %= animLengthBody_Hor_Throw;
                    srBody.sprite = spBody_Hor_Throw[animIndexBody_Hor_Throw];
                    if (animIndexBody_Hor_Throw == 2)
                    {
                        Instantiate(sld, new Vector3(throwPos.position.x, throwPos.position.y), Quaternion.Euler(0, 0, 0));
                        //AudioSource.PlayClipAtPoint(playMove.soundShoot, new Vector3(0, 0, 0));
                    }
                    if (animIndexBody_Hor_Throw == animLengthBody_Hor_Throw - 1)
                    {
                        isThrow = false;
                        bodyStatus = BodyStatus.Hor_Idle;
                    }
                }
                break;
            case BodyStatus.Up_Shoot:
                timerBody_Up_Shoot += Time.deltaTime;
                if (timerBody_Up_Shoot > animIntervalBody_Up_Shoot)
                {
                    timerBody_Up_Shoot -= animIntervalBody_Up_Shoot;
                    animIndexBody_Up_Shoot++;
                    animIndexBody_Up_Shoot %= animLengthBody_Up_Shoot;
                    srBody.sprite = spBody_Up_Shoot[animIndexBody_Up_Shoot];
                    if (animIndexBody_Up_Shoot == 2)
                    {
                        Shoot();
                        Instantiate(projectile, new Vector3(shootPos.position.x, shootPos.position.y), Quaternion.Euler(0, 0, z_Rotation));
                        AudioSource.PlayClipAtPoint(playMove.soundShoot, new Vector3(0, 0, 0));
                    }
                    if (animIndexBody_Up_Shoot == animLengthBody_Up_Shoot - 1)
                    {
                        isShoot = false;
                        bodyStatus = BodyStatus.Up_Idle;
                    }
                }
                break;
            case BodyStatus.Down_Shoot:
                timerBody_Down_Shoot += Time.deltaTime;
                if (timerBody_Down_Shoot > animIntervalBody_Down_Shoot)
                {
                    timerBody_Down_Shoot -= animIntervalBody_Down_Shoot;
                    animIndexBody_Down_Shoot++;
                    animIndexBody_Down_Shoot %= animLengthBody_Down_Shoot;
                    srBody.sprite = spBody_Down_Shoot[animIndexBody_Down_Shoot];
                    if (animIndexBody_Down_Shoot == 2)
                    {
                        Shoot();
                        Instantiate(projectile, new Vector3(shootPos.position.x, shootPos.position.y), Quaternion.Euler(0, 0, z_Rotation));
                        AudioSource.PlayClipAtPoint(playMove.soundShoot, new Vector3(0, 0, 0));
                    }
                    if(animIndexBody_Down_Shoot==animLengthBody_Down_Shoot-1)
                    {
                        isShoot = false;
                        bodyStatus = BodyStatus.Down_Idle;
                    }
                }
                break;
            case BodyStatus.Down_Idle:
                timerBody_Down_Idle += Time.deltaTime;
                if (timerBody_Down_Idle > animIntervalBody_Down_Idle)
                {
                    timerBody_Down_Idle -= animIntervalBody_Down_Idle;
                    animIndexBody_Down_Idle++;
                    animIndexBody_Down_Idle %= animLengthBody_Down_Idle;
                    srBody.sprite = spBody_Down_Idle[animIndexBody_Down_Idle];
                }
                break;
            default:
                break;
        }
        #region leg
        //下身srLeg=
        switch (legStatus)
        {
            case LegStatus.Idle:
                timerLeg_Idle += Time.deltaTime;
                if (timerLeg_Idle > animIntervalLeg_Idle)
                {
                    timerLeg_Idle -= animIntervalLeg_Idle;
                    animIndexLeg_Idle++;
                    animIndexLeg_Idle %= animLengthLeg_Idle;
                    srLeg.sprite = spLeg_Idle[animIndexLeg_Idle];
                }
                break;
            default:
                break;
        }
        #endregion
    }
    Transform shootPos;
    float z_Rotation = 0;
    public void Shoot()
    {
        isShoot = true;
        if (bodyStatus == BodyStatus.Up_Shoot || bodyStatus == BodyStatus.Up_Idle)
        {
            bodyStatus = BodyStatus.Up_Shoot;
            shootPos = shootPos_Up;
            z_Rotation = -90;
        }
        else
        {
            print(bodyStatus);
            if (bodyStatus == BodyStatus.Down_Shoot || bodyStatus == BodyStatus.Down_Idle)
            {
                shootPos = shootPos_Down;
                z_Rotation = 90;
            }
            else
            {
                bodyStatus = BodyStatus.Hor_Shoot;
                if (playMove.isRightDir)
                {
                    shootPos = shootPos_Hor;
                    z_Rotation = 180;
                }
                else
                {
                    shootPos = shootPos_Hor;
                    z_Rotation = 0;
                }
            }
        }
    }
    public void Throw()
    {
        isThrow = true;
        bodyStatus = BodyStatus.Hor_Throw;
    }
}
