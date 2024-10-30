using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class RandomLevelAdjustment : MonoBehaviour
{
    [SerializeField] BaseGun PlayerGun;
    BaseGun PCOriginalCopy;
    PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerGun)
            PlayerGun = FindObjectOfType<BaseGun>();
        if (!PC)
            PC = FindObjectOfType<PlayerController>();

        PCOriginalCopy = new BaseGun();

        PCOriginalCopy.AltareChargeTime = PlayerGun.AltareChargeTime;
        PCOriginalCopy.AltareColor = PlayerGun.AltareColor;
        PCOriginalCopy.AltareFireOnFullCharge = PlayerGun.AltareFireOnFullCharge;
        PCOriginalCopy.ShinriChargeTime = PlayerGun.ShinriChargeTime;
        PCOriginalCopy.ShinriColor = PlayerGun.ShinriColor;
        PCOriginalCopy.ShinriFireOnFullCharge = PlayerGun.ShinriFireOnFullCharge;
        PCOriginalCopy.HakkaChargeTime = PlayerGun.HakkaChargeTime;
        PCOriginalCopy.HakkaColor = PlayerGun.HakkaColor;
        PCOriginalCopy.HakkaFireOnFullCharge = PlayerGun.HakkaFireOnFullCharge;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            RandomizeCharacterStats();
            PlayerGun.SetCharacter(PC.currentCharacter);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ResetCharacterStats();
            PlayerGun.SetCharacter(PC.currentCharacter);
        }
    }

    public void RandomizeCharacterStats()
    {
        PlayerGun.AltareChargeTime = Random.Range(0f,1f);
        PlayerGun.AltareFireOnFullCharge = RandomBool();   
        PlayerGun.ShinriChargeTime = Random.Range(0f, 1f);
        PlayerGun.ShinriFireOnFullCharge = RandomBool();
        PlayerGun.HakkaChargeTime = Random.Range(0f, 1f);
        PlayerGun.HakkaFireOnFullCharge = RandomBool();
    }

    public void ResetCharacterStats()
    {
        PlayerGun.AltareChargeTime = PCOriginalCopy.AltareChargeTime;
        PlayerGun.AltareColor = PCOriginalCopy.AltareColor;
        PlayerGun.AltareFireOnFullCharge = PCOriginalCopy.AltareFireOnFullCharge;
        PlayerGun.ShinriChargeTime = PCOriginalCopy.ShinriChargeTime;
        PlayerGun.ShinriColor = PCOriginalCopy.ShinriColor;
        PlayerGun.ShinriFireOnFullCharge = PCOriginalCopy.ShinriFireOnFullCharge;
        PlayerGun.HakkaChargeTime = PCOriginalCopy.HakkaChargeTime;
        PlayerGun.HakkaColor = PCOriginalCopy.HakkaColor;
        PlayerGun.HakkaFireOnFullCharge = PCOriginalCopy.HakkaFireOnFullCharge;
    }

    bool RandomBool()
    {
        int result = Random.Range(0, 2);
        if (result == 1)
            return true;
        return false;
    }
    
}
