using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Transform pfDeath;

    public Transform pfArrow;


    public Transform pfTrap;

    public Transform pfJavelin;

    public Transform pfFireball;

    public Transform pfCannonball;

    public Transform pfMortarCannonball;

    public Transform pfArcherTower;

    public Transform pfCannonTower;

    public Transform pfBallistaTower;

    public Transform pfTrapper;

    public Transform pfMortarTower;

    public Transform pfFarmTower;

    public Transform pfWarmageTower;

    public Transform pfSpearmanTower;
    
    public Transform pfGladiator;

    public Transform pfBandit;

    public Transform pfHotAirBalloon;

    public Transform pfGiant;

    public Transform pfSkeleton;

    public Transform pfArchmage;

    public Transform pfDragon;

    public Transform pfDemon;

    

}
