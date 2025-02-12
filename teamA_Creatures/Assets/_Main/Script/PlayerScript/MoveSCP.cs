using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSCP : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;

    public Vector3 PlayerPosX;
    public Vector3 EnemyPosX;
    public float dis;
    private float InDis;

    private bool moveF;
    private bool moveB;

    public float moveFSpeed;
    public float moveBSpeed;
    // Start is called before the first frame update
    void Start()
    {
        InDis = Vector3.Distance(PlayerPosX, EnemyPosX);
        moveF = false;
        moveB = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosX = Player.transform.position;
        EnemyPosX = Enemy.transform.position;
        dis = Vector3.Distance(PlayerPosX, EnemyPosX);

        if (dis > InDis)
        {
            moveB = true;
        }
        else if(dis < InDis)
        {
            moveF = true;
        }

        InDis = dis;

        Vector3 EnemyPosition = new Vector3(Enemy.transform.position.x,transform.position.y,Enemy.transform.position.z);
        transform.LookAt(EnemyPosition);

    }

    public void MovePlay(float velosictyX,float velosityY)
    {
        if (moveF)
        {

        }
    }
}
