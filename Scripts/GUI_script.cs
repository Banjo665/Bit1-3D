using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_script : MonoBehaviour
{
    GUIStyle cross = new GUIStyle();
    GUIStyle textStyle = new GUIStyle();
    player_script player;

    void Start(){
        cross.fontSize = 20;
        cross.normal.textColor = Color.black;
        textStyle.fontSize = 20;
        textStyle.normal.textColor = Color.white;
        player = GameObject.FindWithTag("Player").GetComponent<player_script>();
    }

    void OnGUI() {
        GUI.Label (new Rect (new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(500, 500)), "+", cross);
    }
}
