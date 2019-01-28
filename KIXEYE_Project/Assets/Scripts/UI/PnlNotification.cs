using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LC.Ultility;

public class PnlNotification : MonoBehaviour
{
    public TextMeshProUGUI txtNoti;
    public int autoDestroyTimer = 3;
    public GameObject pnlMain;

    // Start is called before the first frame update
    void Start()
    {
        GlobalFunc.TweenShowPopup(pnlMain.transform, 0.25f);

        //StartCoroutine(IEAutoDestroy());
        Destroy(gameObject, autoDestroyTimer);
    }
    
    public void SetNotiText(string noti)
    {
        txtNoti.SetText(noti);
    }

    private IEnumerator IEAutoDestroy()
    {
        int timer = autoDestroyTimer;
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        while (timer > 0)
        {
            timer--;
            yield return wait;
        }

        Destroy(gameObject);
    }
}
