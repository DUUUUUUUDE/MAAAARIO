using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRestartGameElement
{
    void RestartGame();

}

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddRestartGameElement(IRestartGameElement RestartGameElement)
    {

        m_RestartGameElements.Add(RestartGameElement);

    }


    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    public void SoftRestart()
    {

        foreach (IRestartGameElement l_RestartGameElement in m_RestartGameElements)
            l_RestartGameElement.RestartGame();

    }

}
