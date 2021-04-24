using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float Speed = 10;

    Rigidbody2D m_Rigidbody;
    
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        var position = m_Rigidbody.position;
        position.x += horizontalInput * Speed * Time.fixedDeltaTime;
        
        m_Rigidbody.MovePosition(position);
    }
}
