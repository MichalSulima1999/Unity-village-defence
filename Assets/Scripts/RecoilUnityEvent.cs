using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecoilUnityEvent : MonoBehaviour
{
    public UnityEvent recoil;

    public void StartRecoil() {
        recoil.Invoke();
    }
}
