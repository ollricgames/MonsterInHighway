Shader "Clevchamps/Car/CarPaint"
{
  Properties
  {
    _Color0 ("Primary Color", Color) = (1,0,0,1)
    _Color1 ("Secondary Color", Color) = (0,1,0,1)
    _Color2 ("Rim Color", Color) = (0,0,1,1)
    _SpecColor ("Specular Color", Color) = (0,0,0,0)
    _DecalColor ("Decal Color", Color) = (1,0,0,1)
    [Header(X rimV      Y rimP      Z fresnel   W Layer)] _vector1 ("", Vector) = (1,3.5,1.25,0.15)
    [Header(X gi        Y spec      Z ibl       W refl)] _vector2 ("", Vector) = (2,3,1,1.5)
    [NoScaleOffset] _IBL ("IBL", Cube) = "" {}
    [NoScaleOffset] _DecalTex ("Decal Texture", 2D) = "black" {}
  }
  SubShader
  {
    Tags
    { 
      "LIGHTMODE" = "FORWARDBASE"
      "QUEUE" = "Geometry"
      "RenderType" = "Opaque"
    }
    LOD 700
    Pass // ind: 1, name: HIGH VERSION
    {
      Name "HIGH VERSION"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry"
        "RenderType" = "Opaque"
      }
      LOD 700
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
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _vector1;
      uniform float4 _vector2;
      uniform float3 _Color0;
      uniform float3 _Color1;
      uniform float3 _Color2;
      uniform float3 _DecalColor;
      uniform float3 _SpecColor;
      uniform sampler2D _DecalTex;
      uniform samplerCUBE _IBL;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float3 color :COLOR0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 color :COLOR0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float3 u_xlat3;
      float u_xlat12;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.xyz = float3((_Color0.xyz + (-_Color1.xyz)));
          u_xlat1.xyz = float3((in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat1.xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat1.xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat1.xyz));
          u_xlat1.xyz = float3(((-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat12 = rsqrt(u_xlat12);
          u_xlat2.xyz = float3(((u_xlat1.xyz * float3(u_xlat12, u_xlat12, u_xlat12)) + _WorldSpaceLightPos0.xyz));
          u_xlat1.xyz = float3((float3(u_xlat12, u_xlat12, u_xlat12) * u_xlat1.xyz));
          u_xlat3.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat3.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat3.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat3.xyz = float3(normalize(u_xlat3.xyz));
          u_xlat2.z = dot(u_xlat3.xyz, u_xlat2.xyz);
          u_xlat2.y = dot(u_xlat1.xyz, u_xlat3.xyz);
          u_xlat2.x = dot(_WorldSpaceLightPos0.xyz, u_xlat3.xyz);
          u_xlat2.xyz = float3(max(u_xlat2.xyz, float3(0, 0, 0)));
          u_xlat12 = ((-u_xlat2.z) + _vector1.w);
          u_xlat12 = (u_xlat12 * u_xlat12);
          out_v.color.xyz = float3(((float3(u_xlat12, u_xlat12, u_xlat12) * u_xlat0.xyz) + _Color1.xyz));
          u_xlat0.x = dot((-u_xlat1.xyz), u_xlat3.xyz);
          u_xlat0.x = (u_xlat0.x + u_xlat0.x);
          out_v.texcoord.xyz = float3(((u_xlat3.xyz * (-u_xlat0.xxx)) + (-u_xlat1.xyz)));
          u_xlat0.x = (u_xlat2.x + u_xlat2.x);
          u_xlat0.xyz = float3(((u_xlat0.xxx * u_xlat3.xyz) + (-_WorldSpaceLightPos0.xyz)));
          u_xlat0.x = dot(u_xlat0.xyz, u_xlat1.xyz);
          u_xlat0.x = (u_xlat0.x * u_xlat2.x);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          out_v.texcoord.w = (u_xlat0.x * _vector2.y);
          out_v.texcoord1.xy = float2(in_v.texcoord.xy);
          out_v.texcoord1.zw = in_v.texcoord1.xy;
          u_xlat0.x = (((-u_xlat2.z) * 0.5) + 1);
          u_xlat0.x = (((-u_xlat2.x) * u_xlat0.x) + _vector2.x);
          out_v.texcoord2.z = (u_xlat0.x * u_xlat0.x);
          u_xlat0.x = ((_vector1.y * u_xlat2.y) + (-_vector1.x));
          out_v.texcoord2.x = ((-u_xlat2.y) + _vector1.z);
          u_xlat0.x = (u_xlat0.x + 1);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          u_xlat0.x = min(u_xlat0.x, 1);
          out_v.texcoord2.y = ((-u_xlat0.x) + 1);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float u_xlat16_1;
      int u_xlatb1;
      float3 u_xlat2_d;
      float4 u_xlat10_2;
      float3 u_xlat4;
      float u_xlat16_4;
      float u_xlat9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_DecalTex, in_f.texcoord1.zw);
          u_xlat16_1 = (u_xlat10_0.w + (-0.5));
          u_xlat16_1 = (u_xlat16_1 * 9.99999809);
          u_xlat16_1 = clamp(u_xlat16_1, 0, 1);
          u_xlat16_4 = ((u_xlat16_1 * (-2)) + 3);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_1);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_4);
          u_xlat4.xyz = float3(((_DecalColor.xyz * u_xlat10_0.xyz) + (-u_xlat10_0.xyz)));
          u_xlat0_d.xyz = float3(((float3(u_xlat16_1, u_xlat16_1, u_xlat16_1) * u_xlat4.xyz) + u_xlat10_0.xyz));
          if((u_xlat10_0.w>=0.400000006))
          {
              u_xlatb1 = 1;
          }
          else
          {
              u_xlatb1 = 0;
          }
          u_xlat4.x = (u_xlatb1)?(1):(float(0));
          u_xlat1_d.xzw = (int(u_xlatb1))?(float3(0, 0, 0)):(in_f.color.xyz);
          u_xlat1_d.xyz = float3(((u_xlat0_d.xyz * u_xlat4.xxx) + u_xlat1_d.xzw));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + (-_SpecColor.xyz)));
          u_xlat0_d.xyz = float3(((u_xlat10_0.www * u_xlat0_d.xyz) + _SpecColor.xyz));
          u_xlat10_2 = texCUBE(_IBL, in_f.texcoord.xyz);
          u_xlat1_d.xyz = float3(((u_xlat10_2.xyz * _vector2.zzz) + u_xlat1_d.xyz));
          u_xlat9 = (u_xlat10_2.w * in_f.texcoord2.x);
          u_xlat9 = (u_xlat9 * _vector2.w);
          u_xlat9 = clamp(u_xlat9, 0, 1);
          u_xlat2_d.xyz = float3(((-u_xlat1_d.xyz) + _Color2.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord2.yyy * u_xlat2_d.xyz) + u_xlat1_d.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord2.zzz * u_xlat1_d.xyz) + float3(u_xlat9, u_xlat9, u_xlat9)));
          out_f.color.xyz = float3(((in_f.texcoord.www * u_xlat0_d.xyz) + u_xlat1_d.xyz));
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
    Pass // ind: 1, name: LOW VERSION
    {
      Name "LOW VERSION"
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
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _vector1;
      uniform float4 _vector2;
      uniform float3 _Color0;
      uniform float3 _DecalColor;
      uniform sampler2D _DecalTex;
      uniform samplerCUBE _IBL;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float3 color :COLOR0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 color :COLOR0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float4 u_xlat3;
      float u_xlat12;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.color.xyz = float3(_Color0.xyz);
          u_xlat0.xyz = float3((in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0.xyz = float3(((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
          u_xlat12 = rsqrt(u_xlat12);
          u_xlat1.xyz = float3(((u_xlat0.xyz * float3(u_xlat12, u_xlat12, u_xlat12)) + _WorldSpaceLightPos0.xyz));
          u_xlat0.xyz = float3((float3(u_xlat12, u_xlat12, u_xlat12) * u_xlat0.xyz));
          u_xlat2.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat2.xyz = float3(normalize(u_xlat2.xyz));
          u_xlat1.z = dot(u_xlat2.xyz, u_xlat1.xyz);
          u_xlat1.y = dot(u_xlat0.xyz, u_xlat2.xyz);
          u_xlat1.x = dot(_WorldSpaceLightPos0.xyz, u_xlat2.xyz);
          u_xlat1.xyz = float3(max(u_xlat1.xyz, float3(0, 0, 0)));
          u_xlat3.xy = float2((u_xlat1.xy + u_xlat1.xy));
          u_xlat3.xzw = ((u_xlat3.xxx * u_xlat2.xyz) + (-_WorldSpaceLightPos0.xyz));
          out_v.texcoord.xyz = float3(((u_xlat3.yyy * u_xlat2.xyz) + (-u_xlat0.xyz)));
          u_xlat0.x = dot(u_xlat3.xzw, u_xlat0.xyz);
          u_xlat0.x = (u_xlat0.x * u_xlat1.x);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          out_v.texcoord.w = (u_xlat0.x * 4);
          out_v.texcoord1.xy = float2(in_v.texcoord.xy);
          out_v.texcoord1.zw = in_v.texcoord1.xy;
          u_xlat0.x = (((-u_xlat1.x) * u_xlat1.z) + _vector2.x);
          u_xlat0.x = ((u_xlat0.x * u_xlat0.x) + (-u_xlat1.x));
          u_xlat0.x = ((u_xlat0.x * 0.5) + u_xlat1.x);
          out_v.texcoord2.z = ((u_xlat0.x * 0.5) + 0.100000001);
          u_xlat0.x = (((-_vector1.y) * u_xlat1.y) + 0.5);
          out_v.texcoord2.x = ((-u_xlat1.y) + _vector1.z);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          u_xlat0.x = min(u_xlat0.x, 1);
          out_v.texcoord2.y = (u_xlat0.x + (-0.150000006));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float u_xlat1_d;
      float u_xlat16_1;
      float4 u_xlat10_1;
      float3 u_xlat3_d;
      float u_xlat16_3;
      float u_xlat6;
      int u_xlatb6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_DecalTex, in_f.texcoord1.zw);
          u_xlat16_1 = (u_xlat10_0.w + (-0.5));
          u_xlat16_1 = (u_xlat16_1 * 9.99999809);
          u_xlat16_1 = clamp(u_xlat16_1, 0, 1);
          u_xlat16_3 = ((u_xlat16_1 * (-2)) + 3);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_1);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_3);
          u_xlat3_d.xyz = float3(((_DecalColor.xyz * u_xlat10_0.xyz) + (-u_xlat10_0.xyz)));
          u_xlat0_d.xyz = float3(((float3(u_xlat16_1, u_xlat16_1, u_xlat16_1) * u_xlat3_d.xyz) + u_xlat10_0.xyz));
          if((u_xlat10_0.w>=0.400000006))
          {
              u_xlatb6 = 1;
          }
          else
          {
              u_xlatb6 = 0;
          }
          u_xlat1_d = (u_xlatb6)?(1):(float(0));
          u_xlat3_d.xyz = float3((int(u_xlatb6))?(float3(0, 0, 0)):(in_f.color.xyz));
          u_xlat0_d.xyz = float3(((u_xlat0_d.xyz * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d)) + u_xlat3_d.xyz));
          u_xlat6 = (_vector2.z + 0.5);
          u_xlat10_1 = texCUBE(_IBL, in_f.texcoord.xyz);
          u_xlat0_d.xyz = float3(((u_xlat10_1.xyz * float3(u_xlat6, u_xlat6, u_xlat6)) + u_xlat0_d.xyz));
          u_xlat6 = (u_xlat10_1.w * in_f.texcoord2.x);
          u_xlat6 = (u_xlat6 * _vector2.w);
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat0_d.xyz = float3(((in_f.texcoord2.zzz * u_xlat0_d.xyz) + float3(u_xlat6, u_xlat6, u_xlat6)));
          out_f.color.xyz = float3(((in_f.texcoord.www * _Color0.xyz) + u_xlat0_d.xyz));
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
