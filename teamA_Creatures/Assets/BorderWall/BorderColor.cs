using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderColor : MonoBehaviour
{
    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }*/
    private Color borderWall;
    public GameObject player;
    public GameObject enemy;
    private float wallDistanceP;
    private float wallDistanceE;
    private float wallDistance;
    public float maxColor = 120f;
    public float darkMag = 1f;

    private void Start()
    {
        borderWall = this.GetComponent<Renderer>().material.color;
        borderWall.a = 0f;
    }
    private void Update()
    {
        wallDistanceP = this.gameObject.transform.position.x - player.transform.position.x;
        wallDistanceE = this.gameObject.transform.position.x - enemy.transform.position.x;
        if (wallDistanceP < 0f)
        {
            wallDistanceP *= -1;
        }
        if (wallDistanceE < 0f)
        {
            wallDistanceE *= -1;
        }
        if (wallDistanceP < wallDistanceE)
        {
            wallDistance = wallDistanceP;
        }
        else
        {
            wallDistance = wallDistanceE;
        }
        wallDistance *= darkMag;
        if(wallDistance > maxColor)
        {
            wallDistance = maxColor;
        }
        borderWall.a = maxColor - wallDistance;
        this.gameObject.GetComponent<Renderer>().material.color = borderWall;
    }
}
