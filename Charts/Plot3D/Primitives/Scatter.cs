using nzy3D.Colors;
using nzy3D.Events;
using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Primitives
{
    public class Scatter : AbstractDrawable, ISingleColorable {
        
        private Color[] _colors;
        private Color _color;
        private Coord3d[] _coordinates;
        private float _width;
        
        public Scatter() {
            _bbox = new BoundingBox3d();
            Width = 1;
            Color = Color.BLACK;
        }
        
        public Scatter(Coord3d[] coordinates) : 
                this(coordinates, Color.BLACK) {
        }

        public Scatter(Coord3d[] coordinates, Color rgb, float width = 1) {
            _bbox = new BoundingBox3d();
            Data = coordinates ;
            Width = width;
            Color = rgb;
        }

        public Scatter(Coord3d[] coordinates, Color[] colors, float width = 1) {
            _bbox = new BoundingBox3d();
            Data = coordinates ;
            Width = width;
            Colors = colors;
        }
        
        public void Clear() {
            _coordinates = null;
            _bbox.reset();
        }
        
        public override void Draw(Camera cam)
        {
            
            _transform?.Execute();

            GL.PointSize(_width);
            GL.Begin(BeginMode.Points);
            if (_colors == null)
            {
                GL.Color4(_color.r, _color.g, _color.b, _color.a);
            }

            if (_coordinates != null)
            {
                int k = 0;
                foreach (Coord3d c in _coordinates) {
                    if ((_colors != null)) {
                        GL.Color4(_colors[k].r, _colors[k].g, _colors[k].b, _colors[k].a);
                        k++;
                    }
                    
                    GL.Vertex3(c.x, c.y, c.z);
                    
                }
                
            }
            GL.End();
            
            // doDrawBounds (MISSING)
            
        }
        
        public override Transform.Transform Transform {
            get => _transform;
            set {
                _transform = value;
                UpdateBounds();
            }
        }

        private void UpdateBounds() {
            _bbox.reset();
            foreach (var c in _coordinates) {
                _bbox.add(c);
            }
            
        }

        private Coord3d[] Data
        {
            get => _coordinates;
            set
            {
                _coordinates = value;
                UpdateBounds();
            }
        }

        private Color[] Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
            }
        }

        private float Width
        {
            get => _width;
            set => _width = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }
    }
}