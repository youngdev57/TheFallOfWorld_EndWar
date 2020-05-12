using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedButton : MonoBehaviour
{
    RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Transform selectedBtn = hit.transform;

            float position = Mathf.Abs(Vector3.Distance(selectedBtn.position, transform.position));
            position = 1f - Mathf.Clamp((position + 0.025f) * 10, 0f, 1f);

            if (position < .3f)
                position = 0;

            hit.transform.localScale = Vector3.one * (1f + (.5f * position));
        }
    }
}
