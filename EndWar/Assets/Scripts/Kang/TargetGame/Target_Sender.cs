using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Sender : MonoBehaviour
{
    [SerializeField]
    public Queue<GameObject> pool;
    public GameObject targetPrefab;

    void Start() => Initialize();

    void Initialize(int size = 3)
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            pool.Enqueue(AddTarget());
        }

        StartGame();
    }

    void StartGame()
    {
        StartCoroutine(SendTarget());
    }

    public GameObject GetTarget()
    {
        GameObject obj = null;

        if(pool.Count > 0)
            obj = pool.Dequeue();
        else
            obj = AddTarget();

        obj.SetActive(true);
        int rnd = Random.Range(10, 20);
        obj.GetComponent<TargetMove>().speed = rnd;
        return obj;
    }

    GameObject AddTarget()    //풀에 타겟 추가
    {
        GameObject inst = Instantiate(targetPrefab, transform.position, Quaternion.identity);
        inst.SetActive(false);
        inst.GetComponent<TargetMove>().sender = this;

        
        return inst;
    }

    public void Restore(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    ///////////////////////////////////////////////
    
    IEnumerator SendTarget()
    {
        int rnd = Random.Range(2, 4);
        yield return new WaitForSeconds(rnd);

        GetTarget();

        StartCoroutine(SendTarget());
    }
}
