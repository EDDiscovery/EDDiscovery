using nzy3D.Colors;
using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Primitives
{
    public class ScatterMultiColor : AbstractDrawable, IMultiColorable
    {

        private Coord3d[] _coordinates;
        private float _width;
        private ColorMapper _mapper;

        public ScatterMultiColor(Coord3d[] coordinates, ColorMapper mapper, float width = 1.0f)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            Width = width;
            ColorMapper = mapper;
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

            if (_coordinates != null)
            {
                foreach (Coord3d c in _coordinates) {
                    var color = _mapper.Color(c); // TODO: should store result in the point color
                    GL.Color4(color.r, color.g, color.b, color.a);
                    GL.Vertex3(c.x, c.y, c.z);
                }
            }
            GL.End();
            
            // doDrawBounds (MISSING)
            
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
        
        public override Transform.Transform Transform {
            get => _transform;
            set {
                _transform = value;
                UpdateBounds();
            }
        }

        private float Width
        {
            get => _width;
            set => _width = value;
        }

        public ColorMapper ColorMapper
        {
            get => _mapper;
            set => _mapper = value;
        }
    }
}