using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class powerUpStats : ScriptableObject
{
    public float shootRate;
    public int shootDist;
    public int shootDamage;
    public GameObject bulletHitEffect;
    public GameObject spellModel;
}
