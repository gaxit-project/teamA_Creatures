using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer2 : MonoBehaviour
{
    [SerializeField] MoveComponent move;
    [SerializeField] JumpComponent jump;
    [SerializeField] AttackComponent attack;
    public bool AttackButton;
    public Vector2 velocity;
    // インプットの登録と破棄
    PlayerAct input;

    void Awake()
    {
        input = new PlayerAct();
    }
    void OnDisable()
    {
        input.Disable();
    }

    // インプットの有効・無効化
    void OnDestroy()
    {
        input.Disable();
    }

    void OnEnable()
    { 
        input.Enable();
    }

    void Update()
    {
        if (!PlayerHP.Instance.HitNow)
        {
            if (!AttackComponent.Instance.attackNow)
            {
                // スティックの移動を取得して動かす
                velocity = input.PlatformAction.Move.ReadValue<Vector2>();

                move.MoveHorizontal(-velocity.x, velocity.y);
                jump.JumpVertical(velocity.y);
            }


            AttackButton = (input.PlatformAction.Attack.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint);
            if (AttackButton) attack.Attack();
        }

    }
}