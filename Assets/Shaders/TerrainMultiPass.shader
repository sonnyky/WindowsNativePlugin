// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "TerrainMultiPass" {
    Properties {
        _BottomPosition ("BottomPosition", Float ) = -0.5
        _UpperPosition ("UpperPosition", Float ) = 0.5
        _T1b ("T1b", Range(0, 1)) = 0.05
        _T2a ("T2a", Range(0, 1)) = 0.1
        _T2b ("T2b", Range(0, 1)) = 0.15
        _T3a ("T3a", Range(0, 1)) = 0.2
        _T3b ("T3b", Range(0, 1)) = 0.25
        _T4a ("T4a", Range(0, 1)) = 0.3
        _T4b ("T4b", Range(0, 1)) = 0.35
        _T5a ("T5a", Range(0, 1)) = 0.4
        _T5b ("T5b", Range(0, 1)) = 0.45
        _T6a ("T6a", Range(0, 1)) = 0.5
        _T6b ("T6b", Range(0, 1)) = 0.55
        _T7a ("T7a", Range(0, 1)) = 0.6
        _T7b ("T7B", Range(0, 1)) = 0.65
        _T8a ("T8a", Range(0, 1)) = 0.7690078

        _AreaChannel ("AreaChannel", 2D) = "white" {}
        _MultiBlendNoise ("MultiBlendNoise", 2D) = "black" {}

        _Texture1 ("Texture1", 2D) = "white" {}
        _Texture1Hue ("Texture1Hue", Range(0, 1)) = 0
        _Texture1Sat ("Texture1Sat", Range(-1, 1)) = 0
        _Texture1Val ("Texture1Val", Range(-1, 1)) = 0

        _Texture2Hue ("Texture2Hue", Range(0, 1)) = 0
        _Texture2Val ("Texture2Val", Range(-1, 1)) = 0
        _Texture2Sat ("Texture2Sat", Range(-1, 1)) = 0.7179487

        _Texture3 ("Texture3", 2D) = "white" {}
        _Texture3Hue ("Texture3Hue", Range(0, 1)) = 0
        _Texture3Sat ("Texture3Sat", Range(-1, 1)) = 0
        _Texture3Val ("Texture3Val", Range(-1, 1)) = 0

        _Wave ("Wave", 2D) = "white" {}
        _WaveNoise ("WaveNoise", 2D) = "black" {}
        _WaveSpeed ("WaveSpeed", Range(0, 1)) = 0.005
        _WavePower1 ("WavePower1", Range(0, 10)) = 0.11
        _WaterPower2 ("WaterPower2", Range(-10, 10)) = 1.54
        _WaveFloor ("WaveFloor", Range(0, 10)) = 10
        _WaveColor ("WaveColor", Color) = (0.5,0.5,0.5,1)
        _WaterAlpha ("WaterAlpha", Range(0, 1)) = 1

        _Texture4a ("Texture4a", 2D) = "white" {}
        _Texture4b ("Texture4b", 2D) = "white" {}
        _Texture4c ("Texture4c", 2D) = "white" {}
        _Texture4d ("Texture4d", 2D) = "white" {}
        _Texture4Hue ("Texture4Hue", Range(0, 1)) = 0
        _Texture4Sat ("Texture4Sat", Range(-1, 1)) = 0
        _Texture4Val ("Texture4Val", Range(-1, 1)) = 0

        _Texture5a ("Texture5a", 2D) = "white" {}
        _Texture5b ("Texture5b", 2D) = "white" {}
        _Texture5c ("Texture5c", 2D) = "white" {}
        _Texture5d ("Texture5d", 2D) = "white" {}
        _Texture5Hue ("Texture5Hue", Range(0, 1)) = 0
        _Texture5Sat ("Texture5Sat", Range(-1, 1)) = 0
        _Texture5Val ("Texture5Val", Range(-1, 1)) = 0

        _Texture6a ("Texture6a", 2D) = "white" {}
        _Texture6b ("Texture6b", 2D) = "white" {}
        _Texture6c ("Texture6c", 2D) = "white" {}
        _Texture6d ("Texture6d", 2D) = "white" {}
        _Texture6Hue ("Texture6Hue", Range(0, 1)) = 0
        _Texture6Sat ("Texture6Sat", Range(-1, 1)) = 0
        _Texture6Val ("Texture6Val", Range(-1, 1)) = 0

        _Texture7a ("Texture7a", 2D) = "white" {}
        _Texture7b ("Texture7b", 2D) = "white" {}
        _Texture7c ("Texture7c", 2D) = "white" {}
        _Texture7d ("Texture7d", 2D) = "white" {}
        _Texture7Hue ("Texture7Hue", Range(0, 1)) = 0
        _Texture7Sat ("Texture7Sat", Range(-1, 1)) = 0
        _Texture7Val ("Texture7Val", Range(-1, 1)) = 0

        _Texture8a ("Texture8a", 2D) = "white" {}
        _Texture8b ("Texture8b", 2D) = "white" {}
        _Texture8c ("Texture8c", 2D) = "white" {}
        _Texture8d ("Texture8d", 2D) = "white" {}
        _Texture8Hue ("Texture8Hue", Range(0, 1)) = 0
        _Texture8Sat ("Texture8Sat", Range(-1, 1)) = 0
        _Texture8Val ("Texture8Val", Range(-1, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

CGINCLUDE
            #include "UnityCG.cginc"

            float RangeData(float data, float minInput, float maxInput){
                return min(max(data - minInput, 0) / (maxInput - minInput), 1);
            }

            fixed3 RangeData2( fixed3 color1 , fixed3 color2 , float range1 , float range2 , float range3 , float data , float scale ){
                fixed3 color3 = lerp(color1,color2,RangeData(data,range1*scale,range2*scale));
                return lerp(color3,color2,RangeData(data,range2*scale,range3*scale));
            }

            fixed RangeData3( fixed color1 , fixed color2 , float range1 , float range2 , float data , float scale ){
                 return lerp(color1,color2,RangeData(data,range1*scale,range2*scale));
            }

            fixed RangeData4( fixed color1 , fixed color2 , float range1 , float range2 , float range3 , float data , float scale ){
                fixed3 color3 = lerp(color1,color2,RangeData(data,range1*scale,range2*scale));
                return lerp(color3,color2,RangeData(data,range2*scale,range3*scale));
            }

            float3 ChangeColor(float3 c, float hue, float sat, float val){
                float4 _p = lerp(float4(float4(c,0.0).zy, float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0).wz), float4(float4(c,0.0).yz, float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0).xy), step(float4(c,0.0).z, float4(c,0.0).y));
                float4 _q = lerp(float4(_p.xyw, float4(c,0.0).x), float4(float4(c,0.0).x, _p.yzx), step(_p.x, float4(c,0.0).x));
                float3 _r = float3(abs(_q.z + (_q.w - _q.y) / (6.0 * (_q.x - min(_q.w, _q.y)) + (1.0e-10))), (_q.x - min(_q.w, _q.y)) / (_q.x + (1.0e-10)), _q.x);
                return lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac((_r.r+hue)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),(_r.g+sat))*(_r.b+val);
            }

            uniform float _BottomPosition;
            uniform float _UpperPosition;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
