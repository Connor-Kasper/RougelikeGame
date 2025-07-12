using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class findPlayer : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindWithTag("player");
        vcam.Follow = Player.transform;
    }
}
