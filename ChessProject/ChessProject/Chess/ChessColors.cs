using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ChessColors : MonoBehaviour
{
    public List<RawImage> WhiteSquares = new List<RawImage>();
    public Color Color1;
    [Space]
    public List<RawImage> BlackSquares = new List<RawImage>();
    public Color Color2;

    private void Update()
    {
        if (!GetComponent<ChessBoard>().playing)
        {
            foreach (RawImage square in WhiteSquares)
            {
                square.color = Color1;
            }

            foreach (RawImage square in BlackSquares)
            {
                square.color = Color2;
            }
        }
    }
}
