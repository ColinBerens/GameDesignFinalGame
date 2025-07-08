using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHandler : MonoBehaviour
{
    SoundManager _soundManager;
    private void Start()
    {
        // Find the SoundManager in the scene
        _soundManager = FindFirstObjectByType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame()
	{
        _soundManager.PlayFull("ButtonClick");
        // Quit the application
        // If we are running in the editor, stop playing the scene
        while (!_soundManager.GetSoundInfo("ButtonClick").IsPlayed)
        {
            // Wait for the sound to finish playing
            System.Threading.Thread.Sleep(100);
        }
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
	public void ReplayGame()
	{
        // Play the button click sound
        _soundManager.PlayFull("ButtonClick");
        // Load the game scene
        while (!_soundManager.GetSoundInfo("ButtonClick").IsPlayed)
        {
            // Wait for the sound to finish playing
            System.Threading.Thread.Sleep(100);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
