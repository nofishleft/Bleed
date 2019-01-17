using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Bleed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Texture2D IN;
    private Texture2D OUT;
    
    public int bleed_amount;

    //Dimensions of texture
    public int width { get; set; }
    public int height { get; set; }

    //Dimensions of each sprite
    public int sWidth = -1;
    public int sHeight = -1;

    //Number of sprites in each dimension
    public int sXNum = -1;
    public int sYNum = -1;

    //Output texture dimensions
    public int OUT_WIDTH { get; set; }
    public int OUT_HEIGHT { get; set; }

    [MenuItem("Tools/Bleed/Generate Bleed %g")]
    static void DoSomethingWithAShortcutKey()
    {
        
    }

    public struct Corner
    {
        public (int x, int y) a;
        public (int x, int y) b;
        public Color c;

        public Corner((int x, int y) a, (int x, int y) b, Color c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }

    public void ColorMap (Color[] array, Color color)
    {
        for (int i = 0; i < array.Length; ++i) array[i] = color;
    }

    public void Process()
    {
        Define();
        
        
        for (int xSprite = 0; xSprite < sXNum; ++xSprite) {
            for (int ySprite = 0; ySprite < sYNum; ++ySprite)
            {
                //Copy sprite over
                int x = (2 * xSprite + 1) * bleed_amount + (xSprite * sWidth);
                int y = (2 * ySprite + 1) * bleed_amount + (ySprite * sHeight);
                OUT.SetPixels(x,y,sWidth,sHeight,IN.GetPixels(xSprite * sWidth, ySprite * sHeight, sWidth, sHeight));
                
                
                //Create Bleed Corners
                Color[] array = new Color[bleed_amount * bleed_amount];

                ColorMap(array, OUT.GetPixel(x,y));
                OUT.SetPixels(x - bleed_amount, y - bleed_amount, bleed_amount, bleed_amount, array);
                
                ColorMap(array, OUT.GetPixel(x + sWidth - 1,y));
                OUT.SetPixels(x + sWidth, y - bleed_amount, bleed_amount, bleed_amount, array);
                
                ColorMap(array, OUT.GetPixel(x,y + sHeight - 1));
                OUT.SetPixels(x - bleed_amount, y + sHeight, bleed_amount, bleed_amount, array);
                
                ColorMap(array, OUT.GetPixel(x + sWidth - 1,y + sHeight - 1));
                OUT.SetPixels(x + sWidth, y + sHeight, bleed_amount, bleed_amount, array);

                
                //Create Bleed Lines              
                //Top
                array = new Color[sWidth];
                for (int i = 0; i < sWidth; ++i)
                    array[i] = OUT.GetPixel(x + i, y);
                for (int i = 1; i <= bleed_amount; ++i)
                    OUT.SetPixels(x,y - i,sWidth,1,array);
                
                //Bot
                for (int i = 0; i < sWidth; ++i)
                    array[i] = OUT.GetPixel(x + i, y + sHeight - 1);
                for (int i = 0; i < bleed_amount; ++i)
                    OUT.SetPixels(x,y + i + sHeight,sWidth,1,array);
                
                //Left
                array = new Color[sHeight];
                for (int i = 0; i < sWidth; ++i)
                    array[i] = OUT.GetPixel(x, y + i);
                for (int i = 1; i <= bleed_amount; ++i)
                    OUT.SetPixels(x - i,y,1,sHeight,array);
                
                //Right
                array = new Color[sHeight];
                for (int i = 0; i < sWidth; ++i)
                    array[i] = OUT.GetPixel(x + sWidth - 1, y + i);
                for (int i = 0; i < bleed_amount; ++i)
                    OUT.SetPixels(x + i + sWidth,y,1,sHeight,array);
                
                
                OUT.Apply();
            }
        }
        

        Save();
    }

    public void Save()
    {
        //AssetDatabase.CreateAsset(OUT, "Assets/out.png");
        byte[] bytes = OUT.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/out.png", bytes);
    }

    public void Define()
    {
        width = IN.width;
        height = IN.height;

        if ((sWidth <= 0 && sXNum <= 0) || (sHeight <= 0 && sYNum <= 0))
        {
            throw new InvalidDataException();
        }

        if (sWidth <= 0)
        {
            sWidth = width / sXNum;
        }
        else
        {
            sXNum = width / sWidth;
        }
        
        if (sHeight <= 0)
        {
            sHeight = height / sYNum;
        }
        else
        {
            sYNum = height / sHeight;
        }
        
        OUT_WIDTH = (sWidth + 2*bleed_amount) * sXNum;
        OUT_HEIGHT = (sHeight + 2*bleed_amount) * sYNum;
        
        OUT = new Texture2D(OUT_WIDTH, OUT_HEIGHT, IN.format, false);
    }

}
