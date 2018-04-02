using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : ItemSuper {

    bool fly = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(!fly)
        {
            GetComponent<Collider>().enabled = false;
            fly = true;
            StartCoroutine(FlyUp());
        }
    }

    IEnumerator FlyUp()
    {
        float scaleTime = 0.2f;
        float flyTime = 2.0f;
        float speed = 300.0f;
        Vector3 scale = transform.localScale;
        Vector3 targetScale = scale;
        targetScale.x *= 0.2f;
        targetScale.z *= 0.2f;
        targetScale.y *= 2.0f;
        while (flyTime >= 0)
        {
            transform.position = transform.position + Vector3.up * (speed * Time.deltaTime);
            if (scaleTime >= 0)
            {
                transform.localScale = Vector3.Lerp(scale, targetScale, 1.0f - scaleTime / 0.2f);
                scaleTime -= Time.deltaTime;
            }
            flyTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject, 0.1f);
    }
}
