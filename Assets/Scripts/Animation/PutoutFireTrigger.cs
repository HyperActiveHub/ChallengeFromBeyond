using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutoutFireTrigger : MonoBehaviour
{
    Animator fireAnim = null;

    public void SetFireAnimator(Animator anim)
    {
        fireAnim = anim;
    }

    public void TriggerDie()
    {
        if (fireAnim == null)
        {
            Debug.LogError("FireAnimator not set. SetFireAnimator needs to be called before TriggerDie does.", this);


        }
        else
            fireAnim.SetTrigger("Die");
    }
}