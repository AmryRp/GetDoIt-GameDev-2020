Shader "Unlit/WaterfallShader"
{
	Properties
	{
		_NoiseTex("Noise texture", 2D) = "white" {}
		_DisplGuide("Displacement guide", 2D) = "white" {}
		_DisplAmount("Displacement amount", float) = 0
		[HDR]_ColorBottomDark("Color bottom dark", color) = (1,1,1,1)
		[HDR]_ColorTopDark("Color top dark", color) = (1,1,1,1)
		[HDR]_ColorBottomLight("Color bottom light", color) = (1,1,1,1)
		[HDR]_ColorTopLight("Color top light", color) = (1,1,1,1)
		_BottomFoamThreshold("Bottom foam threshold", Range(0,1)) = 0.1

			_WaveScale("Wave scale", Range(0.02,0.15)) = 0.063
			_RefrColor("Refraction color", COLOR) = (.34, .85, .92, 1)
			_ReflDistort("Reflection distort", Range(0,1.5)) = 0.44
		[NoScaleOffset] _Fresnel("Fresnel (A) ", 2D) = "gray" {}
	[NoScaleOffset] _BumpMap("Normalmap ", 2D) = "bump" {}
	WaveSpeed("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
	[NoScaleOffset] _ReflectiveColor("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
	_HorizonColor("Simple water horizon color", COLOR) = (.172, .463, .435, 1)
	[HideInInspector] _ReflectionTex("Internal Reflection", 2D) = "" {}
	[HideInInspector] _RefractionTex("Internal Refraction", 2D) = "" {}
	}
		SubShader
	{
		Tags { "WaterMode" = "Refractive" "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			uniform float _RefrDistort;

	uniform float _ReflDistort;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 bumpuv0 : TEXCOORD2;
				float2 bumpuv1 : TEXCOORD3;
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 noiseUV : TEXCOORD5;
				float2 displUV : TEXCOORD6;
				float4 ref : TEXCOORD1;
				float3 viewDir : TEXCOORD4;
				UNITY_FOG_COORDS(3)
			};

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			sampler2D _DisplGuide;
			float4 _DisplGuide_ST;
			fixed4 _ColorBottomDark;
			fixed4 _ColorTopDark;
			fixed4 _ColorBottomLight;
			fixed4 _ColorTopLight;
			half _DisplAmount;
			half _BottomFoamThreshold;

			v2f vert(appdata v)
			{
			
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.displUV = TRANSFORM_TEX(v.uv, _DisplGuide);
				o.uv = v.uv;

				float4 temp;
				o.bumpuv0 = temp.xy;
				o.bumpuv1 = temp.wz;
				o.ref = ComputeNonStereoScreenPos(o.vertex);
				o.viewDir.xzy = WorldSpaceViewDir(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			sampler2D _Fresnel;
			sampler2D _ReflectionTex;
			uniform float4 _RefrColor;
			sampler2D _RefractionTex;
			sampler2D _BumpMap;
			half4 frag(v2f i) : SV_Target
			{	
				i.viewDir = normalize(i.viewDir);
				half3 bump1 = UnpackNormal(tex2D(_BumpMap, i.bumpuv0)).rgb;
				half3 bump2 = UnpackNormal(tex2D(_BumpMap, i.bumpuv1)).rgb;
				half3 bump = (bump1 + bump2) * 0.5;
				
				half fresnelFac = dot(i.viewDir, bump);
				half4 color;
				float4 uv1 = i.ref; uv1.xyz += bump * _ReflDistort;
				half4 refl = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(uv1));
				float4 uv2 = i.ref; uv2.xyz -=  _RefrDistort;
				half4 refr = tex2Dproj(_RefractionTex, UNITY_PROJ_COORD(uv2)) * _RefrColor;

				half fresnel = UNITY_SAMPLE_1CHANNEL(_Fresnel, float2(fresnelFac, fresnelFac));
				color = lerp(refr, refl, fresnel);
				//Displacement
				half2 displ = tex2D(_DisplGuide, i.displUV + _Time.y / 4).xy;
				displ = ((displ * 2) - 1) * _DisplAmount;

				//Noise
				half noise = tex2D(_NoiseTex, float2(i.noiseUV.x, i.noiseUV.y + _Time.y / 4) + displ).x;
				noise = round(noise * 5.0) / 5.0;

				fixed4 col = lerp(lerp(color, color, i.uv.y), lerp(color, color, i.uv.y), noise);
				col = lerp(fixed4(1,1,1,1), col, step(_BottomFoamThreshold, i.uv.y + displ.y));
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
		Fallback "VertexLit"
}