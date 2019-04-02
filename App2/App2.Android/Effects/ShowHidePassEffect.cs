using Android.Views;
using Android.Widget; 
using Android.Text.Method;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(App2.Droid.Effects.ShowHidePassEffect), "ShowHidePassEffect")]

namespace App2.Droid.Effects
{
    public class ShowHidePassEffect : PlatformEffect
        {


            protected override void OnAttached()
            {
                ConfigureControl();
            }

            protected override void OnDetached()
            {
            }

            private void ConfigureControl()
            {
                EditText editText = ((EditText)Control);
                editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, App2.Droid.Resource.Drawable.show_pwd, 0);
                editText.SetOnTouchListener(new OnDrawableTouchListener());

            }
        }

        public class OnDrawableTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                if (v is EditText && e.Action == MotionEventActions.Down)
                {
                    EditText editText = (EditText)v;
                    if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
                    {
                        if (editText.TransformationMethod == null)
                        {
                            editText.TransformationMethod = PasswordTransformationMethod.Instance;
                            editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, App2.Droid.Resource.Drawable.show_pwd, 0);
                        }
                        else
                        {
                            editText.TransformationMethod = null;
                            editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, App2.Droid.Resource.Drawable.hide_pwd, 0);
                        }

                        return true;
                    }
                }

            if (v is EditText && e.Action == MotionEventActions.Up)
            {
                EditText editText = (EditText)v;
                if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
                {
                    if (editText.TransformationMethod == null)
                    {
                        editText.TransformationMethod = PasswordTransformationMethod.Instance;
                        editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, App2.Droid.Resource.Drawable.show_pwd, 0);
                    }
                    else
                    {
                        editText.TransformationMethod = null;
                        editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, App2.Droid.Resource.Drawable.hide_pwd, 0);
                    }

                    return true;
                }
            }

            return false;
            }
        }
    }
 