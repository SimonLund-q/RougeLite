using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    private Vector2[] enemySpawns;
    private bool hasSpawned = false;
    float spawnTime = 2f;

    public GameObject particleEffect;
    public GameObject enemy;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SpawnEN();
            hasSpawned = true;
        }
    }

    void SpawnEN()
    {
        if (hasSpawned == false)
        {
            Debug.Log("has been hit");


            float roomSize = this.transform.localScale.x * this.transform.localScale.y;
            int numOfEnemies = (int)roomSize / 30;

            Debug.Log(numOfEnemies);
            for (int i = 0; i < numOfEnemies; i++)
            {

                Vector2 spawnLocation = new Vector2(Random.Range(-this.transform.localScale.x / 2, this.transform.localScale.x / 2) + this.transform.localPosition.x, Random.Range(-this.transform.localScale.y / 2, this.transform.localScale.y / 2) + this.transform.localPosition.y);
                /*
                    //Load a text file (Assets/Resources/Text/textFile01.txt)
                    var textFile = Resources.Load<TextAsset>("Text/textFile01");
                */

                //var particleEffect = Resources.Load<TextAsset>("PreFabs/EnemySpawnWarning.prefab");


                //particleEffect = GameObject.Find("EnemySpawnWarning");
                Instantiate(particleEffect, spawnLocation, Quaternion.identity);
                Instantiate(enemy, spawnLocation, Quaternion.identity);
            }
        }
    }
}
