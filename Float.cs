using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour {
    public float amplitude;
    public float y0;
    public float speed;
	// Use this for initialization
	void Start () {

        y0 = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        float y1 = y0 + amplitude * Mathf.Sin(speed * Time.time);
        Vector2 vectorFloating = new Vector2(this.transform.position.x, y1);
        
        this.transform.position = vectorFloating;
    }
}
