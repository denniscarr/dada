using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerEthanSpawner : MonoBehaviour {
    public GameObject ethanPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(ethanPrefab, transform.position, Quaternion.identity);
        }
    }
}
