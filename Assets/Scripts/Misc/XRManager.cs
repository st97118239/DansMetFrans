using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRManager : MonoBehaviour
{
    [SerializeField] private GameObject xrPrefab;

    private GameObject xrObj;

    private void Awake()
    {
        xrObj = Instantiate(xrPrefab);
    }

    private void Start()
    {
        DontDestroyOnLoad(xrObj);

        SceneManager.LoadScene(1);
    }
}
