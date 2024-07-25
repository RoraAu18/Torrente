using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCC : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float offset;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetTarget(Player _player)
    {
        player = _player;
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        Vector3 vectorPos = new Vector3();
        vectorPos.x = player.transform.position.x + offset;
        vectorPos.y = player.transform.position.y + offset;
        transform.position = vectorPos;
    }
}
