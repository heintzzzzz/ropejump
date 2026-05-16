using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public Vector2 CurrentMovement {get; set;}
    public bool NormalMovement {get; set;}

    private Rigidbody2D myRigidbody2D;
    private CharMovement chm;
    void Start()
    {
        NormalMovement = true;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        chm = GetComponent<CharMovement>();
    }

    private void FixedUpdate()
    {
        if (NormalMovement && chm != null && chm.isPlayer)
        {
            MoveCharacter(); 
        }
    }

    private void MoveCharacter()
    {
        Vector2 currentMovePosition = myRigidbody2D.position + CurrentMovement * Time.fixedDeltaTime;
        myRigidbody2D.MovePosition(currentMovePosition);
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
