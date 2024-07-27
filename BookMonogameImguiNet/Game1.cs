using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BookMonogameImguiNet
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private BasicEffect _basicEffect;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set up the camera
            _world = Matrix.CreateTranslation(Vector3.Zero);
            _view = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize the BasicEffect
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                World = _world,
                View = _view,
                Projection = _projection,
                VertexColorEnabled = true
            };

            // Create cube geometry
            CreateCube();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the cube's world matrix to rotate it
            _world = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply the updated world matrix
            _basicEffect.World = _world;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SetVertexBuffer(_vertexBuffer);
                GraphicsDevice.Indices = _indexBuffer;
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }

            base.Draw(gameTime);
        }

        private void CreateCube()
        {
            var vertices = new VertexPositionColor[]
            {
            new VertexPositionColor(new Vector3(-1, 1, -1), Color.Red),
            new VertexPositionColor(new Vector3(1, 1, -1), Color.Green),
            new VertexPositionColor(new Vector3(1, -1, -1), Color.Blue),
            new VertexPositionColor(new Vector3(-1, -1, -1), Color.Yellow),
            new VertexPositionColor(new Vector3(-1, 1, 1), Color.Cyan),
            new VertexPositionColor(new Vector3(1, 1, 1), Color.Magenta),
            new VertexPositionColor(new Vector3(1, -1, 1), Color.White),
            new VertexPositionColor(new Vector3(-1, -1, 1), Color.Black)
            };

            _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            var indices = new ushort[]
            {
            // Front face
            0, 1, 2,
            0, 2, 3,
            // Back face
            4, 6, 5,
            4, 7, 6,
            // Top face
            4, 5, 1,
            4, 1, 0,
            // Bottom face
            3, 2, 6,
            3, 6, 7,
            // Left face
            4, 0, 3,
            4, 3, 7,
            // Right face
            1, 5, 6,
            1, 6, 2
            };

            _indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
    }
}
