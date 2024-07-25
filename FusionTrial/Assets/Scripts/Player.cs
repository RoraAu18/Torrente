using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : NetworkBehaviour
{
    NetworkCharacterController netCc;
    Rigidbody2D _rb;
    [SerializeField] int coins;
    AudioSource _audioSource;
    [SerializeField] AudioClip coinSound;
    float currYVelocity;
    [SerializeField] float jumpHeight;
    bool oldIsGrounded;
    bool isGrounded;
    public CameraCC cam;
    Manager manager => Manager.Instance;
    public void Awake()
    {
        TryGetComponent(out netCc);
        TryGetComponent(out _audioSource);
    }
    /*
    public override void Spawned()
    {

        oldIsGrounded = true;
        cam = FindObjectOfType<CameraCC>();
        cam.SetTarget(this);
    }*/
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            netCc.Move(manager.movementSpeed * data.direction * Runner.DeltaTime);

            oldIsGrounded = isGrounded;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Coin coin))
        {
            coins++;
            GameUIController.Instance.CaptureCoins(coins);
            _audioSource.PlayOneShot(coinSound);
            coin.Recycle();
        }

    }
}
