Shader "Clevchamps/Car/Glass IBL"
{
  Properties
  {
    _Color0 ("Tint Color", Color) = (0,0,0,1)
    [NoScaleOffset] _IBL ("IBL + Ref on Alpha", Cube) = "" {}
    _ReflStrenght ("Reflection Strenght", Range(0, 2)) = 0
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
      uniform float _ReflStrenght;
      uniform samplerCUBE _IBL;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.xyz = float3((in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat6 = dot((-u_xlat0.xyz), u_xlat1.xyz);
          u_xlat6 = (u_xlat6 + u_xlat6);
          out_v.texcoord.xyz = float3(((u_xlat1.xyz * (-float3(u_xlat6, u_xlat6, u_xlat6))) + (-u_xlat0.xyz)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = texCUBE(_IBL, in_f.texcoord.xyz);
          u_xlat16_1.x = (u_xlat10_0.w + 0.5);
          u_xlat16_1.xyz = float3((u_xlat10_0.xyz * u_xlat16_1.xxx));
          u_xlat0_d.xyz = float3(((u_xlat16_1.xyz * float3(_ReflStrenght, _ReflStrenght, _ReflStrenght)) + _Color0.xyz));
          out_f.color.xyz = float3(u_xlat0_d.xyz);
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "LIGHTMODE" = "FORWARDBASE"
      "QUEUE" = "Geometry"
      "RenderType" = "Opaque"
    }
    LOD 500
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry"
        "RenderType" = "Opaque"
      }
      LOD 500
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
      uniform float _ReflStrenght;
      uniform samplerCUBE _IBL;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float u_xlat9;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.xyz = float3((in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat1.xyz = float3(in_v.normal.xyz);
          u_xlat1.w = 1;
          u_xlat2.x = dot(u_xlat1, conv_mxt4x4_0(unity_WorldToObject));
          u_xlat2.y = dot(u_xlat1, conv_mxt4x4_1(unity_WorldToObject));
          u_xlat2.z = dot(u_xlat1, conv_mxt4x4_2(unity_WorldToObject));
          u_xlat9 = dot((-u_xlat0.xyz), u_xlat2.xyz);
          u_xlat9 = (u_xlat9 + u_xlat9);
          out_v.texcoord.xyz = float3(((u_xlat2.xyz * (-float3(u_xlat9, u_xlat9, u_xlat9))) + (-u_xlat0.xyz)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = texCUBE(_IBL, in_f.texcoord.xyz);
          u_xlat16_1.x = (u_xlat10_0.w + 0.5);
          u_xlat16_1.xyz = float3((u_xlat10_0.xyz * u_xlat16_1.xxx));
          u_xlat0_d.xyz = float3(((u_xlat16_1.xyz * float3(_ReflStrenght, _ReflStrenght, _ReflStrenght)) + _Color0.xyz));
          out_f.color.xyz = float3(u_xlat0_d.xyz);
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "LIGHTMODE" = "FORWARDBASE"
      "QUEUE" = "Geometry"
      "RenderType" = "Opaque"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry"
        "RenderType" = "Opaque"
      }
      LOD 100
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
      struct appdata_t
      {
          float4 vertex :POSITION0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 vertex :Position;
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
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = _Color0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
