using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupConfirm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalFunc.TweenShowPopup(transform, 0.25f);
    }
    
    public void OnBtnNoClicked()
    {
        Log.Info("OnBtnNoClicked()");
        this.PostEvent(EventID.OnBtnNoClicked);
    }

    public void OnBtnYesClicked()
    {
        Log.Info("OnBtnYesClicked()");
        this.PostEvent(EventID.OnBtnYesClicked);
    }
}
