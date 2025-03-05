using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Shield Instance;

    public GameObject ShieldObject;
    private GameObject ShieldInstance;
    public Vector3 shieldRotation = new Vector3(0, 90, 0);
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance == this)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollishonEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("1");
        }
    }


    public void OnShield()
    {
        if(ShieldInstance == null)
        {
            Vector3 ShieldPosition = transform.position + transform.forward * 2f+transform.up*2f;
            ShieldInstance = Instantiate(ShieldObject, ShieldPosition, Quaternion.identity);
            ShieldInstance.transform.rotation = transform.rotation * Quaternion.Euler(shieldRotation);
            MoveComponent.Instance.ATFieldNow = true;
            PlayerMaterialChange.Instance.ChangeMaterial(2);
        }


    }
    public void OffShield()
    {
        if (ShieldInstance != null)
        {
            Destroy(ShieldInstance);
            MoveComponent.Instance.ATFieldNow = false;
            PlayerMaterialChange.Instance.ReturnMaterial();
        }
    }
    private void Update()
    {

        if(ShieldInstance != null)
        {
            Vector3 ShieldPosition = transform.position + transform.forward * 2f+transform.up*2f;
            ShieldInstance.transform.position = ShieldPosition;

            // シールドの向きをプレイヤーの回転 + オフセットに更新
            ShieldInstance.transform.rotation = transform.rotation * Quaternion.Euler(shieldRotation);

        }
    }


}
