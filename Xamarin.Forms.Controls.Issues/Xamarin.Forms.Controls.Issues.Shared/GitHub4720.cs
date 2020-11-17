﻿using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Threading;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Controls.GalleryPages;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.WebView)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 4720, "UWP: Webview: Memory Leak in WebView", PlatformAffected.UWP)]
	public class GitHub4720 : TestNavigationPage
	{
		//shameless copy of the test case for Bugzilla39489.

		protected override void Init()
		{
			PushAsync(new GitHub4720Content());
		}

#if UITEST
		protected override bool Isolate => true;

		[Test]
		public async Task GitHub4720Test()
		{
			//4 iterations were enough to run out of memory before the fix.
			int iterations = 10;

			for (int n = 0; n < iterations; n++)
			{
				RunningApp.WaitForElement(q => q.Marked("New Page"));
				RunningApp.Tap(q => q.Marked("New Page"));
				RunningApp.WaitForElement(q => q.Marked("Close Page"));
				await Task.Delay(2000);
				RunningApp.Tap(q => q.Marked("Close Page"));
			}
		}

#endif
	}
	public class _4730WebView : WebView
	{
	}
	public class GitHub4720WebPage : ContentPage
	{
		static int s_count;

		public GitHub4720WebPage()
		{
			Interlocked.Increment(ref s_count);
			Debug.WriteLine($"++++++++ GitHub4720WebPage : Constructor, count is {s_count}");

			var label = new Label { Text = "Test case for GitHub issue #4720 -  sorry youtube." };

			var button = new Button { Text = "Close Page" };
			button.Clicked += Button_Clicked;

			var wv = new _4730WebView()
			{				
				Source = new UrlWebViewSource { Url = "https://www.youtube.com/" },
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Red
				
			};
			Content = new StackLayout { Children = { label, button, wv } };
		}
		void Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		~GitHub4720WebPage()
		{
			Interlocked.Decrement(ref s_count);
			Debug.WriteLine($"-------- GitHub4720WebPage: Destructor, count is {s_count}");
		}
	}

	[Preserve(AllMembers = true)]
	public class GitHub4720Content : ContentPage
	{
		static int s_count;

		public GitHub4720Content()
		{
			Interlocked.Increment(ref s_count);
			Debug.WriteLine($">>>>> GitHub4720Content GitHub4720Content : Constructor, count is {s_count}");

			var button = new Button { Text = "New Page" };
			button.Clicked += Button_Clicked;

			var gcbutton = new Button { Text = "GC" };
			gcbutton.Clicked += GCbutton_Clicked;

			Content = new StackLayout { Children = { button, gcbutton } };
		}

		void GCbutton_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(">>>>>>>> Running Garbage Collection");
			GarbageCollectionHelper.Collect();
			System.Diagnostics.Debug.WriteLine($">>>>>>>> GC.GetTotalMemory = {GC.GetTotalMemory(true):n0}");
		}

		void Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new GitHub4720WebPage());
		}

		~GitHub4720Content()
		{
			Interlocked.Decrement(ref s_count);
			Debug.WriteLine($">>>>> GitHub4720Content ~GitHub4720Content : Destructor, count is {s_count}");
		}
	}


}
