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
    public class Progress<T> : SliderBar<T> where T : struct, IConvertible, IComparable<T>, IEquatable<T>
    {
        public Box box;

        /// <summary>
        /// This is the growth factor of the value e.g => min 0, max 100, start 70 than bindableValue would be 0.7
        /// </summary>
        public Bindable<T> bindableValue { get; private set; } = new Bindable<T>();

        protected const int Multiplier = 1;

        public BindableFloat BoxWidth { get; private set; } = new BindableFloat();

        public bool CurrentlyDragging { get; set; } = false;

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
            BoxWidth.Value = box.Width;
        }

        protected override void UpdateValue(float value)
        {
            if (Current.Value.GetType() == typeof(float))
            {
                var x = (T) Convert.ChangeType(value * Multiplier, TypeCode.Single);
                bindableValue.Value = x;
            }
            else if (Current.Value.GetType() == typeof(int))
            {
                var x = (T) Convert.ChangeType(Convert.ToInt32(value * Multiplier), TypeCode.Int32);
                bindableValue.Value = x;
            }

            box.Width = value;
            BoxWidth.Value = box.Width;
        }

        protected override bool OnDragStart(DragStartEvent e) => CurrentlyDragging = true;

        protected override bool OnDragEnd(DragEndEvent e) => CurrentlyDragging = false;
    }
}
