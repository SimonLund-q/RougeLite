using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 0.5f;
    private Vector2 moveDir;
    private bool isMovingX = false;
    private bool isMovingY = false;
    private float playerX = 0;
    private float playerY = 0;

    private GameObject spawnPoint;
    private GameObject projectilePrefab;
    private GameObject handGunPrefab;

    private Camera cam;

    public GameObject firePoint;
    public GunSystem gunSystem;

    public void Start()
    {
        //Refererar utan att göra variablerna public
        
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        gunSystem.GetGun(1);
    }
    public void Update()
    {
        // associerar en vector med WASD tangenterna

        moveDir.x = Input.GetAxis("Horizontal"); 
        moveDir.y = Input.GetAxis("Vertical");
    }
    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            gunSystem.Fire(firePoint);
        }

        rb.MovePosition(rb.position + moveDir * speed);
        counterSteer(moveDir);
    }

    void counterSteer(Vector2 vec2)
    {
        // Gör en extra kraft så man inte glider runt

        if (vec2.x < 0 || vec2.x > 0){
            isMovingX = true;
            playerX = vec2.x;
        }
        if (vec2.y < 0 || vec2.y > 0){
            isMovingY = true;
            playerY = vec2.y;
        }

        if (isMovingY == true)
        {
            if (playerY < 0)
            {
                rb.AddForce(transform.up * speed * 100);
                isMovingY = false;
            }
            if (playerY > 0)
            {
                rb.AddForce(transform.up * speed * -100);
                isMovingY = false;
            }
        }

        if (isMovingX == true)
        {
            if (playerX < 0){
                rb.AddForce(transform.right * speed * 100);
                isMovingX = false;
            }
            if (playerX > 0){
                rb.AddForce(transform.right * speed * -100);
                isMovingX = false;
            }
        }
    }
    public Vector3 getMousePos()
    {
        Vector3 playerPos = gameObject.transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 relativeMousePos = mousePos - playerPos;
        return relativeMousePos;
    }
}
