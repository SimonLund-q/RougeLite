using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour
{
    // No shit vad de h�r g�r

    public GameObject wall;
    public GameObject ground;

    public GameObject hallGround;
    public GameObject hallWall;
    public GameObject door;

    private int minRoomSize = 10;
    private int maxRoomSize = 30;
    private int minRooms = 5;
    private int maxRooms = 10;

    private Vector2Int mapSize = new Vector2Int(100, 100);
    private List<Vector2Int> doorways = new List<Vector2Int>();
    private List<Rect> rooms = new List<Rect>();


    private Vector2 startPos;
    private GameObject player;
    public GameObject enemySpawn;


    private GameObject spawnArea;

    public bool MapsFucked = false;
    private bool firstRoom = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GenerateMap();
    }

    void GenerateMap()
    {
        int numRooms = Random.Range(minRooms, maxRooms);

        // B�stemmer storleken av rumm och var d�rrar ska vara
        for (int i = 0; i < numRooms; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            Vector2Int doorway = GenerateRoom(roomWidth, roomHeight);
            doorways.Add(doorway);
        }

        // Generate hallways between doorways
        GenerateHallways();
    }

    Vector2Int GenerateRoom(int width, int height)
    {
        bool roomPlaced = false;
        int attempts = 0;

        while (!roomPlaced && attempts < 100)
        {
            int x, y;

            // Genererar de f�rsta rummet
            if (rooms.Count == 0)
            {
                x = Random.Range(1, mapSize.x - width - 1);
                y = Random.Range(1, mapSize.y - height - 1);

                startPos = new Vector2(x + width / 2, y + height / 2);
                player.transform.position = startPos;

                firstRoom = false;
            }
            // Genererar rumm baserat p� vart de senaste rummet var pacerat
            else
            {
                
                Rect lastRoom = rooms[rooms.Count - 1];
                x = Random.Range(1, mapSize.x - width - 1);
                y = Random.Range(1, mapSize.y - height - 1);

                



                if (Random.value < 0.5f)
                    x = (int)(lastRoom.center.x - width / 2);
                else
                    y = (int)(lastRoom.center.y - height / 2);
            }

            Rect newRoom = new Rect(x, y, width, height);

            // Kollar om 2 rumm kommer att generera p� varandra;
            bool overlaps = false;
            foreach (Rect room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
                roomPlaced = true;

                for (int i = x; i < x + width; i++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        if (i == x || i == x + width - 1 || j == y || j == y + height - 1)
                        {
                            Instantiate(wall, new Vector3(i, j, 0), Quaternion.identity);
                        }
                        else
                        {
                            GameObject g = Instantiate(ground, new Vector3(i, j, 0), Quaternion.identity);
                        }
                    }
                }

                if (firstRoom == false){
                    Vector3 spawnPos = new Vector3(x + width / 2, y + height / 2, 0);
                    Debug.Log(spawnPos);
                    firstRoom = true;
                }
                else if (firstRoom == true)
                {

                    Vector3 spawnPos = new Vector3(x + width / 2, y + height / 2, 0);
                    Vector3 spawnScale = new Vector3(width - 2, height - 2);

                    spawnArea = Instantiate(enemySpawn, spawnPos, Quaternion.identity);
                    spawnArea.transform.localScale = spawnScale;
                    
                }
                


                // Returnerar vart d�rrarna ska vara
                return new Vector2Int(x + width / 2, y + height / 2);
            }

            attempts++;
        }


        // Om koden kommer hit �r mapen fucked
        Debug.Log("Failed to place room!");
        return Vector2Int.zero;
    }

    void GenerateHallways()
    {
        // Hittar start och slut position av d�rrarna
        for (int i = 0; i < doorways.Count; i++)
        {
            if (i < doorways.Count - 1)
            {
                Vector2Int start = doorways[i];
                Vector2Int end = doorways[i + 1];
                GenerateHallway(start, end);
            }
        }
    }

    // Genererar hallar baserat p� en start och slut vector;
    // Jag anv�nder samma k�d f�r verticala hallways som horizontella men jag har bara copy pastat koden ist�llet f�r att �terandv�nda den
    // Det �r h�r det b�rjar bli sv�rt att se vad jag har t�ngt egentligen
    void GenerateHallway(Vector2Int start, Vector2Int end)
    {
        Vector2Int pos = new Vector2Int(end.x - start.x, end.y - start.y);

        if (pos.x == 0)
        {
            for (int y = 0; y <= Mathf.Abs(pos.y); y++)
            {
                Vector2 rayOrigin = new Vector2(start.x, Mathf.Min(start.y, end.y) + y);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, Mathf.Abs(pos.x) + 1);

                if (hit.collider != null && hit.collider.CompareTag("Ground") || hit.collider != null && hit.collider.CompareTag("EnemySpawn"))
                {
                    // Vet inte varf�r den h�r �r tom men allt fallerar om jag r�r den s� jag l�mnar den s� h�r;
                }
                else if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    Destroy(hit.transform.gameObject);

                    Instantiate(door, hit.transform.position, Quaternion.identity);

                    Vector2 rayOriginn = new Vector2(start.x + 1, Mathf.Min(start.y, end.y) + y);
                    RaycastHit2D hitt = Physics2D.Raycast(rayOriginn, Vector2.right, Mathf.Abs(pos.x) + 1);
                    if (hitt.collider != null && hitt.collider.CompareTag("Wall"))
                    {
                        Destroy(hitt.transform.gameObject);

                        Instantiate(door, hitt.transform.position, Quaternion.identity);

                    }
                }
                else
                {
                    // Det h�r �r bara en objectiv d�lig l�sning p� problemet
                    Vector2 hallOffset = new Vector2(1, 0);
                    Vector2 wallOffset1 = new Vector2(-1, 0);
                    Vector2 wallOffset2 = new Vector2(2, 0);
                    Instantiate(hallGround, new Vector2(start.x, Mathf.Min(start.y, end.y) + y), Quaternion.identity);
                    Instantiate(hallGround, new Vector2(start.x, Mathf.Min(start.y, end.y) + y) + hallOffset, Quaternion.identity);
                    Instantiate(hallWall, new Vector2(start.x, Mathf.Min(start.y, end.y) + y) + wallOffset1, Quaternion.identity);
                    Instantiate(hallWall, new Vector2(start.x, Mathf.Min(start.y, end.y) + y) + wallOffset2, Quaternion.identity);
                }
            }
        }
        else if (pos.y == 0)
        {
            for (int x = 0; x <= Mathf.Abs(pos.x); x++)
            {
                Vector2 rayOrigin = new Vector2(Mathf.Min(start.x, end.x) + x, start.y);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, Mathf.Abs(pos.y) + 1);

                if (hit.collider != null && hit.collider.CompareTag("Ground") || hit.collider != null && hit.collider.CompareTag("EnemySpawn"))
                {
                    // Vet inte varf�r den h�r �r tom men allt fallerar om jag r�r den s� jag l�mnar den s� h�r;
                }
                else if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    Destroy(hit.transform.gameObject);

                    Instantiate(door, hit.transform.position, Quaternion.identity);

                    Vector2 rayOriginn = new Vector2(Mathf.Min(start.x, end.x) + x, start.y + 1);
                    RaycastHit2D hitt = Physics2D.Raycast(rayOriginn, Vector2.up, Mathf.Abs(pos.y) + 1);

                    if (hitt.collider != null && hitt.collider.CompareTag("Wall"))
                    {
                        Destroy(hitt.transform.gameObject);
                        Instantiate(door, hitt.transform.position, Quaternion.identity);
                    }

                }
                else
                {
                    // Det h�r �r bara en objectiv d�lig l�sning p� problemet
                    Vector2 hallOffset = new Vector2(0, 1);
                    Vector2 wallOffset1 = new Vector2(0, -1);
                    Vector2 wallOffset2 = new Vector2(0, 2);
                    Instantiate(hallGround, new Vector2(Mathf.Min(start.x, end.x) + x, start.y), Quaternion.identity);
                    Instantiate(hallGround, new Vector2(Mathf.Min(start.x, end.x) + x, start.y) + hallOffset, Quaternion.identity);
                    Instantiate(hallWall, new Vector2(Mathf.Min(start.x, end.x) + x, start.y) + wallOffset1, Quaternion.identity);
                    Instantiate(hallWall, new Vector2(Mathf.Min(start.x, end.x) + x, start.y) + wallOffset2, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("Maps Fucked");
            MapsFucked = true;
        }
    }


    /*
     
        N�r jag skrev den h�r k�den hade jag n�gon form utav koffein blackout och sedan vaknade jag till deth�r, jag vet vad den g�r men jag har ingen aning om hur den g�r det.
    
        Totala m�ngden timmar sl�sad h�r : 17
        Totala problem l�sta : 1 (Ish)
        Totala problem skapade av min l�sning : 4
    
    */
}