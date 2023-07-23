using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVisualizer : MonoBehaviour
{
    BaseGravitySpell parent;
    Material material;
    int screenPositionID;
    int fieldStrengthID;

    private void Awake()
    {
        parent = transform.parent.GetComponent<BaseGravitySpell>();
        material = GetComponent<Renderer>().material;

        screenPositionID = Shader.PropertyToID("ScreenPosition");
        fieldStrengthID = Shader.PropertyToID("FieldStrength");
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * parent.PullRadius;
        material.SetFloat("_FieldStrength", parent.PullForce);
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = GameManager.Instance.MainCamera.WorldToViewportPoint(transform.position);
        material.SetVector("_ScreenPosition", screenPos);
    }
}
