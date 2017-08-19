using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class I18nText : MonoBehaviour {

    Text text;
    public string Text {
        get { return text != null ? text.text : null; }
        set {
            if(text != null) {
                text.text = value;
            }
        }
    }

    public string key;

    void Awake() {
        text = GetComponent<Text>();
    }

}
