using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityChan;

public class AmdroidController : MonoBehaviour
{
    [SerializeField]
    private Button left;
    [SerializeField]
    private Button right;
    [SerializeField]
    private Button up;
    [SerializeField]
    private Button down;
    [SerializeField]
    private Button isController;
    private UnityChanControlScriptWithRgidBody uccs;
    // Start is called before the first frame update
    void Start()
    {
        uccs = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityChanControlScriptWithRgidBody>();
        isController.OnClickAsObservable()
            .Subscribe(_ =>
            {
                isController.gameObject.SetActive(false);
                uccs.isAndroid = true;
                left.gameObject.SetActive(true);
                right.gameObject.SetActive(true);
                up.gameObject.SetActive(true);
                down.gameObject.SetActive(true);
            });
    }
    public void holiUp()
    {
        uccs.h = 1;
    }
    public void holiDown()
    {
        uccs.h = -1;
    }
    public void vecUp()
    {
        uccs.v = 1;
    }
    public void vecDown()
    {
        uccs.v = -1;
    }
    public void holiReset()
    {
        uccs.h = 0;
    }
    public void vecReset()
    {
        uccs.v = 0;
    }
}
