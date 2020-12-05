using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static GameManager instance;
    public Database weaponDB;
    public WeaponFactory weaponFactory;
    public WeaponManager playerWM;
    private void Awake() {
        CheckSingleton();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameManager Instance()
    {
        return instance;
    }

    private void CheckSingleton()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }
}
