using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuartBossLaserAttack : StateMachineBehaviour
{
    private BossEnemy bossEnemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossEnemy = animator.GetComponentInParent<BossEnemy>();
        Vector3 laserDirection = bossEnemy.transform.forward;
        //// Crea una instancia del láser
        //LaserBeam laser = Instantiate(laserPrefab, transform.position, Quaternion.LookRotation(laserDirection));
        PoolManager.instance.Pull(bossEnemy.enemyLaser, bossEnemy.shootingPointLaser.position, Quaternion.LookRotation(laserDirection));

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossEnemy.transform.rotation = Quaternion.Slerp(bossEnemy.transform.rotation,
                                                 Quaternion.LookRotation(bossEnemy.target.position - bossEnemy.transform.position),
                                                 bossEnemy.enemyAttackTurnSpeed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
