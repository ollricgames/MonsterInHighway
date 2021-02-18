Shader "Clevchamps/Car/Glass Emissive"
{
  Properties
  {
    _Color0 ("Tint Color", Color) = (0,0,0,1)
    _Emission ("Emission", Range(0, 1)) = 0.25
    _MainTex ("Texture", 2D) = "white" {}
    _Cube ("Reflection Cube Map", Cube) = "" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 700
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 700
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color0;
      uniform float _Emission;
      uniform sampler2D _MainTex;
      uniform samplerCUBE _Cube;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float u_xlat9;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat1.xyz = float3((in_v.normal.yyy * conv_mxt4x4_1(unity_WorldToObject).xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_0(unity_WorldToObject).xyz * in_v.normal.xxx) + u_xlat1.xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_2(unity_WorldToObject).xyz * in_v.normal.zzz) + u_xlat1.xyz));
          u_xlat9 = dot((-u_xlat0.xyz), u_xlat1.xyz);
          u_xlat9 = (u_xlat9 + u_xlat9);
          out_v.texcoord.xyz = float3(((u_xlat1.xyz * (-float3(u_xlat9, u_xlat9, u_xlat9))) + (-u_xlat0.xyz)));
          out_v.texcoord1.xy = float2(in_v.texcoord.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float3 u_xlat16_0;
      float3 u_xlat10_0;
      float4 u_xlat10_1;
      float u_xlat16_2;
      float u_xlat9_d;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = float3(texCUBE(_Cube, in_f.texcoord.xyz).xyz);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord1.xy);
          u_xlat16_0.xyz = float3(((u_xlat10_0.xyz * float3(0.100000001, 0.100000001, 0.100000001)) + u_xlat10_1.xyz));
          u_xlat16_2 = (u_xlat10_1.w + u_xlat10_1.w);
          u_xlat9_d = (u_xlat16_2 * _Emission);
          u_xlat0_d.xyz = float3(((float3(u_xlat9_d, u_xlat9_d, u_xlat9_d) * _Color0.xyz) + u_xlat16_0.xyz));
          u_xlat0_d.w = _Color0.w;
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 500
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 500
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color0;
      uniform float _Emission;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = float2(in_v.texcoord.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float u_xlat16_1;
      float u_xlat6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_1 = (u_xlat10_0.w + u_xlat10_0.w);
          u_xlat6 = (u_xlat16_1 * _Emission);
          u_xlat0_d.xyz = float3(((float3(u_xlat6, u_xlat6, u_xlat6) * _Color0.xyz) + u_xlat10_0.xyz));
          out_f.color.xyz = float3(u_xlat0_d.xyz);
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
