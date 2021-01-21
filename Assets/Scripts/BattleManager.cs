using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;

public class BattleManager : IActorManagerInterface
{
    StateManager sm;
    public Action<int,Vector3,Vector3> hitPointEvent;
    private void Awake() 
    {
        hitPointEvent += InitialBloodOnSurface;
    }
    private void Start() {
        sm = am.sm;
    }

    public bool DealDamge(ActorManager Enemyam,float damage,bool defenceVaild,bool counterVaild)
    {
        if(sm.isImmortal) return false;
        

        if(sm.isCountBack)
        {
            if(sm.isCountBackSuccess && counterVaild)
            {
                Enemyam.Stunned();
                am.im.overlapEcastms[0].active = true;
                return false;
            }
            else
            {
                Hurt(damage,false);
                return true;
            }
        }

        else if(sm.isDefence && defenceVaild)
        {
            am.Blocked();
            am.sfxm.PlayShield();
            return false;
        }
        else 
        {
            Hurt(damage,true);
            return true;
        }
    }

    public void Hit()
    {
        am.ac.animator.SetTrigger("hit");
    }

    public void Die()
    {
        if(!sm.isDead)
        {
            am.ac.animator.SetTrigger("die");
            am.ac.playerInput.InputToggle(false);
            if(am.ac.cc && am.ac.cc.cameraStatus == CameraStatus.LOCKON)
            {
                //CameraController.instance.LockOff();
            }
        }
        
    }



    public void Hurt(float damage,bool doHitAnimation)
    {
        if(sm.hp <= 0)
        {
            //already dead;
            return ;
        }

        sm.AddHp(-damage);
        
        if (sm.hp > 0)
        {
            if(doHitAnimation)
            {
                Hit();
                am.sfxm.OnHit();
            }
        }
        else if(sm.hp <= 0 && !sm.isDead)
        {
            am.sfxm.OnDead();
            Die();
        }
    }

    IEnumerator StopTimeForSeconds(float seconds)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1.0f;
    }


    public static bool CheckAnglePlayer(GameObject player,GameObject target,float angleLimit)
    {
        Vector3 dir = target.transform.position - player.transform.position;
        float angle1 = Vector3.Angle(player.transform.forward,dir);
        float angle2 = Vector3.Angle(target.transform.forward,player.transform.forward);

        return angle1 < angleLimit && Mathf.Abs(angle2 - 180) < angleLimit;
    }

    public static bool CheckAngleTarget(GameObject player,GameObject target,float angleLimit)
    {
        Vector3 attackDir =  player.transform.position - target.transform.position;

        float defenceAngle = Vector3.Angle(target.transform.forward,attackDir);
        return defenceAngle < angleLimit;
    }

    //对于所有damage类型，调用该一次该函数
    private float ComputerDamage(StateManager enemySm,bool rh)
    {
        Damage weaponPanelATK = sm.GetPanelATK(rh);
        Damage damage = Damage.ComputerFinalDamage(weaponPanelATK,enemySm.defenceRate);
        return damage.Total() ;
    }


    //TODO:拆分为飞行道具、近战武器；区分玩家和AI的伤害计算方式.
    public bool TryDoDmage(GameObject hitObject,Vector3 hitPoint,Vector3 normal,bool flyWeapon)
    {
        int layer = am.whatIsEnemy >> hitObject.layer;
        //对于物体击中敌人
        if(layer == 1 )
        {
            ActorManager enemyAm = hitObject.GetComponent<ActorManager>();
            StateManager enemySm = enemyAm.sm;


            //获取攻守双方模型
            GameObject attacker = am.ac.model;
            GameObject receiver = enemyAm.ac.model;

            //计算伤害,如果没有设置r0l1 默认左手.
            bool hand = true;
            float damage;


            //顺序
            //1.无敌
            //2.弹反成功
            //3.防御
            //4.其他
            if (enemySm.isImmortal) return false;

            if (enemySm.isCountBack)
            {
                if (enemySm.isCountBackSuccess)
                {
                    bool counterVaild = flyWeapon ? false : CheckAnglePlayer(attacker, receiver, 45.0f);
                    if (!counterVaild)
                    {
                        hitPointEvent.Invoke(hitObject.layer, hitPoint, normal);
                        damage = ComputerDamage(enemyAm.sm, hand);
                        enemyAm.bm.Hurt(damage, false);
                        return true;
                    }

                    am.Stunned();
                    am.im.overlapEcastms[0].active = true; //防止被重复处决
                    return false;
                }
                else
                {
                    damage = ComputerDamage(enemyAm.sm, hand);

                    hitPointEvent.Invoke(hitObject.layer, hitPoint, normal);

                    //顿帧，增加打击感
                    //StartCoroutine("StopTimeForSeconds", 0.05f);
                    enemyAm.bm.Hurt(damage, false);
                    return true;
                }
            }
            else if (enemySm.isDefence)
            {
                bool defenceVaild = CheckAngleTarget(attacker, receiver, 50.0f);
                if (!defenceVaild)
                {
                    hitPointEvent.Invoke(hitObject.layer, hitPoint, normal);
                    damage = ComputerDamage(enemyAm.sm, hand);
                    enemyAm.bm.Hurt(damage, true);
                    return true;
                }
                enemySm.am.Blocked();
                enemyAm.sfxm.PlayShield();
                InitialSparkOnShield(hitPoint, normal);
                return false;
            }
            else
            {
                hitPointEvent.Invoke(hitObject.layer, hitPoint, normal);
                damage = ComputerDamage(enemyAm.sm, hand);
                enemyAm.bm.Hurt(damage, true);
                return true;
            }


        }
        else
        {
            //TODO:弹刀
            return false;
        }
    }

    void InitialBloodOnSurface(int layer,Vector3 hitPoint,Vector3 normal)
    {
        GameObject blood = ObjectPool.instance.GetObject("Blood");
        blood.transform.position = hitPoint;
        //TODO:应该朝向切线方向
        blood.transform.forward = normal != Vector3.zero ? normal : blood.transform.forward;
        blood.GetComponent<ParticleSystem>().Play();
    }

    public void InitialSparkOnShield(Vector3 hitPoint,Vector3 normal)
    {
        GameObject spark = ObjectPool.instance.GetObject("Spark");
        spark.transform.position = hitPoint;
        spark.transform.forward = normal != Vector3.zero ? normal : spark.transform.forward;
        spark.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator ReSetAttackSpeed()
    {
        float passTime = am.ac.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        am.ac.animator.SetFloat("attackSpeed",-1.0f);
        while(true)
        {
            //Time.timeScale = 0.5f;
            if(am.ac.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.1f)
            {
                am.ac.animator.CrossFade("Ground",0.1f,0);
                break;
            }
            yield return null;
        }
        
        //yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1.0f;
        am.ac.animator.SetFloat("attackSpeed",1.0f);
    }
}
