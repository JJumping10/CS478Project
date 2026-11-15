using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageScript : MonoBehaviour
{
    public Sprite[] currentImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite ChangeBackground(int level)
    {
        if(level == 0) return currentImage[1];//level 1 background -> level 2 background
        if(level == 1) return currentImage[2];//level 2 background -> level 3 background
        if(level == 2) return currentImage[0];//level 3 background -> level 1 background
        return null;
    }
}
