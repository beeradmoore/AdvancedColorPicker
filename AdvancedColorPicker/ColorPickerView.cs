using System;
using CoreGraphics;
using UIKit;

namespace AdvancedColorPicker
{
    public class ColorPickerView : UIView
    {
        public event Action ColorPicked;

        CGSize satBrightIndicatorSize = new CGSize(28, 28);
        HuePickerView huewView = new HuePickerView();
        SaturationBrightnessPickerView satbrightview = new SaturationBrightnessPickerView();
        //SelectedColorPreviewView selPrevView = new SelectedColorPreviewView();
        HueIndicatorView huewIndicatorView = new HueIndicatorView();
        SatBrightIndicatorView satBrightIndicatorView = new SatBrightIndicatorView();


        public ColorPickerView(CGRect frame)
            : base(frame)
        {
            BackgroundColor = UIColor.White;

            float selectedColorViewHeight = 44;

            float viewSpace = 1;

            /*
			selPrevView.Frame = new RectangleF(0,0,this.View.Bounds.Width,selectedColorViewHeight);
			selPrevView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			selPrevView.Layer.ShadowOpacity = 0.6f;
			selPrevView.Layer.ShadowOffset = new SizeF(0,7);
			selPrevView.Layer.ShadowColor = UIColor.Black.CGColor;
			*/


            //to megalo view epilogis apoxrwsis tou epilegmenou xrwmats
            satbrightview.Frame = new CGRect(0, 0, Frame.Width, Frame.Height - selectedColorViewHeight - viewSpace);
            satbrightview.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            satbrightview.ColorPicked += HandleColorPicked;
            satbrightview.AutosizesSubviews = true;


            //to mikro view me ola ta xrwmata
            huewView.Frame = new CGRect(0, Frame.Height - selectedColorViewHeight, Frame.Width, selectedColorViewHeight);
            huewView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;
            huewView.HueChanged += HandleHueChanged;

	
            huewIndicatorView.huePickerViewRef = huewView;
            float pos = (float)(huewView.Frame.Width * huewView.Hue);
            huewIndicatorView.Frame = new CGRect(pos - 10, huewView.Bounds.Y - 2, 20, huewView.Bounds.Height + 2);
            huewIndicatorView.UserInteractionEnabled = false;
            huewIndicatorView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
            huewView.AddSubview(huewIndicatorView);


            satBrightIndicatorView.satBrightPickerViewRef = satbrightview;
            CGPoint pos2 = new CGPoint(satbrightview.saturation * satbrightview.Frame.Size.Width, 
                               satbrightview.Frame.Size.Height - (satbrightview.brightness * satbrightview.Frame.Size.Height));
            satBrightIndicatorView.Frame = new CGRect(pos2.X - satBrightIndicatorSize.Width / 2, pos2.Y - satBrightIndicatorSize.Height / 2, satBrightIndicatorSize.Width, satBrightIndicatorSize.Height);
            satBrightIndicatorView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin;
            satBrightIndicatorView.UserInteractionEnabled = false;
            satbrightview.AddSubview(satBrightIndicatorView);

            AddSubviews(new UIView[] { satbrightview, huewView }); //selPrevView


        }

        void PositionSatBrightIndicatorView()
        {
            UIView.Animate(0.3f, 0f, UIViewAnimationOptions.AllowUserInteraction, delegate()
                {
                    CGPoint pos = new CGPoint(satbrightview.saturation * satbrightview.Frame.Size.Width, 
                                      satbrightview.Frame.Size.Height - (satbrightview.brightness * satbrightview.Frame.Size.Height));
                    satBrightIndicatorView.Frame = new CGRect(pos.X - satBrightIndicatorSize.Width / 2, pos.Y - satBrightIndicatorSize.Height / 2, satBrightIndicatorSize.Width, satBrightIndicatorSize.Height);
                }, delegate()
                {
                });
        }

        void PositionHueIndicatorView()
        {
            UIView.Animate(0.3f, 0f, UIViewAnimationOptions.AllowUserInteraction, delegate()
                {
                    float pos = (float)(huewView.Frame.Width * huewView.Hue);
                    huewIndicatorView.Frame = new CGRect(pos - 10, huewView.Bounds.Y - 2, 20, huewView.Bounds.Height + 2);
                }, delegate()
                {
                    huewIndicatorView.Hidden = false;
                });
        }

        void HandleColorPicked()
        {
            PositionSatBrightIndicatorView();
            //selPrevView.BackgroundColor = UIColor.FromHSB (satbrightview.hue, satbrightview.saturation, satbrightview.brightness);
            Console.WriteLine(UIColor.FromHSB(satbrightview.hue, satbrightview.saturation, satbrightview.brightness));

            if (ColorPicked != null)
            {
                ColorPicked();
            }
        }

        void HandleHueChanged()
        {
            PositionHueIndicatorView();
            satbrightview.hue = huewView.Hue;
            satbrightview.SetNeedsDisplay();
            HandleColorPicked();
        }


        void PositionIndicators()
        {
            PositionHueIndicatorView();
            PositionSatBrightIndicatorView();
        }


        public UIColor SelectedColor
        {
            get
            {
                return UIColor.FromHSB(satbrightview.hue, satbrightview.saturation, satbrightview.brightness);
            }
            set
            {
                nfloat hue = 0, brightness = 0, saturation = 0, alpha = 0;
                value.GetHSBA(out hue, out saturation, out brightness, out alpha);
                huewView.Hue = (float)hue;
                satbrightview.hue = (float)hue;
                satbrightview.brightness = (float)brightness;
                satbrightview.saturation = (float)saturation;
                //selPrevView.BackgroundColor = value;

                PositionIndicators();

                satbrightview.SetNeedsDisplay();
                huewView.SetNeedsDisplay();
            }
        }
    }
}

