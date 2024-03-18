using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuartBossAttack : StateMachineBehaviour
{
    //referencia al controller principal
    //private Enemy enemy;
    private BossEnemy bossEnemy;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemy = animator.GetComponentInParent<Enemy>();
        bossEnemy = animator.GetComponentInParent<BossEnemy>();
        //pedimo a la pool correspondiente instanciar un proyectil
        PoolManager.instance.Pull(bossEnemy.enemyProjectile, bossEnemy.shootingPoint.position,
                                   Quaternion.LookRotation(bossEnemy.target.position - bossEnemy.shootingPoint.position));
        //pedimo a la pool correspondiente instanciar un proyectil
        PoolManager.instance.Pull(bossEnemy.enemyProjectile, bossEnemy.shootingPointDownRight.position,
                                   Quaternion.LookRotation(bossEnemy.target.position - bossEnemy.shootingPointUpRight.position));
        //pedimo a la pool correspondiente instanciar un proyectil
        PoolManager.instance.Pull(bossEnemy.enemyProjectile, bossEnemy.shootingPointUpRight.position,
                                   Quaternion.LookRotation(bossEnemy.target.position - bossEnemy.shootingPointDownRight.position));
        //pedimo a la pool correspondiente instanciar un proyectil
        PoolManager.instance.Pull(bossEnemy.enemyProjectile, bossEnemy.shootingPointDownLeft.position,
                                   Quaternion.LookRotation(bossEnemy.target.position - bossEnemy.shootingPointDownLeft.position));

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
