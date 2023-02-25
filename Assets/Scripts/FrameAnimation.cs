using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimation : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;
    public Vector4[] Frames;
    int m_counter;

    private MaterialPropertyBlock m_PropertyBlock;


    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_PropertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    public void FrameChanged()
    {
        m_PropertyBlock.SetVector("_AtlasTiles", Frames[m_counter]);
        m_spriteRenderer.SetPropertyBlock(m_PropertyBlock);

        m_counter = (m_counter + 1) % Frames.Length;
    }
}
