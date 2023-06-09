using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace Checkers
{
    public class ButtonStyles
    {
        private static ButtonStyles instance;

        public Style grayCellStyle { get; set; }
        public Style whiteCellStyle { get; set; }
        public Style clickedCellStyle { get; set; }

        public Style highlightedCellStyle { get; set; }


        private ButtonStyles()
        {
            grayCellStyle = new Style();
            ControlTemplate templateButton = new ControlTemplate(typeof(Button));
            FrameworkElementFactory elemFactory = new FrameworkElementFactory(typeof(Border));
            elemFactory.SetBinding(Border.BackgroundProperty, new Binding { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("Background") });
            templateButton.VisualTree = elemFactory;
            elemFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));

            grayCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Gray });
            grayCellStyle.Setters.Add(new Setter { Property = Button.TemplateProperty, Value = templateButton });
            Trigger styleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            Trigger focusTrigger = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };
            styleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.NavajoWhite });
            focusTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.NavajoWhite });
            grayCellStyle.Triggers.Add(styleTrigger);
            grayCellStyle.Triggers.Add(focusTrigger);




            whiteCellStyle = new Style(typeof(Button));
            ControlTemplate whiteTemplateButton = new ControlTemplate(typeof(Button));

            FrameworkElementFactory whiteElemFactory = new FrameworkElementFactory(typeof(Border));
            whiteElemFactory.SetBinding(Border.BackgroundProperty, new Binding { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("Background") });
            whiteTemplateButton.VisualTree = whiteElemFactory;
            whiteElemFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));

            whiteCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.White });
            whiteCellStyle.Setters.Add(new Setter { Property = Button.TemplateProperty, Value = whiteTemplateButton });
            Trigger whiteStyleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            whiteCellStyle.Triggers.Add(whiteStyleTrigger);



            clickedCellStyle = new Style(typeof(Button), grayCellStyle);
            Trigger clickedStyleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            Trigger keyFocusStyleTrigger = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };

            clickedStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });
            keyFocusStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });


            clickedCellStyle.Triggers.Add(clickedStyleTrigger);
            clickedCellStyle.Triggers.Add(keyFocusStyleTrigger);





            highlightedCellStyle = new Style(typeof(Button), grayCellStyle);
            highlightedCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Blue });
            highlightedCellStyle.Setters.Add(new Setter { Property = Button.CursorProperty, Value = Cursors.Hand });
            Trigger clickedStyleTrigger1 = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            Trigger keyFocusStyleTrigger1 = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };

            clickedStyleTrigger1.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Yellow });
            keyFocusStyleTrigger1.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Yellow });

            highlightedCellStyle.Triggers.Add(clickedStyleTrigger1);
            highlightedCellStyle.Triggers.Add(keyFocusStyleTrigger1);

        }

        public static ButtonStyles Instance //создадим экземпляр, чтобы можно было обращаться к стилям без создания экземпляра через конструктор
        {
            get
            {
                if (instance == null)
                    instance = new ButtonStyles();
                return instance;
            }
        }
    }

}
