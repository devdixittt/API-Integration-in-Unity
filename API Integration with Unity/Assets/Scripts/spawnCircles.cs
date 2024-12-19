using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnCircles : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject restartButton;
    public int maxCircle = 10;
    public int minCircle = 5;

    private List<GameObject> spawnCircle = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SpawnCircles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCircles()
    {
        foreach (var circle in spawnCircle)
        {
            Destroy(circle);
        }
        spawnCircle.Clear();

        int Range = Random.Range(minCircle, maxCircle+1);
        for(int i = 0; i < Range; i++)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(100, Screen.width - 100), Random.Range(100, Screen.height - 100)));
            GameObject circle = Instantiate(circlePrefab, pos, Quaternion.identity);
            spawnCircle.Add(circle);
        }
    }

    public List<GameObject> GetSpawnCircle()
    {
        return spawnCircle;
    }

    public void RemoveCircle(GameObject circle)
    {
        if (spawnCircle.Contains(circle))
        {
            spawnCircle.Remove(circle);
        }

        if (spawnCircle.Count == 0)
        {
            restartButton.SetActive(true);
        }
    }

    public void ShowRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
