using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("PlayerAvatar", spawnPoint.position, spawnPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
