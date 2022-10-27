using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    [SerializeField] Transform hookTransform;

    private Camera mainCamera;
    [Header("Hook attributes")]
    [SerializeField] int length;
    [SerializeField] int strength;
    [SerializeField] int fishCount;

    private Collider2D coll;

    private bool canMove = true;

    //List of fishes

    private Tweener cameraTween;

    private void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        //List of fishes
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }
}
