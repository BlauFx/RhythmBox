using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;

namespace RhythmBox.Window.Objects
{
    /// <summary>
    /// <typeparamref name="T"/> only supports float and int.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Progress<T> : SliderBar<T> where T : struct, IConvertible, IComparable
    {
        public Action Action;

        private Box box;

        public bool AllowAction = false;

        /// <summary>
        /// This is the growth factor of the value e.g => min 0, max 100, start 70 than bindableValue would be 0.7
        /// </summary>
        public Bindable<T> bindableValue = new Bindable<T>();

        protected const int Multiplier = 1;

        public Progress(T MinValue, T MaxValue, T StartValue)
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Depth = 1,
                    Colour = Color4.Cyan,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
                box = new Box
                {
                    Depth = 0,
                    Colour = Color4.Gray,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f),
                },
            };

            CurrentNumber.MaxValue = MaxValue;
            CurrentNumber.MinValue = MinValue;
            Current.Value = StartValue;

            box.Width = Convert.ToSingle(Current.Value);

        }

        protected override void UpdateValue(float value)
        {
            if (AllowAction)
            {
                Action?.Invoke();
            }

            if (Current.Value.GetType() == typeof(float))
            {
                var x = (T) Convert.ChangeType(value* Multiplier, TypeCode.Single);
                bindableValue.Value = x;
            }
            else if (Current.Value.GetType() == typeof(int))
            {
                var x = (T) Convert.ChangeType(Convert.ToInt32(value* Multiplier), TypeCode.Int32);
                bindableValue.Value = x;
            }

            box.Width = value;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (this.IsHovered)
            {
                if (e.Key == osuTK.Input.Key.Left)
                {
                    if (Current.Value.GetType() == typeof(float))
                    {
                        var x = (T) Convert.ChangeType(Convert.ToSingle(Current.Value) - 0.01f, TypeCode.Single);
                        Current.Value = x;
                    }
                    else if (Current.Value.GetType() == typeof(int))
                    {
                        var x = (T) Convert.ChangeType(Convert.ToInt32(Current.Value) - 1, TypeCode.Int32);
                        Current.Value = x;
                    }
                }
                else if (e.Key == osuTK.Input.Key.Right)
                {
                    if (Current.Value.GetType() == typeof(float))
                    {
                        var x = (T) Convert.ChangeType(Convert.ToSingle(Current.Value) + 0.01f, TypeCode.Single);
                        Current.Value = x;
                    }
                    else if (Current.Value.GetType() == typeof(int))
                    {
                        var x = (T) Convert.ChangeType(Convert.ToInt32(Current.Value) + 1, TypeCode.Int32);
                        Current.Value = x;
                    }
                }
            }
            return false;
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            if (e.ScrollDelta == new Vector2(0, 1)) //Up
            {
                if (Current.Value.GetType() == typeof(float))
                {
                    var x = (T) Convert.ChangeType(Convert.ToSingle(Current.Value) - 0.01f, TypeCode.Single);
                    Current.Value = x;
                }
                else if (Current.Value.GetType() == typeof(int))
                {
                    var x = (T) Convert.ChangeType(Convert.ToInt32(Current.Value) - 1, TypeCode.Int32);
                    Current.Value = x;
                }
            }
            else if (e.ScrollDelta == new Vector2(0, -1)) //Down
            {
                if (Current.Value.GetType() == typeof(float))
                {
                    var x = (T) Convert.ChangeType(Convert.ToSingle(Current.Value) + 0.01f, TypeCode.Single);
                    Current.Value = x;
                }
                else if (Current.Value.GetType() == typeof(int))
                {
                    var x = (T) Convert.ChangeType(Convert.ToInt32(Current.Value) + 1, TypeCode.Int32);
                    Current.Value = x;
                }
            }

            return base.OnScroll(e);
        }
    }
}
