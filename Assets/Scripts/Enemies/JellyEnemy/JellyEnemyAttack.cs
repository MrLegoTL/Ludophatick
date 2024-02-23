using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyEnemyAttack : StateMachineBehaviour
{
    //referencia al controller principal
    private Enemy enemy;
    //variable para controlar en que momento disapra el proyectil
    public float projectileFireTime = 0.15f;
    //booleana ara verificar si el proyectil ya ha sido disaprado
    private bool projectileFired = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<Enemy>();
        //restablece la variable del proyectil diparado al entrar em el estado
        projectileFired = false;
      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!projectileFired && stateInfo.normalizedTime >= projectileFireTime)
        {
            //pedimo a la pool correspondiente instanciar un proyectil
            PoolManager.instance.Pull(enemy.enemyProjectile, enemy.shootingPoint.position,
                                       Quaternion.LookRotation(enemy.target.position - enemy.shootingPoint.position));
            //marca el proyectil como diaparado
            projectileFired=true;
        }
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,
                                                  Quaternion.LookRotation(enemy.target.position - enemy.transform.position),
                                                  enemy.enemyAttackTurnSpeed * Time.deltaTime);
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
