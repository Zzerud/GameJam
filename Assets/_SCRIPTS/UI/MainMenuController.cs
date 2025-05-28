using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        Loading.instance.StartLoading("Chapter1_1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
