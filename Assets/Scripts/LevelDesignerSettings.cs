using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesignerSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmButton;
    
    private int width = 10;
    private int height = 10;
    private string name = "Default";

    private void updateHeight(string height)
    {
        int tempHeight;
        if(!int.TryParse(height, out tempHeight))
        {
            Debug.Log("Invalid Height Entered");
            return;
        }
        if(tempHeight < 0)
        {
            Debug.Log("Invalid Height Entered");
            return;
        }
        this.height = tempHeight;
    }

    private void updateWidth(string width)
    {
        int tempWidth;
        if (!int.TryParse(width, out tempWidth))
        {
            Debug.Log("Invalid Width Entered");
            return;
        }
        if (tempWidth < 0)
        {
            Debug.Log("Invalid Width Entered");
            return;
        }
        this.width = tempWidth*2;
    }

    public void confirmSettings()
    {
        updateWidth(widthInput.text);
        updateHeight(heightInput.text);
        name = nameInput.text;
    }

    public Button getConfirm()
    {
        return confirmButton;
    }

    public int getHeight()
    {
        return height;
    }

    public int getWidth()
    {
        return width;
    }

    public string getName()
    {
        return name;
    }
}
