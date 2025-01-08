using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    private int playerMaxHP;
    private int playerHP;

    public Slider slider;
    void Start()
    {
        playerMaxHP = 100;
        playerHP = playerMaxHP;
    }

    private void Update()
    {
        slider.value = (float)playerHP / (float)playerMaxHP;
    }
    public void Button()
    {
        playerHP = playerHP - 7;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            playerHP = playerHP-10;
            
        }
    }
}
