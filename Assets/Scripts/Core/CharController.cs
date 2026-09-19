using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public Vector2 CurrentMovement {get; set;}
    public bool NormalMovement {get; set;}

    private Rigidbody2D myRigidbody2D;
    private RubberBandController rubberBand;
    void Start()
    {
        NormalMovement = true;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        rubberBand = GetComponent<RubberBandController>();
    }

    private void FixedUpdate()
    {
        if (NormalMovement)
        {
            MoveCharacter();
        }
    }

    // This is the only place that may call myRigidbody2D.MovePosition: MovePosition calls
    // don't stack, only the last one before a physics step wins, so horizontal input and
    // RubberBandController's vertical target are combined into a single call here.
    private void MoveCharacter()
    {
        float targetX = myRigidbody2D.position.x + CurrentMovement.x * Time.fixedDeltaTime;
        float targetY = rubberBand != null
            ? rubberBand.TargetY
            : myRigidbody2D.position.y + CurrentMovement.y * Time.fixedDeltaTime;

        myRigidbody2D.MovePosition(new Vector2(targetX, targetY));
    }

    public void MovePosition(Vector2 newPosition)
    { 
        myRigidbody2D.MovePosition(newPosition); 
    }

    public void SetMovement(Vector2 newPosition)
    {
        CurrentMovement = newPosition;
    }
 

}
