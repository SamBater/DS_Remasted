using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    
    [System.Serializable]
    public class ObjAndSize
    {
        public string name;
        public int capacity;
    }
    [SerializeField]
    public static ObjectPool instance;
    public Dictionary<string,List<GameObject>> pools;
    [SerializeField]
    public List<ObjAndSize> objNames = new List<ObjAndSize>();

    private void Awake() {
        if(instance)
            Destroy(this);
        else
            instance = this;
    }
    private void Start() 
    {
        pools = new Dictionary<string, List<GameObject>>();
        for(int i=0;i<objNames.Count;i++)
        {
            Submit(objNames[i].name,objNames[i].capacity);
        }
    }

    public GameObject GetObject (string name)
    {
        List<GameObject> pool = pools[name];
        for(int i=0;i<pool.Count;i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        //TODO: expand pool
        pool.Add(pool[0]);
        return null;
    }

    //FUNCTION:实例化size个位于Resources文件夹以name命名的preferb
    public void Submit(string name,int size)
    {
        pools.Add(name,new List<GameObject>());
        GameObject protype = Resources.Load<GameObject>(name);
        List<GameObject> list = pools[name];
        for(int i=0;i<size;i++)
        {
            GameObject go = Instantiate(protype,transform.Find("ObjectPools"));
            if(i > 0) go.name = protype.name + i.ToString();
            list.Add(go);
            list[i].SetActive(false);
        }
    }
}
