using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Prototipo
{
    public class CardView : ContentView
    {
        public Label Name { get; set; }
        public Image Photo { get; set; }
        public Label Price { get; set; }

        public CardView()
        {
            RelativeLayout view = new RelativeLayout();

            BoxView boxview1 = new BoxView { Color = Color.FromHex("26CAD3"), InputTransparent = true };

            view.Children.Add(boxview1,
                              Constraint.Constant(0),
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height;
                              })
                              );

            Photo = new Image { InputTransparent = true, Aspect = Aspect.AspectFit };

            view.Children.Add(Photo,
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  double h = parent.Height * 0.80;
                                  return ((parent.Height - h) / 2 + 50);
                              }),
                              Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.RelativeToParent((parent) => { return parent.Height; })
                              );

            Name = new Label
            {
                TextColor = Color.White,
                FontSize = 22,
                InputTransparent = true
            };

            view.Children.Add(Name,
                              Constraint.Constant(10),
                              Constraint.Constant(10),
                              Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.Constant(28));

            Price = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                InputTransparent = true
            };

            view.Children.Add(Price,
                              Constraint.Constant(10),
                              Constraint.Constant(40),
                              Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.Constant(28));

            Content = view;
        }
    }
}
