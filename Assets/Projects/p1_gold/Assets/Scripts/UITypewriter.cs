using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITypewriter : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public string message;

    public AudioClip typingSound;

    public float timeBetweenChars;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "";
        txt.alpha = 0;
    }

    public IEnumerator PlayText()
    {
        txt.alpha = 100;
        foreach (char c in message)
        {
            txt.text += c;
            AudioSource.PlayClipAtPoint(typingSound, Camera.main.transform.position);
            yield return new WaitForSeconds(timeBetweenChars);
        }
    }
}
