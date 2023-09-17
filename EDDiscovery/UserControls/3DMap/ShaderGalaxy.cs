/*
 * Copyright 2019-2021 Robbyxp1 @ github.com
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using GLOFC.GL4;
using GLOFC.GL4.Shaders;
using GLOFC.GL4.Shaders.Volumetric;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace EDDiscovery.UserControls.Map3D
{
    public class GalaxyFragmentPipeline : GLShaderPipelineComponentShadersBase
    {
        string fcode = @"
#version 460 core
out vec4 color;

in vec3 vs_texcoord;

const int galtexbinding = 1;
const int noisebinding = 3;
const int gaussiandistbinding = 4;

layout (binding=galtexbinding) uniform sampler2D tex;
layout (binding=noisebinding) uniform sampler3D noise;     
layout (binding=gaussiandistbinding) uniform sampler1D gaussian;  

layout (location=10) uniform float fadein;

void main(void)
{
    if ( fadein < 0 )
    {
        vec4 c = texture(tex,vec2(vs_texcoord.x,vs_texcoord.z)); 
        color = vec4(c.xyz,1.0);     
    }
    else
    {
        float dx = abs(0.5-vs_texcoord.x);
        float dz = abs(0.5-vs_texcoord.z);
        float d = 0.7073-sqrt(dx*dx+dz*dz);     // 0 - 0.7
        d = d / 0.7073; // 0.707 is the unit circle, 1 is the max at corners

        if ( d > (1-0.707) )               // limit to circle around centre
        {
            vec4 c = texture(tex,vec2(vs_texcoord.x,vs_texcoord.z)); 
            float brightness = sqrt(c.x*c.x+c.y*c.y+c.z*c.z)/1.733;         // 0-1

            if ( brightness > 0.001 )
            {
                float g = texture(gaussian,d).x;      // look up single sided gaussian function, 0-1
                float h = abs(vs_texcoord.y-0.5)*2;     // 0-1 also

                if ( g > h )
                {
                    float nv = texture(noise,vs_texcoord.xyz).x;

                    float alpha = min(max(brightness,0.5),(brightness>0.05) ? 0.3 : brightness);    // beware the 8 bit alpha (0.0039 per bit).
                    color = vec4(c.xyz*(1.0+nv*0.2) * fadein,alpha*(1.0+nv*0.1) * fadein);        // noise adjusting brightness and alpha a little
                }
                else 
                    discard;
            }
            else
                discard;
        }
        else
            discard;
    }

}
            ";

        public GalaxyFragmentPipeline(int galtexbinding = 1, int noisebinding = 3, int gaussiandistbinding = 4)
        {
            CompileLink(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader, fcode, out string unusedcompilerreport, new object[] { "galtexbinding", galtexbinding, "noisebinding", noisebinding, "gaussiandistbinding",gaussiandistbinding });
        }

        // use -1 to indicate no fade/alpha out

        public void SetFader(Vector3 eyepos, float eyedistance, bool perspective)
        {
            if (!perspective)
            {
                GL.ProgramUniform1(this.Id, 10, -1);
            }
            else
            {
                float disttocentre = (float)Math.Sqrt(eyepos.X * eyepos.X + (eyepos.Z - 26100) * (eyepos.Z - 26100));
                float centrev = disttocentre > 30000 ? (disttocentre - 30000) / 5000.0f : 0;         // increases as distance incr
                float vertr = eyepos.Y > 1500 ? (eyepos.Y - 1500) / 500 : 0;                    // increases as vert incr
                float eyedr = (eyedistance > 1000) ? (eyedistance - 1000) / 500 : 0;
                float value = Math.Min(centrev + vertr + eyedr, 1);
                // System.Diagnostics.Debug.WriteLine($"Galbrightness {eyepos} {eyedistance} {disttocentre} : {centrev} {vertr} {eyedr} = {value}");
                GL.ProgramUniform1(this.Id, 10, value);
            }
        }
    }

    public class GalaxyShader : GLShaderPipeline
    {
        private GalaxyFragmentPipeline frag;

        public GalaxyShader(int volumetricbinding = 1, int galtexbinding = 1, int noisebinding = 3, int gaussiandistbinding = 4)
        {
            Add(new GLPLVertexShaderVolumetric(), OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            Add(new GLPLGeometricShaderVolumetric(volumetricbinding), OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
            frag = new GalaxyFragmentPipeline(galtexbinding,noisebinding, gaussiandistbinding);
            Add(frag, OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);
        }

        public void SetFader(Vector3 eyepos, float eyedistance, bool perspective)
        {
            frag.SetFader(eyepos, eyedistance, perspective);
        }
    }

}
