using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Cell : MonoBehaviour
{
    public SpriteRenderer sr;
    private bool alive = false;
    private byte health = 0;
    public bool IsAlive
    {
        get
        {
            return alive;
        }
    }

    public byte Health
    {
        get
        {
            return health;
        }
    }
    // Use this for initialization
    void Start()
    {
		LevelManager.instance.FrameEndCallback += FrameEndCheck;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
    }

    public void OnMouseDown()
    {
        if (LevelManager.instance.generate && !alive)
            MakeAlive();
        else if (LevelManager.instance.generate && alive)
            Kill();
    }

    public void Kill()
    {
        health = 0;
        alive = false;
        ChangeColor(Color.white);
    }

    public void MakeAlive()
    {
        alive = true;
        ChangeColor(Color.black);
    }

    public void IncrementHealth()
    {
        health++;
    }

    public void FrameEndCheck()
    {
        if (!alive && health == 3)
        {
            MakeAlive();
        }
        else if (alive && (health < 2 || health > 3))
        {
            Kill();
        }

		 health = 0;
    }

    private void ChangeColor(Color _color)
    {
        sr.color = _color;
    }

}
