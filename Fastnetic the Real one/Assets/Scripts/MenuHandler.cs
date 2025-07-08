using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    SoundManager _soundManager;
    RumbleManager RumbleManager;
    public string SceneToLoad = "Game";
    private void Start()
    {
        // Find the RumbleManager in the scene
        RumbleManager = FindFirstObjectByType<RumbleManager>();
        // Find the SoundManager in the scene
        _soundManager = FindFirstObjectByType<SoundManager>();
    }
    public void StartGame()
	{
        // Play the button click sound
        _soundManager.PlayFull("ButtonClick");
        // Play the rumble effect
        RumbleManager.RumblePulse(0.25f, 1f, 0.5f);
        // Load the game scene
        while (!_soundManager.GetSoundInfo("ButtonClick").IsPlayed)
        {
            // Wait for the sound to finish playing
            System.Threading.Thread.Sleep(100);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
	}
	public void QuitGame()
	{
        _soundManager.PlayFull("ButtonClick");
        // Quit the application
        // If we are running in the editor, stop playing the scene
        RumbleManager.RumblePulse(0.25f, 1f, 0.5f);
        while (!_soundManager.GetSoundInfo("ButtonClick").IsPlayed)
        {
            // Wait for the sound to finish playing
            System.Threading.Thread.Sleep(100);
        }
        Application.Quit();
		Debug.Log("Game is exiting...");
	}
}
