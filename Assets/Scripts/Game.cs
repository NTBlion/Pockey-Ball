using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Ball _ball;


    private void OnEnable()
    {
        _ball.FinishGame += OnFinishGame;
    }

    private void OnDisable()
    {
        _ball.FinishGame -= OnFinishGame;
    }

    private void OnFinishGame()
    {
        Debug.Log("Game is Finished");
    }
}
