using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotRay : MonoBehaviour
{
    public float outTime = 0.25f;
    public Color color;
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
        float duration = 1;   // 表示期間（秒）

        Vector3 vect = Camera.main.transform.forward - (transform.position - Camera.main.transform.position).normalized;
        Ray ray = new Ray(transform.position, vect);

        Debug.DrawRay(ray.origin, ray.direction * distance, color, duration, false);
    }
}
