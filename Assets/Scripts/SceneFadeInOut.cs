using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour 
{
    public float fadeSpeed = 1.5f;
    private bool sceneStarting = true;

    private GUITexture texture;
    void Awake() {
        texture = GetComponent<GUITexture>();
        texture.pixelInset = new Rect(0f,0f,Screen.width, Screen.height);
    }

    void Update() {
        if(sceneStarting) {
            StartScene();
        }
    }

    void FadeToClear() {
        texture.color = Color.Lerp(texture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack() {
        texture.color = Color.Lerp(texture.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene() {
        FadeToClear();

        if(texture.color.a <= 0.05f) {
            texture.color = Color.clear;
            texture.enabled = false;
            sceneStarting = false;
        }
    }

    public void EndScene() {
        texture.enabled = true;

        FadeToBlack();

        if(texture.color.a >= 0.95f) {
            Application.LoadLevel(0);
        }
    }
}
