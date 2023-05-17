using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;// include this -   load , unload scene
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Options;
    Resolution[] resolutions;
    private int startUpScene = 0;

    public Dropdown resolutionDropdown;

    public Dropdown graphicQualityDropDown;
    bool detected;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;
    bool isOpen;
    private void Awake()
    {

        startUpScene = SceneManager.GetActiveScene().buildIndex;

        if (startUpScene == 0 && PlayerPrefs.GetString("isOld") != "true")
        {
            infoText.text = "Welcome please test your microphone, by saying Hello!";
            StartCoroutine(DisplayText(10f));
        }

        resolutions = Screen.resolutions;//get the resolutions available
        resolutionDropdown.ClearOptions();//clear the dropdown
        int currentIndex = 0;
        int selectedIndex = 0;
        List<string> options = new List<string>();//create a list with resolution options
        foreach (var item in resolutions)
        {
            selectedIndex++;
            string option = item.width + "x" + item.height + " " + item.refreshRate + "Hz";//concat string option with height width and refreshrate
            options.Add(option);//add each options 
            if (item.width == Screen.currentResolution.width && item.height == Screen.currentResolution.height)
            {
                currentIndex = selectedIndex;//set the resolution to current one
            }
        }
        resolutionDropdown.AddOptions(options);//add the options
        resolutionDropdown.value = currentIndex;//set the current selected option
        resolutionDropdown.RefreshShownValue();//refresh them

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GraphiscLevel"));
        graphicQualityDropDown.value = PlayerPrefs.GetInt("GraphiscLevel");
    }

    private void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate.ToString() + " fps";
            time -= pollingTime;
            frameCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && startUpScene != 0)
        {
            if (isOpen)
            {
                BackOptions();
                isOpen = false;

            }
            else
            {
                LoadOptions();
                isOpen = true;
            }
        }
    }
    public IEnumerator DisplayText(float value)
    {
        yield return new WaitForSeconds(0.5f);
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(value);
        infoText.gameObject.SetActive(false);
    }

    IEnumerator StartGame(float value)
    {
        yield return new WaitForSeconds(2f);
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(value);
        infoText.gameObject.SetActive(false);
        SceneManager.LoadScene("Main_Level");// make sure scene is added in build settings
    }

    public void setHello()
    {

        infoText.text = "Your mic was detected.";
        StartCoroutine(DisplayText(5f));
    }
    public void LoadMainMenu() //loadmainmenu
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }


    public void LoadOptions()
    {
        MainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }


    public void BackOptions()
    {
        MainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        infoText.text = "Starting game!";
        StartCoroutine(StartGame(1.5f));
        PlayerPrefs.SetString("isOld", "true");

    }

    public void ExitGame()
    {
        Application.Quit();
        PlayerPrefs.SetString("isOld", "false");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("isOld", "false");
    }

    public void SetQuality(int qualityIndex) //quality of the graphics
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphiscLevel", qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void HidePauseMenu()
    {
        if (MainMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            Options.SetActive(true);
            return;
        }

        if (startUpScene == 0)
        {
            LoadOptions();
            return;
        }
        infoText.text = "Sorry you must pause first!";
        StartCoroutine(DisplayText(2f));
    }

    public void BackOptionPauseMenu()
    {
        if (startUpScene == 0)
        {
            BackOptions();
            return;
        }

        if (MainMenu.activeSelf == false)
        {
            Options.SetActive(false);
            pauseMenu.SetActive(true);
            return;
        }


        infoText.text = "Sorry you can't go back! ";
        StartCoroutine(DisplayText(2f));
    }


}
