using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY WORKS FOR FLAT PLATFORMS, WILL BREAK IF PLATFROM IS AT AN ANGLE
public class Platform : MonoBehaviour
{
    public int MaxCoyoteFrames = 3;
    public LayerMask PlayerLayer = 1 << 3;

    static float baseOffset = 0.2f; // an adjust value to account for floating point BS
    private int CurrCoyoteFrames = 0;
    private BoxCollider2D extendedBox;
    // Start is called before the first frame update
    void Start()
    {
        CurrCoyoteFrames = 0;

        //extendedBox = GetComponent<BoxCollider2D>();
        //create a collision box that will extend outwards when the player touches the platform
        //easier to just do this at startup, rather than when the player touches the platform
        extendedBox = gameObject.AddComponent<BoxCollider2D>();
        //ensure only the play interacts with the coyote platform
        extendedBox.includeLayers = PlayerLayer;
        extendedBox.excludeLayers = 0b_1111_1111_1111_1111 ^ PlayerLayer.value;

        //add a trigger to the platform for detecting when the player is on top of the platform
        BoxCollider2D triggerBox = gameObject.AddComponent<BoxCollider2D>();
        triggerBox.isTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if in coyote time
        if(CurrCoyoteFrames > 0)
        {
            //print(CurrCoyoteFrames);
            CurrCoyoteFrames--;
            if( CurrCoyoteFrames == 0 )
            {
                extendedBox.size = new Vector2(1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" )
        {
            //get player position
            //offset height to get the position of the base of the player
            float playerBase = collision.transform.position.y - collision.bounds.extents.y;
            //get height of top of platform
            float platformHeight = transform.position.y + (transform.localScale.y / 2);

            
            //check if that bottom point is above the top of the platform based on the size of the platform
            if (playerBase + baseOffset > platformHeight)
            {
                //if so, activate the coyote platform
                print("on");
                extendedBox.size = new Vector2(2, 1); //This number is subject to change in the future
                CurrCoyoteFrames = 0;
            }
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("off");
            CurrCoyoteFrames = MaxCoyoteFrames;
        }
            
    }
}
