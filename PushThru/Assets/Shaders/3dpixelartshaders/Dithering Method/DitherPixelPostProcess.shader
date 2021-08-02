Shader "Unlit/DitherPixelPostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _Pixelize("pixelize", Float) = 0
        _PixelSize ("pixel Size",float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout"
        "PreviewType" = "Plane"
        "RenderPipeline" = "UniversalPipeline"}

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Off
            Blend Off
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
            sampler2D _CameraDepthTexture;
            float _Pixelize;
            float _PixelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                
                float2 resolution = float2(_ScreenParams.x,_ScreenParams.y);
                float2 resolutionInverse = 1/resolution;
                float2 pixelPosition = resolution * i.uv;
                float minDepth = 10000;
                float4 col = tex2D(_MainTex, i.uv);
                if(!(_Pixelize == 1))
                    return col;

                //Sampling
                int lower = -floor((_PixelSize-1)/2);
                int upper = floor(_PixelSize/2);
                for(int x = lower;x <= upper ;x++)
                {
                    for(int y = lower;y <= upper;y++)
                    {
                        float2 samplePixelPosition = pixelPosition + float2(x,y);
                        float2 sampleUVPosition = samplePixelPosition * resolutionInverse;
                        float4 sampledColor = tex2D(_MainTex,sampleUVPosition);
                        //if(sampledColor.a > 0.1)
                        {
                            float raw_depth = tex2D(_CameraDepthTexture, sampleUVPosition).r;
                            float depth = LinearEyeDepth(raw_depth);
                            float yOffset = sampleUVPosition.y*0.008f;
                            depth -= yOffset;
                            if(depth < minDepth)
                            {
                                minDepth = depth;
                                col = sampledColor;
                            }   
                        }
                        
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
