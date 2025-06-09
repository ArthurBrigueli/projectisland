using System.Collections;
using TMPro;
using UnityEngine;

public class GetPointsTree : MonoBehaviour
{
    public TextMeshProUGUI Money;

    private int point;


    public void AddPoint(int point)
    {
        this.point += point;        
    }

    public void RemovePoint(int point)
    {
        this.point -= point;
    }

    public int GetPoint()
    {
        return point;
    }

    void Start()
    {
        StartCoroutine(DropPoint());
    }

    IEnumerator DropPoint()
    {
        while (true)
        {
            point += 1;
            yield return new WaitForSeconds(1);
            Money.text = point.ToString();
        }
    }
}
