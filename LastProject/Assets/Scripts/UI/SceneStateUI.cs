using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneStateUI : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    void Update()
    {
        _text.text = GameManager.Instance.sceneState.ToString();
    }
}