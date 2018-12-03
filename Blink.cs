using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// this toggles a component (usually an Image or Renderer) on and off for an interval to simulate a blinking effect
public class Blink : MonoBehaviour
{

    // this is the UI.Text or other UI element you want to toggle
    public MaskableGraphic imageToToggle;

    public float interval = 1f;
    public float startDelay = 0.5f;
    public bool currentState = true;
    public bool defaultState = true;
    public bool isBlinking = true;
    public int numberOfBlink=6;


    

    void Start()
    {

    }

    

    public void ToggleStateTrue()
    {
        imageToToggle.color = Color.red;
        
        numberOfBlink -= 1;
        if (numberOfBlink == 0)
        {
            StopCoroutine("Wait");      
        }
        StartCoroutine(Wait());
        


    }
    public void ToggleStateFalse()
    {
        imageToToggle.color = Color.white;
        numberOfBlink -= 1;
        if (numberOfBlink == 0)
        {
           
            StopCoroutine("Wait");
           
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        isBlinking = !isBlinking;
       
        yield return new WaitForSeconds(0.2f);
        if (numberOfBlink > 0)
        {
            Invoke("ToggleState"+ isBlinking, startDelay);
        }
        else
        {
            imageToToggle.color = Color.white;
            yield return null;
        }
        
    }

}