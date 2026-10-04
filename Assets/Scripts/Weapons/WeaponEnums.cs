// Какой класс оружия должен стоять на префабе (MeleeWeapon / ShootingWeapon)
public enum WeaponKind
{
    Melee,
    Shooting,
}

public enum ReloadType
{
    Manual,       // перезарядка по кнопке R, тратит запас патронов
    AutoOnEmpty,  // перезаряжается сам, когда магазин пуст
}
