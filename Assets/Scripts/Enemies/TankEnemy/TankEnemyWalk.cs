using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PARA HACER USO DEL NAVMESH
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TankEnemyWalk : StateMachineBehaviour
{
    private Enemy enemy;
    private TankDeath tank;
    //Distancia minima para realizar un ataque cercano
    public float stompAttackDistance = 2f;
    public bool shoot = true;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //recuperamos la referencia al enemy propetario del animator
        enemy = animator.GetComponentInParent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.nav != null && enemy.nav.isActiveAndEnabled)
        {
            //float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);
            //si esta activo el navmesh agent y exite un objetivo
            if (enemy.nav.enabled && enemy.target != null)
            {
                //indicamos que el destino es el objetivo del agent
                enemy.nav.SetDestination(enemy.target.position);
            }

            //verificamos is el enemigo se encuentra a la distancia de ataque
            if (!enemy.nav.pathPending && enemy.nav.remainingDistance <= enemy.attackDistance && enemy.enemyShoot)
            {
                //cambiamos al estado de ataque
                animator.SetBool("Attack", true);
            }
            if (!enemy.nav.pathPending && enemy.nav.remainingDistance <= enemy.attackDistance && !enemy.enemyShoot)
            {
                animator.SetBool("Attack", false);
                //cambiamos al estado de ataque
                animator.SetTrigger("Stomp");
            }



        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //reseteamos el trigger al salir del estado, para evitar que se dispare la animacion mas de una vez
        //animator.ResetTrigger("Attack");
        animator.ResetTrigger("Stomp");
    }

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
