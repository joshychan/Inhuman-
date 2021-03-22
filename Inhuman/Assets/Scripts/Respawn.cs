using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform respawnPoint;
    
    

    public void OnTriggerEnter(Collider other)
    {
        player.transform.position = respawnPoint.transform.position;
        FindObjectOfType<gameManger>().EndGame();
        

    }
    
 
}
