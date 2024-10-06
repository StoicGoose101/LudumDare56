using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Food : MonoBehaviour
{

	public float Amount = 1;
    public float BiteSize = 0.1f;


    void Start()
	{
        //OriginalScale = transform.
	}

	void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Amount -= BiteSize;
        //Destroy(col.transform.gameObject);

        if (Amount > 0)
        {
            transform.localScale = new Vector3(Amount, Amount, 1);
        }
        else
        {
            //"You loose"
        }

    }
}
