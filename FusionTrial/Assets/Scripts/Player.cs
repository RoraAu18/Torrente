using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class Player : NetworkBehaviour
{
    CharacterController charController;
    Rigidbody2D _rb;
    [SerializeField] int coins;
    AudioSource _audioSource;
    [SerializeField] AudioClip coinSound;
    float currYVelocity { get; set; }
    public float speed = 6.0f;
    public float fallForce = 6.0f;
    public float jumpHeight = 1.0f;
    public float gravity = 9.81f;
    
    bool isGrounded { get; set; }
    public CameraCC cam;
    [SerializeField] RaycastHit[] results = new RaycastHit[30];
    public Transform groundCheck;
    public LayerMask groundMask;
    Manager manager => Manager.Instance;
    public void Awake()
    {
        TryGetComponent(out charController);
        TryGetComponent(out _audioSource);
    }
    
    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            cam = FindObjectOfType<CameraCC>();
            cam.SetTarget(this);
        }   
    }
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out InputData data))
        {
            UpdateMovement(data);
        }
    }
    void UpdateMovement(InputData input)
    {
        Vector3 currMovement = new Vector3();

        if (input.GetButton(InputButton.LEFT))
        {
            currMovement = Vector3.left * manager.movementSpeed * Runner.DeltaTime;
        }
        else if (input.GetButton(InputButton.RIGHT))
        {
            currMovement = Vector3.right * manager.movementSpeed * Runner.DeltaTime;
        }
        else currMovement = Vector3.zero;

        var direction = transform.up;
        var offset = charController.height / 2 - charController.radius;
        var localPoint0 = charController.transform.position - direction * offset;
        var localPoint1 = charController.transform.position + direction * offset;
        var radius = charController.radius;
        var groundCheckDist = charController.skinWidth + 0.01f;

        var collisionAmount = Physics.CapsuleCastNonAlloc(localPoint0, localPoint1, radius, Vector3.down, results, groundCheckDist, groundMask);

        isGrounded = false;

        Debug.Log("collis " + collisionAmount + " ");
        for (int i = 0; i < collisionAmount; i++)
        {
            var currCollision = results[i];
            var currNormal = currCollision.normal;
            var angle = Vector3.Angle(Vector3.up, currNormal);
            isGrounded = angle < charController.slopeLimit;
            if (isGrounded) break;
        }
        if (isGrounded)
        {
            currYVelocity = -2;
        }
        else
        {
            if (currYVelocity > 0)
            {
                currYVelocity -= fallForce * Runner.DeltaTime;
            }
            currYVelocity -= gravity * Runner.DeltaTime;
        }

        
        if (input.GetButton(InputButton.JUMP) && isGrounded)
        {
            isGrounded = false;
            currYVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        currMovement.y = currYVelocity * Runner.DeltaTime;
        charController.Move(currMovement);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliging");
        if (other.gameObject.TryGetComponent(out Coin coin))
        {
            Debug.Log("coin");
            coins++;
            GameUIController.Instance.CaptureCoins(coins);
            _audioSource.PlayOneShot(coinSound);
            coin.Recycle();
        }
    }
}
