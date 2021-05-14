using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CricleCollision  
{
    public float x;
    public float y;
    public float r;//半径

    public CricleCollision(float x,float y,float r)
    {
        this.x = x;
        this.y = y;
        this.r = r;
    }

    public void Refresh(float x,float y)
    {
        this.x = x;
        this.y = y;
    }
}

public static class CircleCollision
{
    public static bool CricleCollisionCheck(CricleCollision cricleCollision1, CricleCollision cricleCollision2)
    {
        bool resule = false;
        float distance = Vector2.Distance(new Vector2(cricleCollision1.x, cricleCollision1.y), new Vector2(cricleCollision2.x, cricleCollision2.y));

        resule = distance <= (cricleCollision1.r + cricleCollision2.r);
        return resule;

    }
}
