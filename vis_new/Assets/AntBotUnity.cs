using SkladModel;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntBotUnity : MonoBehaviour
{

    public AntStateChange antStateChange = new AntStateChange();
    internal DateTime startTime;
    SpriteRenderer sr;

    public void SetPosition()
    {
        SetPosition(main.getPosition(antStateChange.xCoordinate, antStateChange.yCoordinate));
    }

    public void SetPosition(Vector2 vector2)
    {
        transform.position = vector2;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    public Color GetColor()
    {
        return sr.color;
    }

    public IEnumerator ChangeCollor(int time, Color color)
    {
        yield return new WaitForSeconds(time);
        sr.color = color;
    }

        // Update is called once per frame
    void Update()
    {
        double shift = (DateTime.Now - startTime).TotalSeconds - antStateChange.lastUpdated;
        if (antStateChange.xSpeed != 0 || antStateChange.ySpeed != 0)
        {
            SetPosition(main.getPosition((antStateChange.xCoordinate + antStateChange.xSpeed * shift),
                                         (antStateChange.yCoordinate + antStateChange.ySpeed * shift)));
        }      
    }
}
