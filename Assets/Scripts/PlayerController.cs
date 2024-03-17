using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
    public bool canShoot = true;
    public Transform gunLeft;
    public Transform gunRight;
    //id de la pool de la cual recuperar los proyectiles
    public string bulletType = "RegularBullets";
    //variable para la distancia minima del disparo
    public float minShootDistance = 1f;

    [Header("Laser")]
    // Agrega esta variable para asignar el rayo láser desde el editor de Unity
   // public LaserBeam laserPrefab;
    // Agrega esta variable para controlar el retraso entre disparos del láser
    public float laserDelay = 1.0f;
    public float laserTime = 0.0f;
    public string laserAttack = "LaserBeam";
    public Transform shootingLaserPoint;
    

    [Header("Aiming")]
    //longitud del raycas a realizar
    public float camRayLenght;
    //layer que podras ser "tocado" con el cursor del raton
    public LayerMask pointerLayer;
    //transform usado como objetivo para la rotacion
    public Transform aimingPivotLeft;
    public Transform aimingPivotRight;
    //transform usado visualmente como objetivo de rotacion
    //public Transform visualAimingPivot;
    //para almacenar la referencia de la camara principal
    public Camera cameraMain;
    // Almacena la última dirección de mira
    private Vector3 lastAimDirection;

    [Header("Roll")]
    public float rollDistance = 5f;
    public float rollDuration = 0.5f;
    private bool isRolling;
    private Vector3 rollDirection;
    public float rollCooldown = 3f;

    


    [Header("Animator")]
    public Animator anim;

    //solo vamos a comprobar si es mayor que 0, asin que no necesitamos mas capacidad
    Collider[] colliderBuffer = new Collider[1];

    public static PlayerController instance;

    #endregion
    #region EVENTS


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
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

        // Si el láser está activo, actualiza su posición y rotación para que coincida con el jugador
        if (Time.time < laserTime)
        {
            Vector3 laserDirection = transform.forward;
            PoolManager.instance.UpdatePulledObject(laserAttack, shootingLaserPoint.position, Quaternion.LookRotation(laserDirection));
        }
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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButton("Fire1") && canShoot)
        {
            Shoot();
        }
        if (Time.time >= laserTime && Input.GetButtonDown("Fire2"))
        {
            ShootLaser();
        }
        if (Input.GetButtonDown("Roll") && !isRolling)
        {
            OnRoll();
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

        if ((horizontal!=0 || vertical != 0) && grounded && !walled )
        {
            //aplicamos la velocidad deseada , aumentando frame a frame en base a la aceleracion
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, Time.deltaTime * acceleration);
            

            //debug ray para ver la direccion de velocidad del rigidbody
            if (showGizmos) Debug.DrawRay(transform.position, rb.velocity);

            //rotamos el tanque para que mire hacia la direccion que apunta la velocidad deseada
            //transform.rotation = Quaternion.Slerp(transform.rotation,
            //                                       Quaternion.LookRotation(desiredVelocity),
            //                                       Time.deltaTime * rotationSpeed);



            isMoving = true;
        }
        //En caso de no existir input, paramos la rotacion del tanque
        if ((horizontal == 0 && vertical == 0) || walled )
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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        
        
    }

    private void Shoot()
    {
                
        // si no hemoas superado el timpo estimado para poder volver a disparar, no haremos nada
        if (Time.time < shootTime) return;
        // Definimos el cañón desde el cual disparar y la posición de disparo
        Transform gun = leftGun ? gunLeft : gunRight;
        Vector3 shootPosition = gun.position;
        Vector3 shootDirectionRight = (aimingPivotRight.position - shootPosition).normalized;
        Vector3 shootDirectionLeft = (aimingPivotLeft.position - shootPosition).normalized;
        if (leftGun)
        {
            anim.SetTrigger("Shoot Left");
            //solicitamos a la pool activar un proyectil en el cañon izquierdo
            PoolManager.instance.Pull(bulletType, gun.position, Quaternion.LookRotation(shootDirectionLeft));
        }
        else
        {
            anim.SetTrigger("Shoot Right");
            PoolManager.instance.Pull(bulletType, gun.position, Quaternion.LookRotation(shootDirectionRight));
        }

        shootTime = Time.time + shootDelay;
        leftGun = !leftGun;

        anim.SetFloat("ShootSpeed", 1 / shootDelay);
    }

    /// <summary>
    /// Apuntado de las pistolas hacia la posicion del curso en pantalla
    /// </summary>
    private void AimingBehaviour()
    {

        // Definimos la dirección del objetivo
        Vector3 targetDirection = Vector3.zero;

        Vector3 mousePosition = Input.mousePosition;

        // Para el ratón, obtenemos la dirección del objetivo a partir de la posición del cursor en pantalla
        Ray camRay = cameraMain.ScreenPointToRay(mousePosition);
        RaycastHit groundHit;

        if (Physics.Raycast(camRay, out groundHit, camRayLenght, pointerLayer))
        {
            targetDirection = groundHit.point - transform.position;
            targetDirection.y = 0f;
        }

        // Para el uso de mando, obtenemos la dirección del objetivo a partir del joystick derecho
        float horizontalAim = Input.GetAxis("RightStickHorizontal");
        float verticalAim = Input.GetAxis("RightStickVertical");

        if (new Vector2(horizontalAim, verticalAim).sqrMagnitude > 0.01f)
        {
            // Si se está moviendo el joystick derecho, calculamos la dirección del objetivo
            targetDirection = new Vector3(verticalAim, 0f, horizontalAim).normalized;


        }


        // Rotamos el personaje hacia la dirección del objetivo
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
               

    }  


private void OnRoll()
    {
        rollDirection = transform.forward;
        
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        canShoot = false;
        Vector3 startPosition = transform.position;
         
        Vector3 endPosition = startPosition + rollDirection * rollDistance;
        // Reproducir la animación de dash
        anim.SetTrigger("Roll");
        float startTime = Time.time;
        while(Time.time < startTime + rollDuration)
        {
            float normalizedTime = (Time.time - startTime) / rollDuration;
            transform.position =  Vector3.Lerp(startPosition, endPosition, normalizedTime);
            yield return null;
        }

        transform.position = endPosition;
        canShoot = true;
        yield return new WaitForSeconds(rollCooldown);
        isRolling = false;
       
    }

    void ShootLaser()
    {
        Vector3 laserDirection = transform.forward;
        //// Crea una instancia del láser
        //LaserBeam laser = Instantiate(laserPrefab, transform.position, Quaternion.LookRotation(laserDirection));
        PoolManager.instance.Pull(laserAttack, shootingLaserPoint.position, Quaternion.LookRotation(laserDirection));
        

        // Configura el retraso antes de poder disparar el láser de nuevo
        laserTime = Time.time + laserDelay;
    }
    
    public void ApplySpeedBoost(float speedBoostAmount)
    {
        movementSpeed *= speedBoostAmount; 
    }

    #endregion
}
