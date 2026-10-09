using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharFlip : CharComponents
{
    [Tooltip("Mode used without a weapon. A weapon switches it (see WeaponDataItem.FlipMode).")]
    [FormerlySerializedAs("flipMode")]
    [SerializeField] private MyLibrary.FlipMode defaultFlipMode = MyLibrary.FlipMode.MovementDirection;
    [Tooltip("Min horizontal speed to turn around.")]
    [SerializeField] private float threshold = 0.1f;

    public bool FacingRight { get; private set; }
    public MyLibrary.FlipMode FlipMode { get; private set; }
    public MyLibrary.FlipMode DefaultFlipMode => defaultFlipMode;

    [SerializeField] private Transform fieldOfView;

    [SerializeField] public StateController stc;

    private void Awake()
    {
        FacingRight = true;
        FlipMode = defaultFlipMode;

        stc = GetComponent<StateController>();
    }

    protected override void HandleAbility()
    {
        base.HandleAbility();

        if (FlipMode == MyLibrary.FlipMode.WeaponDirection && TryFlipToWeaponDirection())
            return;

        // режим по движению, или оружие сейчас никуда не целится
        FlipToMoveDirection();
    }

    // CharController.CurrentMovement заполняется и для игрока, и для NPC
    private void FlipToMoveDirection()
    {
        if (controller == null) return;

        float moveX = controller.CurrentMovement.x;
        if (Mathf.Abs(moveX) <= threshold) return;   // стоим — не разворачиваемся

        FaceDirection(moveX > 0f ? 1 : -1);
    }

    // Смотрим туда, куда целится оружие. false — прицела нет (нет оружия / WeaponAim / NPC без цели).
    private bool TryFlipToWeaponDirection()
    {
        if (charWeapon == null || charWeapon.CurrentWeapon == null) return false;

        WeaponAim aim = charWeapon.CurrentWeapon.WeaponAim;
        if (aim == null) return false;

        // у NPC WeaponAim пока не считает угол — пусть разворачиваются по движению
        if (!isPlayer && !aim.autoAim) return false;

        float weaponAngle = aim.CurrentAimAngleAbsolute;
        FaceDirection(weaponAngle > 90f || weaponAngle < -90f ? -1 : 1);
        return true;
    }

    public void FaceDirection(int newDirection)
    {
        bool faceRight = newDirection >= 0;
        if (faceRight == FacingRight) return;   // уже смотрим туда

        FacingRight = faceRight;

        if (character != null && character.CharacterSprite != null)
            character.CharacterSprite.transform.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);

        SetViewFieldDirection();
    }

    private void SetViewFieldDirection()
    {
        if (fieldOfView == null) return;

        float angle = FacingRight ? -90f : 90f;
        fieldOfView.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetFlipMode(MyLibrary.FlipMode mode)
    {
        FlipMode = mode;
    }

    // без оружия возвращаемся к режиму по умолчанию
    public void ResetFlipMode()
    {
        FlipMode = defaultFlipMode;
    }
}