ENDCG

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            uniform float _T1b;
            uniform float _T2a;
            uniform float _T2b;
            uniform float _T3a;
            uniform float _T3b;
            uniform float _T4a;

            uniform sampler2D _Texture1; uniform float4 _Texture1_ST;
            uniform sampler2D _Texture3; uniform float4 _Texture3_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform sampler2D _WaveNoise; uniform float4 _WaveNoise_ST;
            uniform sampler2D _Wave; uniform float4 _Wave_ST;

            uniform float _Texture1Hue;
            uniform float _Texture1Sat;
            uniform float _Texture1Val;
            uniform float _Texture2Hue;
            uniform float _Texture2Sat;
            uniform float _Texture2Val;
            uniform float _Texture3Hue;
            uniform float _Texture3Sat;
            uniform float _Texture3Val;

            uniform float _WaveSpeed;
            uniform float _WavePower1;
            uniform float _WaterPower2;
            uniform float _WaveFloor;
            uniform float4 _WaveColor;
            uniform float  _WaterAlpha;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 texture1 = tex2D(_Texture1,TRANSFORM_TEX(i.uv0, _Texture1));
                float waveNoise = (tex2D(_WaveNoise,TRANSFORM_TEX(i.uv0, _WaveNoise)).r*0.33);
                float2 uv1 = (i.uv0+(float2(0.5,0.5)*fmod(((_Time.g*_WaveSpeed)+waveNoise+0.5),1.0)*0.5));
                float2 uv2 = (i.uv0+(float2(0.5,0.5)*fmod(((_Time.g*_WaveSpeed)+waveNoise),1.0)*0.5));
                float4 tex1 = tex2D(_Wave,TRANSFORM_TEX(uv1, _Wave));
                float4 tex2 = tex2D(_Wave,TRANSFORM_TEX(uv2, _Wave));
                float3 wave1 = saturate((pow(lerp(tex1.rgb,tex2.rgb,_WaterPower2),_WaveFloor)*_WavePower1))*_WaveColor.rgb*2.0;
                float3 wave2 = saturate((texture1.rgb+wave1));
                float4 texture3 = tex2D(_Texture3,TRANSFORM_TEX(i.uv0, _Texture3));
                float scale = (_UpperPosition-_BottomPosition);
                float data = (i.posWorld.g-_BottomPosition);
                float3 toRGB1 = ChangeColor(wave2, _Texture1Hue, _Texture1Sat, _Texture1Val);
                float3 toRGB2 = ChangeColor(wave2, _Texture2Hue, _Texture2Sat, _Texture2Val);
                float3 toRGB3 = ChangeColor(texture3, _Texture3Hue, _Texture3Sat, _Texture3Val);
                float3 finalColor = RangeData2( RangeData2( RangeData2( toRGB1 , toRGB1 , 0.0 , 0.0 , _T1b , scale , data ), saturate(toRGB2+wave1) , _T1b , _T2a , _T2b , data , scale ) ,   toRGB3  , _T2b , _T3a , _T3b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor,lerp(0,1,areaChannel.r)*_WaterAlpha);
            }
            ENDCG
        }

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _T3b;
            uniform float _T4a;
            uniform float _T4b;
            uniform float _T5a;

            uniform sampler2D _Texture4a; uniform float4 _Texture4a_ST;
            uniform sampler2D _Texture4b; uniform float4 _Texture4b_ST;
            uniform sampler2D _Texture4c; uniform float4 _Texture4c_ST;
            uniform sampler2D _Texture4d; uniform float4 _Texture4d_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform float _Texture4Hue;
            uniform float _Texture4Sat;
            uniform float _Texture4Val;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 multiBlendNoise = tex2D(_MultiBlendNoise,TRANSFORM_TEX(i.uv0, _MultiBlendNoise));
                float4 texture4d = tex2D(_Texture4d,TRANSFORM_TEX(i.uv0, _Texture4d));
                float4 texture4a = tex2D(_Texture4a,TRANSFORM_TEX(i.uv0, _Texture4a));
                float4 texture4b = tex2D(_Texture4b,TRANSFORM_TEX(i.uv0, _Texture4b));
                float4 texture4c = tex2D(_Texture4c,TRANSFORM_TEX(i.uv0, _Texture4c));
                float3 channelBlend = (lerp( lerp( lerp( texture4d.rgb, texture4a.rgb, multiBlendNoise.rgb.r ), texture4b.rgb, multiBlendNoise.rgb.g ), texture4c.rgb, multiBlendNoise.rgb.b ));
                channelBlend = ChangeColor(channelBlend, _Texture4Hue, _Texture4Sat, _Texture4Val);
                float data = (i.posWorld.g-_BottomPosition);
                float scale = (_UpperPosition-_BottomPosition);
                float3 finalColor = RangeData2( channelBlend , channelBlend , _T3b , _T4a , _T4b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor, RangeData4( 0.0 , lerp(0,1,areaChannel.r) , _T3b , _T4a , _T4b , data , scale ) );
            }
            ENDCG
        }

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _T4b;
            uniform float _T5a;
            uniform float _T5b;
            uniform float _T6a;

            uniform sampler2D _Texture5a; uniform float4 _Texture5a_ST;
            uniform sampler2D _Texture5b; uniform float4 _Texture5b_ST;
            uniform sampler2D _Texture5c; uniform float4 _Texture5c_ST;
            uniform sampler2D _Texture5d; uniform float4 _Texture5d_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform float _Texture5Hue;
            uniform float _Texture5Sat;
            uniform float _Texture5Val;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 multiBlendNoise = tex2D(_MultiBlendNoise,TRANSFORM_TEX(i.uv0, _MultiBlendNoise));
                float4 texture5d = tex2D(_Texture5d,TRANSFORM_TEX(i.uv0, _Texture5d));
                float4 texture5a = tex2D(_Texture5a,TRANSFORM_TEX(i.uv0, _Texture5a));
                float4 texture5b = tex2D(_Texture5b,TRANSFORM_TEX(i.uv0, _Texture5b));
                float4 texture5c = tex2D(_Texture5c,TRANSFORM_TEX(i.uv0, _Texture5c));
                float3 channelBlend = (lerp( lerp( lerp( texture5d.rgb, texture5a.rgb, multiBlendNoise.rgb.r ), texture5b.rgb, multiBlendNoise.rgb.g ), texture5c.rgb, multiBlendNoise.rgb.b ));
                channelBlend = ChangeColor(channelBlend, _Texture5Hue, _Texture5Sat, _Texture5Val);
                float data = (i.posWorld.g-_BottomPosition);
                float scale = (_UpperPosition-_BottomPosition);
                float3 finalColor = RangeData2( channelBlend , channelBlend , _T4b , _T5a , _T5b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor, RangeData4( 0.0 , lerp(0,1,areaChannel.r) , _T4b , _T5a , _T5b , data , scale ) );
            }
            ENDCG
        }


        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _T5b;
            uniform float _T6a;
            uniform float _T6b;
            uniform float _T7a;

            uniform sampler2D _Texture6a; uniform float4 _Texture6a_ST;
            uniform sampler2D _Texture6b; uniform float4 _Texture6b_ST;
            uniform sampler2D _Texture6c; uniform float4 _Texture6c_ST;
            uniform sampler2D _Texture6d; uniform float4 _Texture6d_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform float _Texture6Hue;
            uniform float _Texture6Sat;
            uniform float _Texture6Val;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 multiBlendNoise = tex2D(_MultiBlendNoise,TRANSFORM_TEX(i.uv0, _MultiBlendNoise));
                float4 texture6d = tex2D(_Texture6d,TRANSFORM_TEX(i.uv0, _Texture6d));
                float4 texture6a = tex2D(_Texture6a,TRANSFORM_TEX(i.uv0, _Texture6a));
                float4 texture6b = tex2D(_Texture6b,TRANSFORM_TEX(i.uv0, _Texture6b));
                float4 texture6c = tex2D(_Texture6c,TRANSFORM_TEX(i.uv0, _Texture6c));
                float3 channelBlend = (lerp( lerp( lerp( texture6d.rgb, texture6a.rgb, multiBlendNoise.rgb.r ), texture6b.rgb, multiBlendNoise.rgb.g ), texture6c.rgb, multiBlendNoise.rgb.b ));
                channelBlend = ChangeColor(channelBlend, _Texture6Hue, _Texture6Sat, _Texture6Val);
                float data = (i.posWorld.g-_BottomPosition);
                float scale = (_UpperPosition-_BottomPosition);
                float3 finalColor = RangeData2( channelBlend , channelBlend , _T5b , _T6a , _T6b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor, RangeData4( 0.0 , lerp(0,1,areaChannel.r) , _T5b , _T6a , _T6b , data , scale ) );
            }
            ENDCG
        }
 
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _T6b;
            uniform float _T7a;
            uniform float _T7b;
            uniform float _T8a;

            uniform sampler2D _Texture7a; uniform float4 _Texture7a_ST;
            uniform sampler2D _Texture7b; uniform float4 _Texture7b_ST;
            uniform sampler2D _Texture7c; uniform float4 _Texture7c_ST;
            uniform sampler2D _Texture7d; uniform float4 _Texture7d_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform float _Texture7Hue;
            uniform float _Texture7Sat;
            uniform float _Texture7Val;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 multiBlendNoise = tex2D(_MultiBlendNoise,TRANSFORM_TEX(i.uv0, _MultiBlendNoise));
                float4 texture7d = tex2D(_Texture7d,TRANSFORM_TEX(i.uv0, _Texture7d));
                float4 texture7a = tex2D(_Texture7a,TRANSFORM_TEX(i.uv0, _Texture7a));
                float4 texture7b = tex2D(_Texture7b,TRANSFORM_TEX(i.uv0, _Texture7b));
                float4 texture7c = tex2D(_Texture7c,TRANSFORM_TEX(i.uv0, _Texture7c));
                float3 channelBlend = (lerp( lerp( lerp( texture7d.rgb, texture7a.rgb, multiBlendNoise.rgb.r ), texture7b.rgb, multiBlendNoise.rgb.g ), texture7c.rgb, multiBlendNoise.rgb.b ));
                channelBlend = ChangeColor(channelBlend, _Texture7Hue, _Texture7Sat, _Texture7Val);
                float data = (i.posWorld.g-_BottomPosition);
                float scale = (_UpperPosition-_BottomPosition);
                float3 finalColor = RangeData2( channelBlend , channelBlend , _T6b , _T7a , _T7b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor, RangeData4( 0.0 , lerp(0,1,areaChannel.r) , _T6b , _T7a , _T7b , data , scale ) );
            }
            ENDCG
        }
   
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _T6b;
            uniform float _T7a;
            uniform float _T7b;
            uniform float _T8a;

            uniform sampler2D _Texture8a; uniform float4 _Texture8a_ST;
            uniform sampler2D _Texture8b; uniform float4 _Texture8b_ST;
            uniform sampler2D _Texture8c; uniform float4 _Texture8c_ST;
            uniform sampler2D _Texture8d; uniform float4 _Texture8d_ST;

            uniform sampler2D _MultiBlendNoise; uniform float4 _MultiBlendNoise_ST;
            uniform sampler2D _AreaChannel; uniform float4 _AreaChannel_ST;

            uniform float _Texture8Hue;
            uniform float _Texture8Sat;
            uniform float _Texture8Val;

            fixed4 frag(VertexOutput i) : COLOR {
                float4 multiBlendNoise = tex2D(_MultiBlendNoise,TRANSFORM_TEX(i.uv0, _MultiBlendNoise));
                float4 texture8d = tex2D(_Texture8d,TRANSFORM_TEX(i.uv0, _Texture8d));
                float4 texture8a = tex2D(_Texture8a,TRANSFORM_TEX(i.uv0, _Texture8a));
                float4 texture8b = tex2D(_Texture8b,TRANSFORM_TEX(i.uv0, _Texture8b));
                float4 texture8c = tex2D(_Texture8c,TRANSFORM_TEX(i.uv0, _Texture8c));
                float3 channelBlend = lerp( lerp( lerp( texture8d.rgb, texture8a.rgb, multiBlendNoise.rgb.r ), texture8b.rgb, multiBlendNoise.rgb.g ), texture8c.rgb, multiBlendNoise.rgb.b );
                channelBlend = ChangeColor(channelBlend, _Texture8Hue, _Texture8Sat, _Texture8Val);
                float data = (i.posWorld.g-_BottomPosition);
                float scale = (_UpperPosition-_BottomPosition);
                float3 finalColor = RangeData2( channelBlend , channelBlend , _T6b , _T7a , _T7b , data , scale );
                float4 areaChannel = tex2D(_AreaChannel,TRANSFORM_TEX(i.uv0, _AreaChannel));
                return fixed4(finalColor,RangeData3( 0.0, lerp(0,1,areaChannel.r) , _T7b , _T8a , data , scale ));
            }
            ENDCG
        }

    }
}