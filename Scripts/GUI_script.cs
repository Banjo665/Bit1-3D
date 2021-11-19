using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_script : MonoBehaviour
{
    GUIStyle style = new GUIStyle();

    void Start(){
        style.fontSize = 20;
        style.normal.textColor = Color.black;
    }

    void OnGUI() {
        GUI.Label (new Rect (new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(500, 500)), "+", style);
    }
}
