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

    private bool canMove;

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

    public void StartFishing()
    {
        length = -50; //IdleManager
        strength = 3; //IdleManager
        fishCount = 0;
        float time = (-length) * .1f;

        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * .25f, false).OnUpdate(delegate
          {
              if (mainCamera.transform.position.y <= -11)
              {
                  transform.SetParent(mainCamera.transform);
              }
          }).OnComplete(delegate{
            coll.enabled = true;
            cameraTween = mainCamera.transform.DOMoveY(0, time*5, false).OnUpdate(delegate 
            {
                if (mainCamera.transform.position.y <= -25f)
                {
                    StopFishing();
                }
            });
          });

        //Screen(GAME)
        coll.enabled = false;
        canMove = true;
        //Clear the hook
    }

    void StopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            int num = 0;
            //Clear out the hook from the fishes
            //IdleManager Totalgain = num
            //SceneManager End Screen
        });
    }
}
