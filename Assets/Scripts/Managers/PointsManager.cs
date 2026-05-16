using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : Singleton<PointsManager>
{

    public int Points {get; set;}

    private readonly string POINTS_KEY = "This_is_special_key_for_points";

    private void Start()
    {
        LoadPoints();
    }

    private void LoadPoints()
    {
        Points = PlayerPrefs.GetInt(POINTS_KEY);
    }

    public void AddPoints(int amount) {
        Points += amount;
        PlayerPrefs.SetInt(POINTS_KEY, Points);
    }

    public void RemovePoints(int amount)
    {
        Points -= amount;
        PlayerPrefs.SetInt(POINTS_KEY, Points);
    }


}
