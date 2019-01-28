using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingletonExt<UIManager>
{
    [HideInInspector]
    public Canvas curCanvas;

    public GameObject popupConfirm;
    public GameObject pnlNotification;

    public override void Init()
    {
        base.Init();
        curCanvas = FindObjectOfType<Canvas>();
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += Handle_sceneLoaded;
    }

    private void Handle_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        curCanvas = FindObjectOfType<Canvas>();
    }

    public void ShowPopupConfirm()
    {
        var pnl = Instantiate(popupConfirm, curCanvas.transform);
    }

    public void ShowNotification(string noti)
    {
        var pnl = Instantiate(pnlNotification, curCanvas.transform);
        var pnlNoti = pnl.GetComponent<PnlNotification>();
        pnlNoti.SetNotiText(noti);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Handle_sceneLoaded;
    }
}
