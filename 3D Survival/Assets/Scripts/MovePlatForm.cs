using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody rigid;

    Rigidbody _player;




    [SerializeField] float moveSpeed;
    Vector3 moveVec;
    Vector3 dirVec;

    public enum MoveDirection
    {
        Vertical,
        Horizontal,
        UpperLeft_LowerRight,
        UpperRight_LowerLeft
    }
    [SerializeField] MoveDirection myDir;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 방향 벡터 초기화
        switch (myDir)
        {
            case MoveDirection.Vertical:
                dirVec = Vector3.forward;
                break;
            case MoveDirection.Horizontal:
                dirVec = Vector3.right;
                break;
            case MoveDirection.UpperLeft_LowerRight:
                dirVec = new Vector3(1, 0, -1).normalized;
                break;
            case MoveDirection.UpperRight_LowerLeft:
                dirVec = new Vector3(1, 0, 1).normalized;
                break;
        }
    }

    void FixedUpdate()
    {
        moveVec = dirVec * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + moveVec);

        if (_player != null)
        {
            _player.MovePosition(_player.position + moveVec);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerIn");

            
             
            _player = collision.gameObject.GetComponent<Rigidbody>();
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerOut");
            _player = null;
        }
    }



}