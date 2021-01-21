using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class AttributionBar
{
    public Image bg;
    public Image mg;
    public Image fg;
}

public class ActorUIBarManager : MonoBehaviour
{
    [SerializeField]
    StateManager sm;

    [SerializeField]
    float updateSpeedSeconds;
    
    public AttributionBar hpBar = new AttributionBar();
    public AttributionBar NlBar = new AttributionBar();
    void Start()
    {
        if (sm == null)
        {
            sm = GetComponentInParent<StateManager>();
            hpBar.bg = transform.GetChild(0).gameObject.GetComponent<Image>();
            hpBar.mg = transform.GetChild(1).gameObject.GetComponent<Image>();
            hpBar.fg = transform.GetChild(2).gameObject.GetComponent<Image>();
        }
        else
        {

        }
        sm.onHealthPctChanged += HandleHpChanged;
        if(sm.gameObject.CompareTag("Player"))
            sm.onEndurancePctChanged += HandleNlChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        hpBar.mg.fillAmount = Mathf.Lerp(hpBar.mg.fillAmount, hpBar.fg.fillAmount, 0.05f);
        if(sm.gameObject.CompareTag("Player"))
            NlBar.mg.fillAmount = Mathf.Lerp(NlBar.mg.fillAmount, NlBar.fg.fillAmount, 2.0f * Time.deltaTime);
    }

    private void HandleHpChanged(float pct)
    {
        StartCoroutine(ChangeToPct(hpBar,pct));
    }
    
    private void HandleNlChanged(float pct)
    {
        StartCoroutine(ChangeToPct(NlBar,pct));
    }

    private IEnumerator ChangeToPct(AttributionBar attributionBar,float pct)
    {
        float preChangePct = attributionBar.fg.fillAmount;
        //每过updateSpeedSenconds秒更新一次.
        float elapsed = 0f;
        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            attributionBar.fg.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        attributionBar.fg.fillAmount = pct;
        yield return null;
    }
}
