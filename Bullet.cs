using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;

    public GameObject player;
    public Camera cam;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 playerPos = player.transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 relativeMousePos = mousePos - playerPos;
        Vector3 shootDir = relativeMousePos / 100;

        rb.AddForce(shootDir * 20000);
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" || other.tag == "HallWall")
        {
            Destroy(gameObject);
        }
    }
}
