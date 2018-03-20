using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammunition Settings", menuName = "__Asteroids/Shared Data/Ammunition Template")]
[SerializeField]
public class AmmunitionSettings : ScriptableObject
{
	public float BulletLife = 1f;
	public float Damage = 1f;
}
