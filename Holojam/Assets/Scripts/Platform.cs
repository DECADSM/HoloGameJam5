using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public int MaxCoyoteFrames = 3;

    private int CurrCoyoteFrames = 0;
    private BoxCollider2D extendedBox;
    // Start is called before the first frame update
    void Start()
    {
        CurrCoyoteFrames = 0;
        BoxCollider2D[] boxColliders = GetComponents<BoxCollider2D>();

        if (boxColliders != null && boxColliders.Length > 0)
        {
            if (boxColliders[0].isTrigger)
            {
                extendedBox = boxColliders[1];
            }
            else
            {
                extendedBox = boxColliders[0];
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CurrCoyoteFrames > 0)
        {
            CurrCoyoteFrames--;
            if( CurrCoyoteFrames == 0 )
            {
                extendedBox.size = new Vector2(1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("on");
        extendedBox.size = new Vector2(2, 1);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("off");
        CurrCoyoteFrames = MaxCoyoteFrames;
    }
}
