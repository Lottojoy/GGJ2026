using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaskInteraction : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {/*
        if (!other.gameObject.CompareTag("Mask")) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Mask targetMask = other.gameObject.GetComponent<Mask>();

            if (targetMask is AttackMask attackMask)
            {
                PlayerStats.Instance.attackMask = attackMask;
            }

            if (targetMask is DefenseMask defenseMask)
            {
                PlayerStats.Instance.defenseMask = defenseMask;
            }

            if (targetMask is DashMask dashMask)
            {
                PlayerStats.Instance.dashMask = dashMask;
            }
        }*/

    }
}
