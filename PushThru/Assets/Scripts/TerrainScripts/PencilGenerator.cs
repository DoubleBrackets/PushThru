using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilGenerator : MonoBehaviour
{
    public bool usePlayerPrefs;
    public int count;

    public float minRad;
    public float maxRad;

    public GameObject prefab;

    private void Awake()
    {
        if(usePlayerPrefs)
        {
            GeneratePencils(PlayerPrefs.GetInt("PencilCount", 1));
        }
        else
        {
            GeneratePencils(count);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerPrefs.SetInt("PencilCount", 1);
        }
    }

    private void GeneratePencils(int count)
    {
        float angleRange = 30f;
        for (int x = 0;x < count;x++)
        {
            float radius = Random.Range(minRad, maxRad);
            float height = Random.Range(1, 6f);
            float angle = Random.Range(0, 2*Mathf.PI);

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + height * Vector3.up;

            Quaternion rotation = Quaternion.Euler(90+Random.Range(-angleRange, angleRange), 
                Random.Range(-angleRange, angleRange), 
                Random.Range(-angleRange, angleRange));
            GameObject newPencil = Instantiate(prefab, transform.position + offset, Quaternion.identity, transform);
            newPencil.transform.rotation = rotation;
        }       
    }
}
