using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;

    bool isGrounded ;

    Vector3 smoothVelocity;
    Vector3 moveAmount;
    Rigidbody rb;
    PhotonView pV;

    const float maxHealth = 100f;
    float currHealth = maxHealth;

    PlayerManager playerManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)pV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (pV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    private void Update()
    {
        if (!pV.IsMine)
        {
            return;
        }

        LookAround();
        Jump();
        MoveAround();

        for(int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }


        SwitchGunScroll();

        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        CheckLowerBoundsForDeath();
    }

    public void LookAround()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }


    public void MoveAround()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothVelocity, smoothTime);
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump is called");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundState(bool _grounded)
    {
        isGrounded = _grounded; 

    }

    private void FixedUpdate()
    {
        if (!pV.IsMine)
        {
            return;
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void EquipItem(int _index)
    {
        if(_index == previousItemIndex)
        {
            return;
        }

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (pV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", items);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("itemIndex") && !pV.IsMine && targetPlayer == pV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void SwitchGunScroll()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }

        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

    }

    public void TakeDamage(float damage)
    {
        pV.RPC(nameof(RPC_TakeDamage), pV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo messageInfo)
    {
        currHealth -= damage;
        healthbarImage.fillAmount = currHealth / maxHealth;

        if(currHealth <= 0)
        {
            Die();
            PlayerManager.Find(messageInfo.Sender).GetKill();
        }
    }

    public void Die()
    {
        playerManager.Die();
    }

    public void CheckLowerBoundsForDeath()
    {
        if(transform.position.y < -10f)
        {
            Die();
        }
    }
}
