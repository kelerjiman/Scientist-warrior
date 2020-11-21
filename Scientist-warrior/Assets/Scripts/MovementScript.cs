using UnityEngine;
using Standard_Assets.CrossPlatformInput.Scripts;
using Script.CharacterStatus;

public class MovementScript : MonoBehaviour
{

    private Vector2 m_Direction = Vector2.zero;
    public float speed = 5;
    public Rigidbody2D rigidbody2d;
    private Vector2 m_TempPos;
    [SerializeField] Transform MountPlaceHolder;
    [SerializeField] GameObject DropShadow;
    private Mount mount;
    private bool OnMount = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mount") && !OnMount)
        {
            mount = collision.GetComponent<Mount>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mount"))
        {
            if (!OnMount)
            {
                mount = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.gameContinue)
            return;
        m_Direction.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        m_Direction.y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (m_Direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (m_Direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (Input.GetButtonDown("Ride") && mount != null)
        {
            Ride();
        }
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.gameContinue)
            return;
        m_TempPos = transform.position;
        m_TempPos.x += m_Direction.x;
        m_TempPos.y += m_Direction.y;
        rigidbody2d.MovePosition(rigidbody2d.position + m_Direction * speed * Time.fixedDeltaTime);
    }
    private void Ride()
    {
        if (OnMount)
        {
            mount.RideOff();
            speed -= mount.Speed;
            if (speed < 1)
                speed = 1;
            StateManager.Instance.RemoveState(mount.states);
            mount = null;
            OnMount = false;
            DropShadow.SetActive(true);
            
            return;
        }
        mount.RideOn(MountPlaceHolder, (int)transform.localScale.x, transform);
        OnMount = true;
        DropShadow.SetActive(false);
        speed += mount.Speed;
        StateManager.Instance.AddState(mount.states);

    }

}