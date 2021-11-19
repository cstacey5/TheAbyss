using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{

    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
    private Image image;
    [SerializeField]
    private Text statText;

    [SerializeField]
    private float lerpSpeed;


    private float currentImageFill;
    public float MyMaxValue { get; set; }

    private float currentValue;
    //use a property for mycurrentvalue to set restraints on currentvalue aswell as to be able to use this in a different script
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if(value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if(value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }
            currentImageFill = currentValue / MyMaxValue;
            statText.text = currentValue.ToString("F0") + " / " + MyMaxValue;
        }

    }
    

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentImageFill != image.fillAmount)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, currentImageFill, Time.deltaTime * lerpSpeed); //use lerp to smooth the transition when changing health
        }
    }

    //initialize properties
    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
