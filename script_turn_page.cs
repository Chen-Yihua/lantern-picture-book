using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class script_turn_page : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private float minSwipeDistance = 50f;
    static public bool turn_page = true;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                CheckSwipe();
            }
        }
    }

    private void CheckSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) > minSwipeDistance)
        {
            float swipeAngle = Mathf.Atan2(fingerUpPosition.y - fingerDownPosition.y, fingerUpPosition.x - fingerDownPosition.x) * Mathf.Rad2Deg;
            float leftThreshold = 135f;
            float rightThreshold = -45f;

            if (swipeAngle > rightThreshold && swipeAngle < leftThreshold)
            {
                // 往右滑動
                LoadPreviousScene();
            }
            else
            {
                // 往左滑動
                LoadNextScene();
            }
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (turn_page == true && currentSceneIndex <= 30)
        {
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }

    private void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
            turn_page = true;
        }
    }

    public void ModelScene(string name)
    {
        Debug.Log("ok");
        SceneManager.LoadScene(name);
    }
}
