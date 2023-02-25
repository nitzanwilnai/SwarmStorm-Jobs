Shader "Unlit/Atlas Shader"
{
	// these are variables you can pass to the shader from the inspector or from a script
    Properties
    {
        [PerRendererData]
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
 		_ColorTint ("Color Tint", Color) = (1,1,1)
		_AtlasTiles("Atlas Index & Size", Vector) = (0, 0, 0, 0) // information about the atlas, the pattern tile (x,y), and the num tiles wide/high for the atlas
    }

    // actual shader code begins here
    SubShader
    {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}

        Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members uv2_MainTex2)
			#pragma exclude_renderers d3d11
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

             #include "UnityCG.cginc"

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float4 vertex : SV_POSITION; // clip space position
                half2 texCoord : TEXCOORD0; // texture coordinate
            };


            sampler2D _MainTex;
            fixed3 _ColorTint;
            fixed4 _AtlasTiles; // (x,y) is which tile we want (0,0 is top left), (z,w) is how many tiles wide and high the atlas is

            // varibles filled by Unity automagically for each texture
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            // vertex shader
            v2f vert (appdata v)
            {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texCoord = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
                 return o;
            }

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f pix) : SV_Target
            {
            	// we need to figure out our tile location
	            float tilesWide = _AtlasTiles.z;
	            float tilesHigh = _AtlasTiles.w;

            	float2 patternCoord = pix.texCoord;

                patternCoord.x = (_AtlasTiles.x / tilesWide) + pix.texCoord.x / tilesWide;
                patternCoord.y = (_AtlasTiles.y / tilesHigh) + pix.texCoord.y / tilesHigh;

	            fixed4 pattern = tex2D(_MainTex, patternCoord);

                fixed4 newColor;
                newColor.rgb = _ColorTint * pattern;
                newColor.a = pattern.a;

                return newColor;
            }
            ENDCG
        }

    }
}
