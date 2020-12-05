using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    StateManager sm;
    [SerializeField]
    Image bg;
    [SerializeField]
    Image mg;
    [SerializeField]
    Image fg;
    [SerializeField]
    float updateSpeedSeconds;
    void Start()
    {
        if (sm == null)
        {
            sm = GetComponentInParent<StateManager>();
            bg = transform.GetChild(0).gameObject.GetComponent<Image>();
            mg = transform.GetChild(1).gameObject.GetComponent<Image>();
            fg = transform.GetChild(2).gameObject.GetComponent<Image>();
        }
        else
        {

        }
        sm.onHealthPctChanged += HandleHpChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        mg.fillAmount = Mathf.Lerp(mg.fillAmount, fg.fillAmount, 0.05f);
    }

    private void HandleHpChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = fg.fillAmount;
        //每过updateSpeedSenconds秒更新一次.
        float elapsed = 0f;
        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            fg.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        fg.fillAmount = pct;
    }
}
