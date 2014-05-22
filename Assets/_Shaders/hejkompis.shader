Shader "Unity Answers/Bumped Specular with own map" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_SpecMap ("Specular map", 2D) = "black" {}
	_GrainTex ("Grain Textue", 2D) = "black" {}
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0


sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _SpecMap;
sampler2D _GrainTex;
fixed4 _Color;
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float2 uv_SpecMap;
	float2 uv_GrainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 specTex = tex2D(_SpecMap, IN.uv_SpecMap);
	fixed4 grainTex = tex2D(_GrainTex,IN.uv_GrainTex);
	
	fixed4 grainFinal = 0;
	
	if((grainTex.r + grainTex.g + grainTex.b) > 2.0) {
		grainFinal.r = 1;
		grainFinal.g = 1;
		grainFinal.b = 1;
	}else if((grainTex.r + grainTex.g + grainTex.b) > 1.5) {
		grainFinal.r = 0.2;
		grainFinal.g = 0.2;
		grainFinal.b = 0.2;
	}
	
	o.Albedo = tex.rgb * _Color.rgb - grainFinal;
	o.Gloss = specTex.r;
	o.Alpha = tex.a * _Color.a;
	o.Specular = _Shininess * specTex.g;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Specular"
}