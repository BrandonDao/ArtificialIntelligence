using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Forms.NET.Components;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Visualizer
{
    public partial class VisualizerDisplay : MonoGame.Forms.NET.Controls.MonoGameControl
    {
        Camera2D camera;

        Dictionary<Keys, List<Action>> KeybindMap;

        Texture2D squareTexture;

        protected override void Initialize()
        {
            Editor.RemoveDefaultComponents();

            KeybindMap = [];
            AddKeybind(Keys.W, () => camera.Move(Vector2.UnitY));
            AddKeybind(Keys.A, () => camera.Move(-Vector2.UnitX));
            AddKeybind(Keys.S, () => camera.Move(-Vector2.UnitY));
            AddKeybind(Keys.D, () => camera.Move(-Vector2.UnitX));

            camera = new(Editor.GraphicsDevice);

            squareTexture = new Texture2D(Editor.GraphicsDevice, width: 1, height: 1);
            squareTexture.SetData([Color.White]);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
         
            foreach(var keys in keyboardState.GetPressedKeys())
            {

            }
        }

        protected override void Draw()
        {
            Editor.GraphicsDevice.Clear(Color.LightGray);

            Editor.spriteBatch.Begin(transformMatrix: camera.GetTransform());

            Editor.spriteBatch.Draw(squareTexture, new Rectangle(100, 100, 10, 10), Color.Red);

            Editor.spriteBatch.End();
        }

        private void AddKeybind(Keys key, Action action)
        {
            if (KeybindMap.TryGetValue(key, out List<Action>? actions))
            {
                actions.Add(action);
            }
            else
            {
                KeybindMap.Add(key, [action]);
            }
        }
    }
}