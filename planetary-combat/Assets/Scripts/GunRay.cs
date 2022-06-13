using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRay : MonoBehaviour
{
    public float outTime = 3f;
    private float cTime;

    // Start is called before the first frame update
    void Start()
    {
        cTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cTime += Time.deltaTime;
        if (cTime >= outTime)
        {
            TestRay();
            cTime = 0;
        }
    }
    
    void TestRay()
    {
        float distance = 100; // 飛ばす&表示するRayの長さ
        float duration = 60;   // 表示期間（秒）

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
    }
}
