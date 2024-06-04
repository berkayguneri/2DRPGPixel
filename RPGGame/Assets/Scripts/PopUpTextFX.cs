using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float dissaperingSpeed;
    [SerializeField] private float colorDisapieranceSpeed;

    [SerializeField] private float lifeTime;


    private float textTimer;


    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }
    private void Update()
    {
        textTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorDisapieranceSpeed * Time.deltaTime;

            myText.color=new Color(myText.color.a,myText.color.g,myText.color.b,alpha);

            if (myText.color.a < 50)
                speed = dissaperingSpeed;

            if (myText.color.a <= 0)
                Destroy(gameObject);

        }
        
            
    }
}
