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

namespace EDDiscovery.UserControls.Map3D
{
    public class GalaxyStarDots : GLShaderStandard
    {
        string vert =
@"
        #version 450 core

        #include UniformStorageBlocks.matrixcalc.glsl

        layout (location = 0) in vec4 position;     // has w=1
        out vec4 vs_color;

        void main(void)
        {
            vec4 p = position;
            vs_color = vec4(position.w,position.w,position.w,0.15);
            p.w = 1;
            gl_Position = mc.ProjectionModelMatrix * p;        // order important
        }
        ";
        string frag =
@"
        #version 450 core

        in vec4 vs_color;
        out vec4 color;

        void main(void)
        {
            color = vs_color;
        }
        ";
        public GalaxyStarDots() : base()
        {
            CompileLink(vert, frag: frag);
        }
    }


}
