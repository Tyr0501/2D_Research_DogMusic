using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingDogs : MonoBehaviour
{
    [SerializeField] private Transform objDogLeft;
    [SerializeField] private Transform objDogRight;
    [SerializeField] private float paddingRight;
    [SerializeField] private float paddingLeft;
    [SerializeField] private float paddingMid;
    [SerializeField] private float moveSpeed;
    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector3 initialPosition;
    private Vector3 initialPosition1;
    private Vector3 initialPosition2;
    private bool endTouch1 = false;
    private int indexTouch = 0;
    private int indexTouch1 = 0;
    private int indexTouch2 = 0;

    void Start()
    {
        Application.targetFrameRate = 240;
        mainCamera = GetComponent<Camera>();
        InitBounds();
    }

    private void Update()
    {
        Move();
    }

    void InitBounds()
    {
        mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Move()
    {
        if (Input.touchCount == 1)
        {
            Touch touch1 = Input.GetTouch(0);
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch1.position);
            if (endTouch1)
            {
                endTouch1 = false;
                initialPosition = touchPosition;
            }
            switch (touch1.phase)
            {
                case TouchPhase.Began:
                    indexTouch = touchPosition.x > 0 ? 2 : 1;
                    initialPosition = touchPosition;
                    break;
                case TouchPhase.Moved:
                    if (indexTouch == 1)
                    {
                        MoveObject(true, objDogLeft, touchPosition, initialPosition);
                        initialPosition = touchPosition;
                    }
                    else if (indexTouch == 2)
                    {
                        MoveObject(false, objDogRight, touchPosition, initialPosition);
                        initialPosition = touchPosition;
                    }
                    break;
            }
        }
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector3 touchPosition1 = mainCamera.ScreenToWorldPoint(touch1.position);
            Vector3 touchPosition2 = mainCamera.ScreenToWorldPoint(touch2.position);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                indexTouch1 = touchPosition1.x > 0 ? 2 : 1;
                indexTouch2 = touchPosition2.x > 0 ? 2 : 1;
                initialPosition1 = touchPosition1;
                initialPosition2 = touchPosition2;
            }

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                if (indexTouch1 == 1)
                {
                    MoveObject(true, objDogLeft, touchPosition1, initialPosition1);
                    initialPosition1 = touchPosition1;
                }
                else if (indexTouch1 == 2)
                {
                    MoveObject(false, objDogRight, touchPosition1, initialPosition1);
                    initialPosition1 = touchPosition1;
                }
                if (indexTouch2 == 1)
                {
                    MoveObject(true, objDogLeft, touchPosition2, initialPosition2);
                    initialPosition2 = touchPosition2;
                }
                else if (indexTouch2 == 2)
                {
                    MoveObject(false, objDogRight, touchPosition2, initialPosition2);
                    initialPosition2 = touchPosition2;
                }
            }
            if (touch1.phase == TouchPhase.Ended)
            {
                endTouch1 = true;
                indexTouch = indexTouch1 == 1 ? 2 : 1;
            }
            if (touch2.phase == TouchPhase.Ended)
            {
                endTouch1 = true;
                indexTouch = indexTouch1 == 2 ? 2 : 1;
            }
        }
    }

    private void MoveObject(bool isMovingObjLeft, Transform obj, Vector3 touchPosition, Vector3 initialPosition)
    {
        float test = touchPosition.x - initialPosition.x;
        float currentValue = Mathf.Lerp(0f, 1, test);
        Debug.Log(currentValue);

        Vector2 newPosition = obj.position + (touchPosition - initialPosition) * moveSpeed;
        newPosition.x = Mathf.Clamp(newPosition.x, isMovingObjLeft ? minBounds.x + paddingLeft : 0.1f + paddingMid, isMovingObjLeft ? -0.1f - paddingMid : maxBounds.x - paddingRight);
        newPosition.y = -2f;
        obj.position = newPosition;
    }
}
