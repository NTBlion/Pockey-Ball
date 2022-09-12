using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Stick _stick;


    private void OnEnable()
    {
        _stick.FinishGame += OnFinishGame;
    }

    private void OnDisable()
    {
        _stick.FinishGame -= OnFinishGame;
    }

    private void OnFinishGame()
    {
        Debug.Log("Game is Finished");
    }
}
