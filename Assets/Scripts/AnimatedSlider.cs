using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimatedSlider : MonoBehaviour {

    public Slider thisSlider;
    float sliderLerpValue;
    public Text thisText;
    float sliderSpeed = .9f;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AnimateSlider(int animateToValue, int animateFromValue, string textPrefix, string textVariant) { 
    	if (thisSlider.value != animateToValue) {
			sliderLerpValue += Time.deltaTime * sliderSpeed;
			thisSlider.value = Mathf.Lerp (animateFromValue, animateToValue, sliderLerpValue);
		} else {
			thisText.text = textPrefix + textVariant;
			sliderLerpValue = 0;
		}
	}
}
