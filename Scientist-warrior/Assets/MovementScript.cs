using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MovementScript : MonoBehaviour
{
    private Vector2 m_Direction = Vector2.zero;
    public int speed = 5;
    public Rigidbody2D rigidbody2d;
    private Vector2 m_TempPos;

    // Update is called once per frame
    void Update()
    {
        m_Direction.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        m_Direction.y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (m_Direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (m_Direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        m_TempPos = transform.position;
        m_TempPos.x += m_Direction.x;
        m_TempPos.y += m_Direction.y;


        rigidbody2d.MovePosition(rigidbody2d.position + m_Direction * speed * Time.fixedDeltaTime);
    }
}