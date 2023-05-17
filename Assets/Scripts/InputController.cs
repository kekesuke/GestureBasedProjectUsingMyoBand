using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;
//using System.Linq;
using UnityEngine.Windows.Speech;


public class InputController : MonoBehaviour
{

    private UIController uiController;

    private SoundController soundController;
    private string playerChoice;

    GrammarRecognizer gr;
    // Action is in System, using System; or System.Action
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    bool detected;

    private void Start()
    {
        //Game states 
        actions.Add("startgame", StartGame);  // start the game. Menu screen to Main Game Scene.
        actions.Add("quit", ExitGame);  // exit the game 
        actions.Add("pause", PauseGame);  // pause the game
        actions.Add("resume", ResumeGame);  // resume the game

        actions.Add("hello", Hello);  //Select rock

        //Sound states
        actions.Add("volumeup", VolumeUp);  // volume up 10%
        actions.Add("volumedown", VolumeDown);  // volume down 10%
        actions.Add("stopmusic", StopMusic);  // Stop music set it to 0%
        actions.Add("musicon", MusicOn);  // Turn music on 100%
        //options input
        actions.Add("options", OpenOptions);  // open options
        actions.Add("back", GoBack);  // go back
        actions.Add("mainmenu", LoadMainMenu);  // go back to mainmenu



        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
            "SimpleGrammer.xml"), ConfidenceLevel.Low); //initilize the grammarrecognizer
        Debug.Log("Grammar is loaded " + gr.GrammarFilePath);
        gr.OnPhraseRecognized += Gr_OnPhraseRecognized; //
        gr.Start();//start the grammer
        if (gr.IsRunning) Debug.Log("GR is running.");

    }

    private void Gr_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Recognised Something ....");
        //read the semantic meanings from the args returned
        //put them in a string to print a message in the console
        StringBuilder message = new StringBuilder();

        SemanticMeaning[] meanings = args.semanticMeanings;
        //return a set of name/value pairs - key/values
        foreach (SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();
            message.Append("key: " + keyString + ", value: " + valueString + System.Environment.NewLine);
            actions[valueString].Invoke();//invoke the methos associated with the action from the valuestring
        }
        Debug.Log(message);


    }
    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= Gr_OnPhraseRecognized;
            gr.Stop();
        }
    }

    private void Awake()
    {

        uiController = GetComponent<UIController>();
        soundController = FindObjectOfType<SoundController>().GetComponent<SoundController>();
    }


    public void Hello()
    {
        if (detected) return;
        uiController.setHello();
        detected = true;
    }

    public void PauseGame()
    {
        uiController.LoadOptions();
    }

    public void VolumeUp()
    {
        soundController.setMasterVolumeUp();
    }
    public void StopMusic()
    {
        soundController.StopMasterVolume();
    }
    public void MusicOn()
    {
        soundController.ResumeMusic();
    }

    public void VolumeDown()
    {
        soundController.setMasterVolumeDown();
    }

    public void ResumeGame()
    {
        uiController.BackOptions();
    }

    public void LoadMainMenu()
    {
        uiController.LoadMainMenu();
    }
    public void StartGame()
    {
        uiController.StartGame();
    }

    public void ExitGame()
    {
        uiController.ExitGame();
    }

    public void OpenOptions()
    {
        uiController.HidePauseMenu();
    }

    public void GoBack()
    {
        uiController.BackOptionPauseMenu();
    }


}
