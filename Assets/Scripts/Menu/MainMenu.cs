using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    [SerializeField] private string[] SceneList;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private GameObject _loaderGrid;
    [SerializeField] private Image _progressBar;

    [SerializeField] GameObject sceneButtonPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeSceneSelector();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene()
    {

    }

    private void InitializeSceneSelector()
    {
        foreach (Transform child in _loaderGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach ( string scene in SceneList )
        {
            var levelButtonObject = Instantiate(sceneButtonPrefab, _loaderGrid.transform );
            Button levelButton = levelButtonObject.GetComponent<Button>();
            levelButton.onClick.AddListener( delegate { LoadScene(scene); } ) ;
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = scene;
        }

    }

    public async void LoadScene( string sceneName )
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            //_progressBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;

        _loaderCanvas.SetActive(false);

    }
}
