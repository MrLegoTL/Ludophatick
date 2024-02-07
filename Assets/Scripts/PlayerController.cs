using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES
    //muestra u oculta los gizmos del player
    public bool showGizmos = false;

    [Header("Player Movement")]
    public float movementSpeed = 8f;
    public float rotationSpeed = 14f;
    public float acceleration = 30f;
    private Vector3 direction;
    private Vector3 desiredVelocity;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool isMoving = false;

    [Header("Jump")]
    public float jumpForce = 10f;
    public bool canJump = false;

    [Header("Physics")]
    public Rigidbody rb;
    //layers considerados ground
    public LayerMask groundLayer;
    //transform para la posicion de chekeo de ground
    public Transform groundCheck;
    //booleana para especificar si estamos grounded
    private bool grounded;
    //tamaño del groundcheck
    public Vector3 groundCheckSize;

    [Header("Collision Pre-Detection")]
    public LayerMask checkLayer;
    public Transform checkPoint;
    public float checkSize = 0.3f;
    [Range(0, 3)]
    public float checkDistance = 2f;
    public bool walled;

    [Header("Shooting")]
    public float shootDelay = 0.5f;
    private float shootTime = 0f;
    private bool leftGun = true;
    public Transform gunLeft;
    public Transform gunRight;
    //id de la pool de la cual recuperar los proyectiles
    public string bulletType = "RegularBullets";

    [Header("Aiming")]
    //longitud del raycas a realizar
    public float camRayLenght;
    //layer que podras ser "tocado" con el cursor del raton
    public LayerMask pointerLayer;
    //transform usado como objetivo para la rotacion
    public Transform aimingPivot;
    //para almacenar la referencia de la camara principal
   public Camera cameraMain;


    [Header("Animator")]
    public Animator anim;

    //solo vamos a comprobar si es mayor que 0, asin que no necesitamos mas capacidad
    Collider[] colliderBuffer = new Collider[1];

    #endregion
    #region EVENTS

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GrounCheck();
        CollisionPreDetection();
        Controls();
        Movement();
        AnimatorFeed();
        AimingBehaviour();
    }

    private void OnDrawGizmos()
    {
        //si esta desactivado el mostrado de gizmo, slaimos del metodo sin hacer nada
        if (!showGizmos) return;

        Gizmos.color = Color.red;
        //pintamos un cubo para visualizar el area de groundcheck
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        Gizmos.color = Color.blue;
        //mostramos el gizmo de check de colision en la direccion de movimiento
        Gizmos.DrawWireSphere(checkPoint.position, checkSize);
    }
    #endregion
    #region METHODS

    /// <summary>
    /// Rcuperamos la informacion de los Inputs
    /// </summary>
    private void Controls()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    /// <summary>
    /// Realiza el desplazamiento del player
    /// </summary>
    private void Movement()
    {
        //componemos el vector de direccion deseado a partir del input
        direction.Set(horizontal,0f,vertical);
        //para asegurarnos que las diagonales no tienen una maagnitud superior a 1, "Clampeamos" su valor
        direction = Vector3.ClampMagnitude(direction, 1f);

        //calculamos la velocidad deseada en base a la driecion y la velocidad maxima
        desiredVelocity = direction * movementSpeed;

        Vector3 temp = transform.position + (direction * checkDistance);
        //respetamos la altura que ya tuviese el checkpoint
        temp.y = checkPoint.position.y;
        //movmeos el checkpoint a su posicion final
        checkPoint.position = temp;

        if (walled) desiredVelocity = Vector3.zero;

        if ((horizontal!=0 || vertical != 0) && grounded && !walled)
        {
            //aplicamos la velocidad deseada , aumentando frame a frame en base a la aceleracion
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, Time.deltaTime * acceleration);

            //debug ray para ver la direccion de velocidad del rigidbody
            if (showGizmos) Debug.DrawRay(transform.position, rb.velocity);

            //rotamos el tanque para que mire hacia la direccion que apunta la velocidad deseada
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                   Quaternion.LookRotation(desiredVelocity),
                                                   Time.deltaTime * rotationSpeed);

            isMoving = true;
        }
        //En caso de no existir input, paramos la rotacion del tanque
        if ((horizontal == 0 && vertical == 0) || walled)
        {
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
        }
    }

    /// <summary>
    /// Alimenta la informacion para el animator
    /// </summary>
    private void AnimatorFeed()
    {
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("Grounded", grounded);
        
        //// Activa la animación si el jugador se está moviendo
        //if ((horizontal != 0 || vertical != 0))
        //{
        //    anim.SetBool("IsMoving", isMoving);
        //}
        //else if((horizontal == 0 && vertical == 0))
        //{
        //    anim.SetBool("IsMoving", !isMoving);
        //}
    }

    /// <summary>
    /// Comprueba si hay contacto con el suelo
    /// </summary>
    private void GrounCheck()
    {
        colliderBuffer[0] = null;
        //comprobamos si hay contacto con el suelo mediante un overlap non alloc, para consumir menos recursos
        //ya que esta comprobacion se hara de forma continua.
        Physics.OverlapBoxNonAlloc(groundCheck.position,
                                    groundCheckSize / 2,
                                    colliderBuffer,
                                    transform.rotation,
                                    groundLayer);

        //si el cvalor delprimer elemento del buffer es distinto de null, consideramos que estamos tocando el suelo
        grounded = colliderBuffer[0] != null;
    }

    /// <summary>
    /// Detecta si hya un collider del layer indicado en contacto con el checker de colision de desplazamiento
    /// </summary>
    private void CollisionPreDetection()
    {
        //inicializamos el buffer
        colliderBuffer[0] = null;

        Physics.OverlapSphereNonAlloc(checkPoint.position,
                                       checkSize,
                                       colliderBuffer,
                                        checkLayer);

        walled = colliderBuffer[0] != null;
    }

    /// <summary>
    /// Aplica una fuerza vertical para saltar
    /// </summary>
    private void Jump()
    {
        if (grounded)
        {
            anim.SetTrigger("Jump");
            //si estamos encontacto con elsuelo, aplicamos una fuerza vertical de tipo impulso
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        
        
    }

    private void Shoot()
    {
        // si no hemoas superado el timpo estimado para poder volver a disparar, no haremos nada
        if (Time.time < shootTime) return;

        if (leftGun)
        {
            anim.SetTrigger("Shoot Left");
            //solicitamos a la pool activar un proyectil en el cañon izquierdo
            PoolManager.instance.Pull(bulletType, gunLeft.position,Quaternion.LookRotation(gunLeft.forward));
        }
        else
        {
            anim.SetTrigger("Shoot Right");
            PoolManager.instance.Pull(bulletType, gunRight.position, Quaternion.LookRotation(gunRight.forward));
        }

        shootTime = Time.time + shootDelay;
        leftGun = !leftGun;

        anim.SetFloat("ShootSpeed", 1 / shootDelay);
    }

    /// <summary>
    /// Apuntado de la torreta hacia la posicion del curso en pantalla
    /// </summary>
    private void AimingBehaviour()
    {
        //definimos el ray en base a la posicion del cursor en pantalla
        Ray camRay = cameraMain.ScreenPointToRay(Input.mousePosition);
        //variable temporal para alamcenar el resultado del raycast
        RaycastHit groundHit = new RaycastHit();

        //realizamos el raycast
        if(Physics.Raycast(camRay, out groundHit, camRayLenght, pointerLayer))
        {
            //en caso de que impacter contra una superfice dentro dle layer
            // desplazo el punto pivote para la rotacion de la torreta
            aimingPivot.position = new Vector3(groundHit.point.x,aimingPivot.position.y, groundHit.point.z);
        }
    }
    #endregion
}
