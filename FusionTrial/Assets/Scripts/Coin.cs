using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Recycle()
    {
        gameObject.SetActive(false);
    }

}
