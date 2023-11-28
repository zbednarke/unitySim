Shader "Custom/CircleShader"
{
    Properties
    {
        _MainColor ("Circle Color", Color) = (0,0,1,1)
        _Center ("Center", Vector) = (0.5,0.5,0,0)
        _Radius ("Radius", float) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainColor;
            float2 _Center;
            float _Radius;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center);
                if(dist <= _Radius)
                    return _MainColor;
                else
                    return half4(0, 0, 0, 0); // Transparent
            }
            ENDCG
        }
    }
}
