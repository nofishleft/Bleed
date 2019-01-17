using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
public class BleedWindow : EditorWindow
{
    [MenuItem ("Tools/Bleed")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(BleedWindow));
    }

    private Texture2D IN_TEX;
    private WIDTH sel_width;
    private HEIGHT sel_height;
    private string path = "out.png";

    private int bleed = 0;
    //Dimensions of each sprite
    private int p_sWidth = -1;
    private int p_sHeight = -1;
    
    //Number of sprites in each dimension
    private int p_sXNum = -1;
    private int p_sYNum = -1;
    
    [System.Serializable]
    private enum WIDTH
    {
        SpecifySpriteWidth,
        SpecifyHorizontalCount
    }
    
    [System.Serializable]
    private enum HEIGHT
    {
        SpecifySpriteHeight,
        SpecifyVerticalCount
    }

    void OnGUI()
    {
        GUILayout.Label("Input", EditorStyles.boldLabel);
        IN_TEX = (Texture2D) EditorGUILayout.ObjectField("Texture", IN_TEX, typeof(Texture2D), false);
        
        if (IN_TEX != null && !IN_TEX.isReadable) EditorGUILayout.HelpBox("Texture is not readable.\nSet the following settings:\nAdvanced.Read/Write Enabled: True", MessageType.Error, true);
        
        GUILayout.Label("Sprite Settings", EditorStyles.boldLabel);
        
        string swl;
        if (sel_width == WIDTH.SpecifySpriteWidth) swl = "Sprite Width";
        else swl = "Horizontal Count";
        
        sel_width = (WIDTH)EditorGUILayout.EnumPopup(swl, sel_width);
        if (sel_width == WIDTH.SpecifySpriteWidth)
        {
            p_sWidth = EditorGUILayout.IntField(p_sWidth);
        }
        else
        {
            p_sXNum = EditorGUILayout.IntField(p_sXNum);
        }
        
        string shl;
        if (sel_height == HEIGHT.SpecifySpriteHeight) shl = "Sprite Height";
        else shl = "Vertical Count";
        
        sel_height = (HEIGHT)EditorGUILayout.EnumPopup(shl, sel_height);
        if (sel_height == HEIGHT.SpecifySpriteHeight)
        {
            p_sHeight = EditorGUILayout.IntField(p_sHeight);
        }
        else
        {
            p_sYNum = EditorGUILayout.IntField(p_sYNum);
        }
        
        
        GUILayout.Label("Bleed Settings", EditorStyles.boldLabel);
        bleed = EditorGUILayout.IntField("Bleed Edge", bleed);
        
        
        GUILayout.Label("Output", EditorStyles.boldLabel);
        path = EditorGUILayout.TextField("Path", path);

        if (GUILayout.Button("Create"))
        {
            Process();
        }

    }
    
    public void ColorMap (Color[] array, Color color)
    {
        for (int i = 0; i < array.Length; ++i) array[i] = color;
    }

    public void Process()
    {
        Texture2D IN = IN_TEX;
        int bleed_amount = bleed;
        Texture2D OUT;
        //Dimensions of texture
        int width = IN.width;
        int height = IN.height;
    
        //Dimensions of each sprite
        int sWidth = p_sWidth;
        int sHeight = p_sHeight;
    
        //Number of sprites in each dimension
        int sXNum = p_sXNum;
        int sYNum = p_sYNum;
    
        if ((sWidth <= 0 && sXNum <= 0) || (sHeight <= 0 && sYNum <= 0))
        {
            throw new InvalidDataException();
        }
    
        
        if (sel_width == WIDTH.SpecifySpriteWidth && sWidth > 0)
        {
            sXNum = width / sWidth;
        }
        else if (sel_width == WIDTH.SpecifyHorizontalCount && sXNum > 0)
        {
            sWidth = width / sXNum;
        }
        else
        {
            throw new InvalidDataException();
        }

        
        if (sel_height == HEIGHT.SpecifySpriteHeight && sHeight > 0)
        {
            sYNum = height / sHeight;
        }
        else if (sel_height == HEIGHT.SpecifyVerticalCount && sYNum > 0)
        {
            sHeight = height / sYNum;
        }
        else
        {
            throw new InvalidDataException();
        }
        
        //Output texture dimensions
        int OUT_WIDTH = (sWidth + 2*bleed_amount) * sXNum;
        int OUT_HEIGHT = (sHeight + 2*bleed_amount) * sYNum;
            
        OUT = new Texture2D(OUT_WIDTH, OUT_HEIGHT, IN.format, false);
        
        
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
        

        byte[] bytes = OUT.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + path, bytes);
    }


}
