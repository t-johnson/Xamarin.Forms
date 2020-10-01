using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using AndroidX.AppCompat.Widget;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class ButtonExtensions
	{
		static float DefaultFontSize;
		static Typeface? DefaultTypeface;

		public static void UpdateText(this AppCompatButton appCompatButton, IButton button)
		{
			appCompatButton.Text = button.Text;
		}

		public static void UpdateColor(this AppCompatButton button, Forms.Color color, ColorStateList? defaultColor)
		{
			if (color.IsDefault)
				button.SetTextColor(defaultColor);
			else
				button.SetTextColor(color.ToNative());
		}

		public static void UpdateColor(this AppCompatButton appCompatButton, IButton button)
		{
			appCompatButton.UpdateColor(button.Color, appCompatButton.TextColors);
		}

		public static void UpdateColor(this AppCompatButton appCompatButton, IButton button, Forms.Color defaultColor)
		{
			appCompatButton.SetTextColor(button.Color.Cleanse(defaultColor).ToNative());
		}

		public static void UpdateFont(this AppCompatButton appCompatButton, IButton button)
		{
			Font font = button.Font;

			if (font == Font.Default && DefaultFontSize == 0f)
				return;

			if (DefaultFontSize == 0f)
			{
				DefaultTypeface = appCompatButton.Typeface;
				DefaultFontSize = appCompatButton.TextSize;
			}

			if (font == Font.Default)
			{
				appCompatButton.Typeface = DefaultTypeface;
				appCompatButton.SetTextSize(ComplexUnitType.Px, DefaultFontSize);
			}
			else
			{
				appCompatButton.Typeface = font.ToTypeface();
				appCompatButton.SetTextSize(ComplexUnitType.Sp, font.ToScaledPixel());
			}
		}

		public static void UpdateCharacterSpacing(this AppCompatButton appCompatButton, IButton button)
		{
			if (NativeVersion.IsAtLeast(21))
			{
				appCompatButton.LetterSpacing = button.CharacterSpacing.ToEm();
			}
		}

		public static void UpdateCornerRadius(this AppCompatButton appCompatButton, IButton button)
		{

		}

		public static void UpdateBorderColor(this AppCompatButton appCompatButton, IButton button)
		{

		}

		public static void UpdateBorderWidth(this AppCompatButton appCompatButton, IButton button)
		{

		}

		public static void UpdateContentLayout(this AppCompatButton appCompatButton, IButton button)
		{

		}

		public static void UpdatePadding(this AppCompatButton appCompatButton, IButton button)
		{

		}

		static Forms.Color Cleanse(this Forms.Color color, Forms.Color defaultColor) => color.IsDefault ? defaultColor : color;
	}
}
