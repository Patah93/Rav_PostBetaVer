Shader "Custom/TransparentShadowCaster" {

Properties 
{ 

	//_Name ( "Displayed Name", type ) = default value [{options}]

 	// Vardags saker
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
 
	// Bumpligheter
	_BumpMap ("Normalmap", 2D) = "bump" {}
	//_Parallax ("Height", Range (0.005, 0.08)) = 0.02
	//_ParallaxMap ("Heightmap (A)", 2D) = "black" {}
 
	// Skuggligheter
	_Cutoff ("Shadow on/off", Range(0.8,0.9)) = 1.0
} 
 
 
SubShader 
{ 
	Tags {
	"Queue"="Transparent" 
	"IgnoreProjector"="True" 
	"RenderType"="Transparent"}
 
	LOD 300

	Pass
        { 
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
 
			Fog {Mode Off}
			ZWrite On ZTest Less Cull Off
			Offset 1, 1
 
			CGPROGRAM
			#pragma vertex vertexPass
			#pragma fragment fragmentPass
			#pragma fragmentoption ARB_precision_hint_nicest
			//#pragma multi_compile_shadowcaster
			#include "ShadowCaster.cginc"
 
 
			v2f vertexPass( appdata_full v )
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
 
			  return o;
			}
 
			float4 fragmentPass( v2f i ) : COLOR
			{
				fixed4 texcol = tex2D( _MainTex, i.uv );
				clip( texcol.a - _Cutoff );
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
        } 
 
 
CGPROGRAM
		#pragma surface surfacePass BlinnPhong alpha vertex:vertexPass fullforwardshadows approxview
		#include "ShadowCaster.cginc"
 
 
		half _Shininess;
 
		sampler2D _BumpMap;
		//sampler2D _ParallaxMap;
		float _Parallax;
 
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			//float3 viewDir;
		};
 
		v2f vertexPass (inout appdata_full v) { 
			v2f o;
			return o; 
		} 
 
		void surfacePass (Input IN, inout SurfaceOutput o) {
			//Felaktig Parallaxning
			//*half h = tex2D (_ParallaxMap, IN.uv_BumpMap).w;
			//float2 offset = ParallaxOffset (h, _Parallax, IN.viewDir);
			//IN.uv_MainTex += offset;
			//IN.uv_BumpMap += offset;*/
 
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a * _Color.a;
			//clip(o.Alpha - _Cutoff);
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
ENDCG
}
 
Fallback "Transparent/VertexLit"
}
