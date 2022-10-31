using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private List<Fish> hookedFishes;

    private Tweener cameraTween;

    [Header("Start Button")]
    [SerializeField] Button startButton;

    private void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
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
        startButton.gameObject.SetActive(false);
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
        hookedFishes.Clear();

        
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
            for (int i = 0; i < hookedFishes.Count; i++)
            {
                hookedFishes[i].transform.SetParent(null);
                hookedFishes[i].ResetFish();
                num += hookedFishes[i].Type.price;
            }
            //IdleManager Totalgain = num
            //SceneManager End Screen
        });

        startButton.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish") && fishCount!=strength)
        {
            fishCount++;
            Fish component = collision.GetComponent<Fish>();
            component.Hooked();
            hookedFishes.Add(component);
            collision.transform.SetParent(transform);
            collision.transform.position = hookTransform.position;
            collision.transform.rotation = hookTransform.rotation;
            collision.transform.localScale = Vector3.one;

            collision.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                collision.transform.rotation = Quaternion.identity;
            });

            if (fishCount == strength)
            {
                StopFishing();
            }
        }
    }

    
}
